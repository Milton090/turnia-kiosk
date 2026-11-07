using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;

namespace TurniaKiosk.WinUI
{
    public partial class App : MauiWinUIApplication
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var instance = AppInstance.FindOrRegisterForKey("TurniaInstance");
            if (!instance.IsCurrent)
            {
                var currentInstance = AppInstance.GetCurrent();
                instance.RedirectActivationToAsync(currentInstance.GetActivatedEventArgs()).AsTask().Wait();
                System.Environment.Exit(0);
                return;
            }

            base.OnLaunched(args);
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
