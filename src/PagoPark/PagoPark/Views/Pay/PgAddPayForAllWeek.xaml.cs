using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgAddPayForAllWeek : ContentPage
{
	public PgAddPayForAllWeek(PgAddPayForAllWeekViwModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}