using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgReports : ContentPage
{
	public PgReports(PgReportsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}