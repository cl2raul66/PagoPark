using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgAddContract : ContentPage
{
	public PgAddContract(PgAddContractViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}