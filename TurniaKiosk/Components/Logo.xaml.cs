using TurniaKiosk.ViewModels.Pages;

namespace TurniaKiosk.Components;

public partial class Logo : ContentView
{
    public Logo()
	{
		InitializeComponent();

        BindingContext = Application.Current.Handler.MauiContext.Services.GetService<LogoViewModel>();
    }
}