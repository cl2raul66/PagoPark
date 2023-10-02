using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgManageVehicles : ContentPage
{
	public PgManageVehicles(PgManageVehiclesViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}