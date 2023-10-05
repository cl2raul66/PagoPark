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
        Routing.RegisterRoute(nameof(PgAddVehicle), typeof(PgAddVehicle));
        Routing.RegisterRoute(nameof(PgManageCarPark), typeof(PgManageCarPark));
        Routing.RegisterRoute(nameof(PgAddPay), typeof(PgAddPay));
    }
}