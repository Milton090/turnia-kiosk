using CommunityToolkit.Mvvm.ComponentModel;

namespace TurniaKiosk.ViewModels.Pages
{
    public class LogoViewModel : ObservableObject
    {
        public ImageSource LogoSource { get; }

        public LogoViewModel()
        {
            LogoSource = ImageSource.FromFile("turnia_logo.png");
        }
    }
}