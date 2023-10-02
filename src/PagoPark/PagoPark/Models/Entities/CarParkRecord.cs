namespace PagoPark.Models;

public class CarParkRecord
{
    public DateTime FechaRegistro { get; set; }
    public DateTime FechaEntrada { get; set; }
    public DateTime FechaSalida { get; set; }
    public string MatriculaVehiculo { get; set; }    
    public string LocalParqueo { get; set; }    
    public string Observaciones { get; set; }    
}
