using PagoPark.Views;

namespace PagoPark
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PgHome), typeof(PgHome));
        }
    }
}