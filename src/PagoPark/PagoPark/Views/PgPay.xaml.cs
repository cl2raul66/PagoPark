using PagoPark.Models;
using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgPay : ContentPage
{
    public PgPay(PgPayViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is not null && e.Item is PayContract)
        {
            (BindingContext as PgPayViewModel).SelectedContract = e.Item as PayContract;
        }
    }
}