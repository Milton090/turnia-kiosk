using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Dispatching;
using Services.Constants;
using TurniaKiosk.Helpers.Paging;
using Services.Queue;
using Services.Queue.Dtos;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TurniaKiosk.NavigationData.Pages;
using TurniaKiosk.Pages;


namespace TurniaKiosk.ViewModels.Pages
{
    public partial class ServiceSelectionViewModel : ObservableObject
    {
        private readonly QueueClientService _queueClientService;

        public ObservableCollection<ServiceDto> KioskServices { get; } = new();

        [ObservableProperty] private string _emptyStateMessage = string.Empty;

        public PagedItems<ServiceDto> Pager { get; }

        public ServiceSelectionViewModel(QueueClientService queueClientService)
        {
            _queueClientService = queueClientService;
            Pager = new PagedItems<ServiceDto>(ConstantDefinitions.ServicesPerPage);
            SelectService = new Command<Guid>(
               execute: (Guid arg) =>
               {
                   SelectServiceCommand(arg);
               });
        }

        private IDispatcherTimer? _pollTimer;
        private const int PollSeconds = 30;

        public async Task InitializeAsync()
        {
            if (_queueClientService._deviceConfigurationCheckin == null)
            {
                await _queueClientService.GetCheckinDeviceConfiguration();
            }

            await _queueClientService.GetServices();
            LoadServices();
            StartPolling();
        }

        public void LoadServices()
        {
            KioskServices.Clear();

            #region Load Services
            var services = _queueClientService._services?.Services ?? new List<ServiceDto>();
            foreach (var service in services)
            {
                KioskServices.Add(service);
            }
            #endregion

            EmptyStateMessage = _queueClientService.ServicesAvailability == DataAvailability.Unavailable
                ? "No se pudieron cargar los servicios. Revise la conexión e intente de nuevo."
                : "No hay servicios disponibles en este momento.";

            Pager.SetItems(KioskServices);
        }

        private void StartPolling()
        {
            if (_pollTimer != null)
                return;

            _pollTimer = Application.Current?.Dispatcher.CreateTimer();
            if (_pollTimer == null)
                return;

            _pollTimer.Interval = TimeSpan.FromSeconds(PollSeconds);
            _pollTimer.Tick += async (s, e) =>
            {
                await _queueClientService.GetServices();
                await MainThread.InvokeOnMainThreadAsync(LoadServices);
            };
            _pollTimer.Start();
        }

        public void StopPolling()
        {
            _pollTimer?.Stop();
            _pollTimer = null;
        }


        #region Commands
        public ICommand SelectService { private set; get; } = null!;
        public async void SelectServiceCommand(Guid serviceId)
        {
            Debugger.Log(1, null, $"Service Id: {serviceId} selected\r\n");

            var service = KioskServices.Where(x => x.Id == serviceId).FirstOrDefault();
            if (service != null)
            {
                StopPolling();

                var navData = new ServiceSelectionNavigationData
                {
                    SelectedService = serviceId
                };
                var dictionary = new Dictionary<string, object>() { { "Service", navData } };

                await MainThread.InvokeOnMainThreadAsync(async () => { await Shell.Current.GoToAsync($"{nameof(TicketCreation)}", dictionary); });
            }
        }
        #endregion
    }
}
