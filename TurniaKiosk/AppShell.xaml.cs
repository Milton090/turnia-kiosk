using Services.Bixolon;
using Services.Constants;
using Services.Queue;
using TurniaKiosk.MauiServices;
using System.Diagnostics;

namespace TurniaKiosk
{
    public partial class AppShell : Shell
    {
        private readonly QueueClientService _queueClientService;
        private readonly BixolonService _bixolonService;

        private bool _preferencesApplied;

        public AppShell(
            QueueClientService queueClientService,
            BixolonService bixolonService)
        {
            InitializeComponent();

            _queueClientService = queueClientService;
            _bixolonService = bixolonService;

            if (_preferencesApplied == false)
            {
                var appPreferences = GetAppPreferences();
                ApplyPreferencesToQueueClientService(appPreferences.Item1, appPreferences.Item2);
                _bixolonService.PrinterDisabled = false;
                _preferencesApplied = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                await InitialDataLoadAsync();
            });
        }

        private async Task InitialDataLoadAsync()
        {
            try
            {
                _queueClientService.ServiceReady = true;

                var resultDevConfig = await _queueClientService.GetCheckinDeviceConfiguration();

                if (resultDevConfig.Item1 == false)
                {
                    _queueClientService.ServiceReady = false;
                }

                var resultServices = await _queueClientService.GetServices();
                if (resultServices.Item1 == false)
                {
                    _queueClientService.ServiceReady = false;
                }

                var resultPrinterReady = _bixolonService.IsPrinterReady();

                if (_queueClientService.ServiceReady == false)
                {
                    Debug.WriteLine("[AppShell] ServiceReady = false");
                }

                #region Color
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AppInitializationService.ApplyColorCustomizations(ConstantDefinitions.PrimaryColor);
                });
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AppShell] Error en InitialDataLoadAsync: {ex.Message}");
            }
        }

        private (string, string) GetAppPreferences()
        {
            LocalPreferencesService.SetAppUrl(ConstantDefinitions.BaseUrl);
            LocalPreferencesService.SetDeviceKey(ConstantDefinitions.DeviceKey);

            var appUrl = LocalPreferencesService.GetAppUrl();
            var deviceKey = LocalPreferencesService.GetDeviceKey();

            return (appUrl, deviceKey!);
        }

        private void ApplyPreferencesToQueueClientService(string appUrl, string deviceKey)
        {
            _queueClientService.SetHttpClientUrl(appUrl);
            _queueClientService.SetDeviceKeyHeader(deviceKey);
        }
    }
}
