using LiteDB;
using PagoPark.Models;

namespace PagoPark.Services;

public interface ILiteDbParkContractServices
{
    bool Any();
    IEnumerable<ParkContract> GetAll();
    ParkContract GetById(string id);
    bool Delete(string id);
    bool Exist(string id);
    bool Insert(ParkContract entity);
}

public class LiteDbParkContractServices : ILiteDbParkContractServices
{
    readonly ILiteCollection<ParkContract> collection;

    public LiteDbParkContractServices()
    {
        var cnx = new ConnectionString()
        {
            Filename = Path.Combine(FileSystem.Current.AppDataDirectory, "ParkContract.db")
        };

        LiteDatabase db = new(cnx);
        collection = db.GetCollection<ParkContract>();
    }

    public bool Any() => collection.Count() > 0;

    public IEnumerable<ParkContract> GetAll() => collection.FindAll();

    public ParkContract GetById(string id) => collection.FindById(id);

    public bool Insert(ParkContract entity)
    {
        if (string.IsNullOrEmpty(entity.Id))
        {
            entity.Id = ObjectId.NewObjectId().ToString();
        }
        return collection.Insert(entity) is not null;
    }

    public bool Delete(string id) => collection.Delete(id);

    public bool Exist(string id) => collection.FindById(id) is not null;
}
