using PagoPark.ViewModels;

namespace PagoPark.Views;

public partial class PgPay : ContentPage
{
    public PgPay(PgPayViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}