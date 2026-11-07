namespace TurniaKiosk.Pages;

public partial class InitialLoadFromAppShell : ContentPage
{
    private static bool _hasBooted;

    public InitialLoadFromAppShell()
    {
        InitializeComponent();

        if (_hasBooted)
            return;

        _hasBooted = true;
        _ = GoToAppStartupPage();
    }

    private async Task GoToAppStartupPage()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(AppStartup)}");
        });
    }
}