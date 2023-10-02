using PagoPark.Views;

namespace PagoPark
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PgHome), typeof(PgHome));
            Routing.RegisterRoute(nameof(PgManageUser), typeof(PgManageUser));
            Routing.RegisterRoute(nameof(PgManageVehicles), typeof(PgManageVehicles));
            Routing.RegisterRoute(nameof(PgAddVehicle), typeof(PgAddVehicle));
            Routing.RegisterRoute(nameof(PgDetailPay), typeof(PgDetailPay));
            Routing.RegisterRoute(nameof(PgPay), typeof(PgPay));
        }
    }
}