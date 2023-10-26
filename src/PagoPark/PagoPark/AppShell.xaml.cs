using PagoPark.Views;

namespace PagoPark;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(PgSingIn), typeof(PgSingIn));
        Routing.RegisterRoute(nameof(PgManageUser), typeof(PgManageUser));
        Routing.RegisterRoute(nameof(PgManageContracts), typeof(PgManageContracts));
        Routing.RegisterRoute(nameof(PgAddContract), typeof(PgAddContract));
        Routing.RegisterRoute(nameof(PgAddPay), typeof(PgAddPay));
        Routing.RegisterRoute(nameof(PgAddPayForAllWeek), typeof(PgAddPayForAllWeek));
    }
}