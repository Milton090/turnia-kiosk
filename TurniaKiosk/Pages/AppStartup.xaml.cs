using TurniaKiosk.ViewModels.Pages;

namespace TurniaKiosk.Pages;

public partial class AppStartup : ContentPage
{
	public AppStartup(AppStartupViewModel appStartupViewModel)
	{
		InitializeComponent();

        BindingContext = appStartupViewModel;
    }
}