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
            builder.Services.AddSingleton<ILiteDbDailyPaymentLogService, LiteDbDailyPaymentLogService>();

            builder.Services.AddSingleton<PgHomeViewModel>();
            builder.Services.AddSingleton<PgPayViewModel>();
            builder.Services.AddTransient<PgAddPayViewModel>();
            builder.Services.AddTransient<PgAddPayForAllWeekViwModel>();
            builder.Services.AddTransient<PgSingInViewModel>();
            builder.Services.AddTransient<PgManageUserViewModel>();
            builder.Services.AddTransient<PgManageContractsViewModel>();
            builder.Services.AddTransient<PgAddContractViewModel>();
            builder.Services.AddTransient<PgReportsViewModel>();

            builder.Services.AddSingleton<PgHome>();
            builder.Services.AddSingleton<PgPay>();
            builder.Services.AddTransient<PgAddPay>();
            builder.Services.AddTransient<PgAddPayForAllWeek>();
            builder.Services.AddTransient<PgSingIn>();
            builder.Services.AddTransient<PgManageUser>();
            builder.Services.AddTransient<PgManageContracts>();
            builder.Services.AddTransient<PgAddContract>();
            builder.Services.AddTransient<PgReports>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}