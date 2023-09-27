using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgSingIn : ContentPage
{
	public PgSingIn(PgSingInViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}