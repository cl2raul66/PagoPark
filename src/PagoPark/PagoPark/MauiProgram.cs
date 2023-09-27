using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using PagoPark.Services;
using PagoPark.ViewModels;
using PagoPark.Views;

namespace PagoPark
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IAuthService, AuthService>();

            builder.Services.AddTransient<PgSingInViewModel>();

            builder.Services.AddSingleton<PgSingIn>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}