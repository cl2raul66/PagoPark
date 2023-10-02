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
                    fonts.AddFont("icofont.ttf", "icofont");
                });

            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ILiteDbVehiclesServices, LiteDbVehiclesServices>();

            builder.Services.AddTransient<PgSingInViewModel>();
            builder.Services.AddTransient<PgHomeViewModel>();
            builder.Services.AddSingleton<PgManageUserViewModel>();
            builder.Services.AddSingleton<PgManageVehiclesViewModel>();
            builder.Services.AddSingleton<PgAddVehicleViewModel>();
            //builder.Services.AddSingleton<PgDetailPayViewModel>();
            //builder.Services.AddSingleton<PgPayViewModel>();

            builder.Services.AddTransient<PgSingIn>();
            builder.Services.AddTransient<PgHome>();
            builder.Services.AddSingleton<PgManageUser>();
            builder.Services.AddSingleton<PgManageVehicles>();
            builder.Services.AddSingleton<PgAddVehicle>();
            //builder.Services.AddSingleton<PgDetailPay>();
            //builder.Services.AddSingleton<PgPay>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}