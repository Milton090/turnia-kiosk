using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Services.Queue;
using TurniaKiosk.Helpers.Navigation;
using TurniaKiosk.NavigationData.Pages;

namespace TurniaKiosk.ViewModels.Pages
{
    [QueryProperty(nameof(Data), "Data")]
    public partial class WalkInBlockedViewModel : ObservableObject
    {
        private const int AutoAdvanceSeconds = 15;

        private CancellationTokenSource? _cts;
        private int _navigated;

        #region Navigation Data
        private WalkInBlockedNavigationData _data = null!;
        public WalkInBlockedNavigationData Data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged(nameof(Data));
                Load();
            }
        }
        #endregion

        [ObservableProperty] private string _title = "No disponible";
        [ObservableProperty] private string _message = string.Empty;
        [ObservableProperty] private string _buttonText = "Volver al inicio";
        [ObservableProperty] private string _countdownText = string.Empty;

        public ICommand OkCommand { get; }

        private readonly QueueClientService _queueClientService;

        public WalkInBlockedViewModel(QueueClientService queueClientService)
        {
            _queueClientService = queueClientService;
            OkCommand = new Command(async () => await GoHomeAsync());
        }

        private void Load()
        {
            Title = string.IsNullOrWhiteSpace(Data?.Title) ? "No disponible" : Data!.Title;
            Message = Data?.Message ?? string.Empty;
            _navigated = 0;
            StartCountdown();
        }

        private void StartCountdown()
        {
            CancelCountdown();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _ = Task.Run(async () =>
            {
                try
                {
                    for (var i = AutoAdvanceSeconds; i > 0; i--)
                    {
                        token.ThrowIfCancellationRequested();
                        var seconds = i;
                        await MainThread.InvokeOnMainThreadAsync(() =>
                            CountdownText = $"Volviendo al inicio en {seconds} s");
                        await Task.Delay(1000, token);
                    }

                    await GoHomeAsync();
                }
                catch (OperationCanceledException) { }
            }, token);
        }

        private void CancelCountdown()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        private async Task GoHomeAsync()
        {
            if (Interlocked.Exchange(ref _navigated, 1) == 1)
                return;

            CancelCountdown();
            await KioskNavigationHelper.GoToStartAsync(_queueClientService);
        }
    }
}
