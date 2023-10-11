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
            builder.Services.AddSingleton<IDateService, DateService>();
            builder.Services.AddSingleton<ILiteDbParkContractServices, LiteDbParkContractServices>();
            builder.Services.AddSingleton<ILiteDbCarParkRecordService, LiteDbCarParkRecordService>();

            builder.Services.AddTransient<PgHomeViewModel>();
            builder.Services.AddTransient<PgPayViewModel>();
            builder.Services.AddSingleton<PgAddPayViewModel>();
            builder.Services.AddTransient<PgSingInViewModel>();
            builder.Services.AddSingleton<PgManageUserViewModel>();
            builder.Services.AddSingleton<PgManageContractsViewModel>();
            builder.Services.AddSingleton<PgAddContractViewModel>();
            //builder.Services.AddSingleton<PgManageCarParkViewModel>();

            builder.Services.AddTransient<PgHome>();
            builder.Services.AddTransient<PgPay>();
            builder.Services.AddSingleton<PgAddPay>();
            builder.Services.AddTransient<PgSingIn>();
            builder.Services.AddSingleton<PgManageUser>();
            builder.Services.AddSingleton<PgManageContracts>();
            builder.Services.AddSingleton<PgAddContract>();
            //builder.Services.AddSingleton<PgManageCarPark>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}