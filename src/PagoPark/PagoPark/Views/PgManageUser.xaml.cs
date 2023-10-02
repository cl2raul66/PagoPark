using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgManageUser : ContentPage
{
	public PgManageUser(PgManageUserViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}