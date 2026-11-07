using Microsoft.Extensions.Logging;
using Services.Queue;
using Services.Constants;
using TurniaKiosk.ViewModels.Pages;
using TurniaKiosk.Pages;
using CommunityToolkit.Maui;

namespace TurniaKiosk
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                    fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                    fonts.AddFont("Inter-Bold.ttf", "InterBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient("HttpClient", config =>
            {
                config.BaseAddress = new Uri(ConstantDefinitions.BaseUrl);
                config.Timeout = new TimeSpan(0, 0, 100);
            });

            builder.RegisterAppShell();
            builder.RegisterAppPages();
            builder.RegisterAppViewModels();
            builder.RegisterAppServices();
            builder.RegisterAppRoutes();


            return builder.Build();
        }

        public static MauiAppBuilder RegisterAppShell(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<AppShell>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterAppPages(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<AppStartup>();
            mauiAppBuilder.Services.AddTransient<ServiceSelection>();
            mauiAppBuilder.Services.AddTransient<TicketCreation>();
            mauiAppBuilder.Services.AddTransient<WalkInBlockedPage>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterAppViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<AppStartupViewModel>();
            mauiAppBuilder.Services.AddTransient<ServiceSelectionViewModel>();
            mauiAppBuilder.Services.AddTransient<TicketCreationViewModel>();
            mauiAppBuilder.Services.AddTransient<WalkInBlockedViewModel>();

            #region ViewModels for Components
            mauiAppBuilder.Services.AddSingleton<LogoViewModel>();
            #endregion

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<QueueClientService>();
            mauiAppBuilder.Services.AddSingleton<Services.Bixolon.BixolonService>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterAppRoutes(this MauiAppBuilder mauiAppBuilder)
        {
            Routing.RegisterRoute(nameof(AppStartup), typeof(AppStartup));
            Routing.RegisterRoute(nameof(ServiceSelection), typeof(ServiceSelection));
            Routing.RegisterRoute(nameof(TicketCreation), typeof(TicketCreation));
            Routing.RegisterRoute(nameof(WalkInBlockedPage), typeof(WalkInBlockedPage));


            return mauiAppBuilder;
        }
    }
}
