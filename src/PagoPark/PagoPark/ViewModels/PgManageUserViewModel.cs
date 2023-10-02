using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PagoPark.Services;
using System.ComponentModel.DataAnnotations;

namespace PagoPark.ViewModels;

public partial class PgManageUserViewModel : ObservableValidator
{
    readonly IAuthService authServ;

    public PgManageUserViewModel(IAuthService authService)
    {
        authServ = authService;
        Username = authServ.currentUser.Username.ToUpper();
    }

    [ObservableProperty]
    string username;

    [ObservableProperty]
    [Required]
    [MinLength(3)]
    string oldPassword;

    [ObservableProperty]
    [Required]
    [MinLength(3)]
    string newPassword;

    [ObservableProperty]
    bool visibleErrorInfo;

    [ObservableProperty]
    string textErrorInfo;

    [RelayCommand]
    async Task PasswordChange()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            TextErrorInfo = "Fill in the required fields (*)";
            VisibleErrorInfo = true;
            await Task.Delay(5000);
            VisibleErrorInfo = false;
            return;
        }

        if (!authServ.ExistPassword(OldPassword.Trim()))
        {
            TextErrorInfo = "Old password invalid!";
            VisibleErrorInfo = true;
            await Task.Delay(5000);
            VisibleErrorInfo = false;
            return;
        }

        authServ.PasswordChange(NewPassword.Trim());
        await Shell.Current.GoToAsync("..", true);
    }
}
