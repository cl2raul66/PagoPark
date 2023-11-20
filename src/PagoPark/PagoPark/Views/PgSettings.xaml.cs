using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PagoPark.Models;
using PagoPark.Services;

namespace PagoPark.Views;

public partial class PgSettings : ContentPage
{
    readonly ILiteDbParkContractServices parkContractServ;
    readonly ILiteDbDailyPaymentLogService dailyPaymentLogServ;

    public PgSettings(ILiteDbParkContractServices parkContractServices, ILiteDbDailyPaymentLogService dailyPaymentLogService)
    {
        InitializeComponent();

        parkContractServ = parkContractServices;
        dailyPaymentLogServ = dailyPaymentLogService;
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
                GenerateContracts();
                GenerateDailypaymentlogs();
                if (dailyPaymentLogServ.Any())
                {
                    CancellationTokenSource cancellationTokenSource = new();

                    string text = $"Generate {parkContractServ.GetAll().Count()} contract and {dailyPaymentLogServ.GetAll().LongCount()} daily payment. Please restart the application to see the changes.";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 12;

                    var toast = Toast.Make(text, duration, fontSize);

                    await toast.Show(cancellationTokenSource.Token);
                }                
                break;
        }
    }

    #region Extra
    void GenerateContracts()
    {
        if (!parkContractServ.Any())
        {
            parkContractServ.Insert(new(new("C211BKG", "Miguel", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Medium), new int[] { 6 }, 100));
            parkContractServ.Insert(new(new("C643BLW", "Miguel", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Medium), new int[] { 0, 3, 5 }, 100));
            parkContractServ.Insert(new(new("C770BWS", "Ramiro", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Heavy), new int[] { 0, 3 }, 50));
            parkContractServ.Insert(new(new("C113308", "Ramiro", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Heavy), new int[] { 1, 4 }, 50));
            parkContractServ.Insert(new(new("C166BTS", "Israel Choi", Tools.TypeVehicleFunction.Load, Tools.TypeLoadCapacity.Medium), new int[] { 1, 5 }, 125));
        }
    }

    void GenerateDailypaymentlogs()
    {
        DateTime fechaInicio = new(2023, 1, 1);
        DateTime fechaActual = DateTime.Now;

        var dias = Enumerable.Range(0, (fechaActual - fechaInicio).Days + 1)
                             .Select(offset => fechaInicio.AddDays(offset));
        Random random = new();

        foreach (var dia in dias)
        {
            int diaSemana = (int)dia.DayOfWeek;

            var contratosValidos = parkContractServ.GetByWeekNumber(diaSemana);

            foreach (var contrato in contratosValidos)
            {
                // Genera un número aleatorio para determinar un caso
                int caso = random.Next(0, 4);

                double pago = 0;
                string nota = string.Empty;

                switch (caso)
                {
                    case 0: // Caso en que la persona asistió y pagó la cantidad requerida
                        pago = contrato.PayPerFrequency;
                        break;
                    case 1: // Caso en que la persona asistió pero no pagó nada
                        pago = 0;
                        break;
                    case 2: // Caso en que la persona no asistió
                        pago = 0;
                        nota = "Not presented; ";
                        break;
                    case 3: // Caso en que la persona asistió pero pagó menos de la cantidad requerida
                        pago = random.Next(0, (int)contrato.PayPerFrequency);
                        nota = $"Debe {contrato.PayPerFrequency - pago}";
                        break;
                }

                DailyPaymentLog log = new(dia, contrato.Id, dia.AddHours(random.Next(10, 20)), pago, nota);
                dailyPaymentLogServ.Upsert(log);
            }
        }
    }
    #endregion
}