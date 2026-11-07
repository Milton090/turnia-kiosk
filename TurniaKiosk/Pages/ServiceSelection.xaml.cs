using TurniaKiosk.ViewModels.Pages;

namespace TurniaKiosk.Pages;

public partial class ServiceSelection : ContentPage
{
    private readonly ServiceSelectionViewModel _viewModel;

    public ServiceSelection(ServiceSelectionViewModel viewModel)
	{
        InitializeComponent();

        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.InitializeAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.StopPolling();
    }
}
