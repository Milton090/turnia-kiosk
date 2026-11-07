using TurniaKiosk.ViewModels.Pages;

namespace TurniaKiosk.Pages;

public partial class TicketCreation : ContentPage
{
	public TicketCreation(TicketCreationViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}