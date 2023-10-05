using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgManageContracts : ContentPage
{
	public PgManageContracts(PgManageContractsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}