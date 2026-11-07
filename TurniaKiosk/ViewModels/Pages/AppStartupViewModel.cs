using CommunityToolkit.Mvvm.ComponentModel;
using Services.Queue;
using System.Windows.Input;
using TurniaKiosk.Helpers.Navigation;
namespace TurniaKiosk.ViewModels.Pages
{
    public partial class AppStartupViewModel : ObservableObject
    {
        private readonly QueueClientService _queueClientService;

        public AppStartupViewModel(QueueClientService queueClientService)
        {
            _queueClientService = queueClientService;

            StartPageCommand = new Command(
            execute: () =>
            {
                StartPage();
            });
        }


        public ICommand StartPageCommand { get; } = null!;


        private async void StartPage()
        {
            await KioskNavigationHelper.GoToStartAsync(_queueClientService);
        }
    }
}
