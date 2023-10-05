using LiteDB;
using PagoPark.Models;

namespace PagoPark.Services;

public interface ILiteDbCarParkRecordService
{
    bool Any();
    bool Delete(string id);
    bool Exist(string id);
    IEnumerable<CarParkRecordDemo> GetAll();
    IEnumerable<CarParkRecordDemo> GetThisWeek();
    bool Insert(CarParkRecordDemo entity);
}

public class LiteDbCarParkRecordService : ILiteDbCarParkRecordService
{
    readonly ILiteCollection<CarParkRecordDemo> collection;

    public LiteDbCarParkRecordService()
    {
        var cnx = new ConnectionString()
        {
            Filename = Path.Combine(FileSystem.Current.AppDataDirectory, "CarParkRecordDemo.db")
        };

        LiteDatabase db = new(cnx);
        collection = db.GetCollection<CarParkRecordDemo>();
    }

    public bool Any() => collection.Count() > 0;

    public IEnumerable<CarParkRecordDemo> GetAll() => collection.FindAll();

    public IEnumerable<CarParkRecordDemo> GetThisWeek()
    {
        DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(7);
        return collection.Find(x => x.RegistrationDate >= startOfWeek && x.RegistrationDate < endOfWeek);
    }

    public bool Insert(CarParkRecordDemo entity) => collection.Insert(entity) is not null;

    public bool Delete(string id) => collection.Delete(id);

    public bool Exist(string id) => collection.FindById(id) is not null;
}
