using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgManageCarPark : ContentPage
{
	public PgManageCarPark(PgManageCarParkViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}