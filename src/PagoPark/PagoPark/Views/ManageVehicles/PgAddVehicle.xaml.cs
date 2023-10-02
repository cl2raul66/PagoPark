using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgAddVehicle : ContentPage
{
	public PgAddVehicle(PgAddVehicleViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}