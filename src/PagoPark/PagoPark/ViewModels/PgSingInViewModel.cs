using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PagoPark.Services;
using PagoPark.Views;

namespace PagoPark.ViewModels;

public partial class PgSingInViewModel : ObservableValidator
{
    readonly IAuthService authServ;

    public PgSingInViewModel(IAuthService authService)
    {
        authServ = authService;

        _ = GoToHome();
    }

    [ObservableProperty]
    string username;

    [ObservableProperty]
    string password;

    [RelayCommand]
    async Task LogIn()
    {
        bool resul = authServ.Login(username, password);
        if (resul)
        {
            await GoToHome();
        }
    }

    [RelayCommand]
    async Task SingIn()
    {
        bool resul = authServ.Register(username, password);
        if (resul)
        {
            await GoToHome();
        }
    }

    #region Extra
    async Task GoToHome()
    {
        if (authServ.LoadCurrentUser())
        {
            await Shell.Current.GoToAsync(nameof(PgHome), true);
        }
    }
    #endregion
}
