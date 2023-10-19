﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PagoPark.Services;
using PagoPark.Views;

namespace PagoPark.ViewModels;

public partial class PgHomeViewModel : ObservableRecipient
{
    readonly IAuthService authServ;
    readonly ILiteDbParkContractServices parkContractServ;

    public PgHomeViewModel(IAuthService authService, ILiteDbParkContractServices parkContractServices)
    {
        IsActive = true;
        authServ = authService;
        parkContractServ = parkContractServices;

        HasParkContracts = parkContractServ.Any();
        _ = GoToSingin();
    }

    [ObservableProperty]
    bool hasParkContracts;

    #region Extra
    protected override void OnActivated()
    {
        base.OnActivated();
        WeakReferenceMessenger.Default.Register<PgHomeViewModel, string, string>(this, nameof(HasParkContracts),(r, m) =>
        {
            _ = bool.TryParse(m, out bool resul);
            r.HasParkContracts = resul;
        });
    }

    async Task GoToSingin()
    {
        if (!authServ.LoadCurrentUser())
        {
            await Shell.Current.GoToAsync(nameof(PgSingIn), true);
        }
    }
    #endregion
}
