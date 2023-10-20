using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgHome : ContentPage
{
    public PgHome(PgHomeViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(Shell.Current.CurrentItem.CurrentItem, true);
    }
}