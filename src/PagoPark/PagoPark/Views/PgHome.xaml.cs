using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgHome : ContentPage
{

    public PgHome(PgHomeViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override bool OnBackButtonPressed() { return true; }
}