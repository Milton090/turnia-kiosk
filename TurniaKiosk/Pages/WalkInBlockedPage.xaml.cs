using TurniaKiosk.ViewModels.Pages;

namespace TurniaKiosk.Pages
{
    public partial class WalkInBlockedPage : ContentPage
    {
        public WalkInBlockedPage(WalkInBlockedViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
