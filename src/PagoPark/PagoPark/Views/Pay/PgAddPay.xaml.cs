using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgAddPay : ContentPage
{
	public PgAddPay(PgAddPayViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}