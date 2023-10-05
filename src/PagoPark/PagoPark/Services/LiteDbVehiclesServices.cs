using LiteDB;
using PagoPark.Models;

namespace PagoPark.Services;

public interface ILiteDbVehiclesServices
{
    bool Any();
    IEnumerable<VehicleDemo> GetAll();
    VehicleDemo GetById(string id);
    bool Delete(string id);
    bool Exist(string id);
    bool Insert(VehicleDemo entity);
}

public class LiteDbVehiclesServices : ILiteDbVehiclesServices
{
    readonly ILiteCollection<VehicleDemo> collection;

    public LiteDbVehiclesServices()
    {
        var cnx = new ConnectionString()
        {
            Filename = Path.Combine(FileSystem.Current.AppDataDirectory, "VehiclesDemo.db")
        };

        LiteDatabase db = new(cnx);
        collection = db.GetCollection<VehicleDemo>();
    }

    public bool Any() => collection.Count() > 0;

    public IEnumerable<VehicleDemo> GetAll() => collection.FindAll();

    public VehicleDemo GetById(string id) => collection.FindById(id);

    public bool Insert(VehicleDemo entity) => collection.Insert(entity) is not null;

    public bool Delete(string id) => collection.Delete(id);

    public bool Exist(string id) => collection.FindById(id) is not null;
}
