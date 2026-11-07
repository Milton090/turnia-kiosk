using Services.Queue;
using TurniaKiosk.Pages;

namespace TurniaKiosk.Helpers.Navigation
{
    public static class KioskNavigationHelper
    {
        public static async Task GoToStartAsync(QueueClientService queueClientService)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync(nameof(ServiceSelection));
            });
        }
    }
}
