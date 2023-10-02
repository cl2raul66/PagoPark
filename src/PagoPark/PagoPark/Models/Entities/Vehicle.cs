using LiteDB;
using PagoPark.Tools;

namespace PagoPark.Models;

public class Vehicle
{
    [BsonId]
    public string LicensePlate { get; set; }
    public TypeVehicleFunction TypeFunction { get; set; }
    public TypeLoadCapacity TypeCapacity { get; set; }
}