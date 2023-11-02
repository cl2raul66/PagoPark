using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PagoPark.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoPark.ViewModels;

[QueryProperty(nameof(Title), nameof(Title))]
public partial class PgReportsViewModel : ObservableObject
{
    readonly string[] options = { "By week", "By month", "All this year" };
    readonly IDateService dateServ;

    public PgReportsViewModel(IDateService dateService)
    {
        dateServ = dateService;
    }

    [ObservableProperty]
    string title;

    [ObservableProperty]
    int selectedWeek = 1;

    [ObservableProperty]
    int lastCurrentweek = 2;

    [RelayCommand]
    async Task Close() => await Shell.Current.GoToAsync("..", true);

    #region Extra
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(Title))
        {
            switch (Title)
            {
                case "By week":
                    LastCurrentweek = dateServ.GetWeekNumber(DateTime.Now);
                    if (LastCurrentweek > 1)
                    {
                        lastCurrentweek--;
                    }
                    SelectedWeek = lastCurrentweek;
                    break;
                case "By month":
                    break;
                case "All this year":
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}
