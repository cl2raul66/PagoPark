namespace PagoPark.Views;

public partial class PgSettings : ContentPage
{
	public PgSettings()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		switch ((sender as Button).Text)
		{
			case "Manage users":
				await Shell.Current.GoToAsync(nameof(PgManageUser), true);
				break;
			case "Manage contracts":
                await Shell.Current.GoToAsync(nameof(PgManageContracts), true);
                break;
            default:
				break;
		}
	}
}