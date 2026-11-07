using CommunityToolkit.Mvvm.ComponentModel;
using Services.Queue;
using Services.Queue.Dtos;
using TurniaKiosk.NavigationData.Pages;
using TurniaKiosk.Helpers.Navigation;
using TurniaKiosk.Pages;

namespace TurniaKiosk.ViewModels.Pages
{
    [QueryProperty(nameof(ServiceNavData), "Service")]
    public partial class TicketCreationViewModel : ObservableObject
    {
        private readonly QueueClientService _queueClientService;

        #region Navigation Data
        private ServiceSelectionNavigationData _serviceNavData = null!;
        public ServiceSelectionNavigationData ServiceNavData
        {
            set
            {
                _serviceNavData = value;

                _ = CreateTicketAsync();
            }
            get
            {
                return _serviceNavData;
            }
        }
        #endregion
        [ObservableProperty]
        private string _ticketNumber = string.Empty;
        [ObservableProperty]
        private bool _isVisible;

        public TicketCreationViewModel(QueueClientService queueClientService)
        {
            _queueClientService = queueClientService;
        }


        public async Task CreateTicketAsync()
        {
            TicketNumber = string.Empty;

            var ticket = new AddTicketDto();
            ticket.DeviceId = _queueClientService._deviceConfigurationCheckin!.Id;
            ticket.DeviceKey = _queueClientService._deviceConfigurationCheckin.DeviceKey;
            ticket.Service = _serviceNavData.SelectedService;

            var resultTicketCreation = await _queueClientService.CreateTicketAsync(ticket);
            if (resultTicketCreation.Item1 && resultTicketCreation.Item2 != null)
            {
                TicketNumber = resultTicketCreation.Item2.TicketNumber;
                IsVisible = true;

                var result = _queueClientService.PrintTicket(resultTicketCreation.Item2);
                if (result)
                {
                    var markAsPrintedRes = await _queueClientService.MarkTicketAsPrintedAsync(resultTicketCreation.Item2, ticket.DeviceKey);
                }
            }
            else
            {
                var reason = string.IsNullOrWhiteSpace(resultTicketCreation.Item3)
                    ? "Este servicio no está disponible en este momento."
                    : resultTicketCreation.Item3;

                var blockedData = new WalkInBlockedNavigationData { Title = "No disponible", Message = reason };
                await MainThread.InvokeOnMainThreadAsync(async () =>
                    await Shell.Current.GoToAsync(
                        $"{nameof(WalkInBlockedPage)}",
                        new Dictionary<string, object> { { "Data", blockedData } }));
                return;
            }

            await Task.Delay(3000);

            await KioskNavigationHelper.GoToStartAsync(_queueClientService);

        }

    }
}
