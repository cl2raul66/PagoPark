using LiteDB;
using PagoPark.Models;

namespace PagoPark.Services;

public interface ILiteDbDailyPaymentLogService
{
    bool Any();
    bool Delete(string id);
    bool Exist(string id);
    IEnumerable<DailyPaymentLog> GetAll();
    IEnumerable<DailyPaymentLog> GetByWeek(int year, int numberweek);
    IEnumerable<DailyPaymentLog> GetThisWeek();
    IEnumerable<DailyPaymentLog> GetByDate(DateTime date);
    IEnumerable<DailyPaymentLog> GetByDates(DateTime start, DateTime end);
    IEnumerable<DailyPaymentLog> GetByParkContractId(string parkcontractid);
    IEnumerable<string> GetParkContractIdByDates(DateTime start, DateTime end);
    double AmountCollected(DateTime start, DateTime end);
    bool Upsert(DailyPaymentLog m);
}

public class LiteDbDailyPaymentLogService : ILiteDbDailyPaymentLogService
{
    readonly IDateService dateServ;
    readonly ILiteCollection<DailyPaymentLog> collection;

    public LiteDbDailyPaymentLogService(IDateService dateService)
    {
        dateServ = dateService;

        var cnx = new ConnectionString()
        {
            Filename = Path.Combine(FileSystem.Current.AppDataDirectory, "DailyPaymentLogs.db")
        };

        LiteDatabase db = new(cnx);
        collection = db.GetCollection<DailyPaymentLog>();
    }

    public bool Any() => collection.Count() > 0;

    public IEnumerable<DailyPaymentLog> GetAll() => collection.FindAll();

    public IEnumerable<DailyPaymentLog> GetThisWeek()
    {
        DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(7);
        var resul = collection.FindAll().Where(x => x.PaymentDate >= startOfWeek && x.PaymentDate < endOfWeek);
        return resul;
    }

    public IEnumerable<DailyPaymentLog> GetByDate(DateTime date) => collection.FindAll().Where(x => DateTimeToCustomString(x.PaymentDate) == DateTimeToCustomString(date));

    public IEnumerable<DailyPaymentLog> GetByDates(DateTime start, DateTime end) => collection.Find(x => x.PaymentDate >= start && x.PaymentDate <= end);

    public IEnumerable<DailyPaymentLog> GetByWeek(int year, int numberweek)
    {
        var weekBeginEnd = dateServ.GetWeekDates(year, numberweek);
        return collection.Find(x => x.PaymentDate >= weekBeginEnd.Item1 && x.PaymentDate < weekBeginEnd.Item2);
    }

    public IEnumerable<DailyPaymentLog> GetByParkContractId(string parkcontractid) => collection.Find(x => x.ParkContractId == parkcontractid);

    public IEnumerable<string> GetParkContractIdByDates(DateTime start, DateTime end) => collection.Find(x => x.PaymentDate >= start && x.PaymentDate <= end && (x.Amount == null || x.Amount == 0)).Select(x => x.ParkContractId);

    public double AmountCollected(DateTime start, DateTime end)
    {
        var d = collection.Find(x => x.PaymentDate >= start && x.PaymentDate <= end && (x.Amount > 0));
        return d.Sum(x => x.Amount) ?? 0;
    }

    public bool Upsert(DailyPaymentLog m)
    {
        if (string.IsNullOrEmpty(m.Id))
        {
            m.Id = ObjectId.NewObjectId().ToString();
            return collection.Insert(m) is not null;
        }
        return collection.Update(m);
    }

    public bool Delete(string id) => collection.Delete(id);

    public bool Exist(string id) => collection.FindById(id) is not null;

    #region Extra
    string DateTimeToCustomString(DateTime datetime) => datetime.ToString("yyyyMMdd");

    DateTime CustomStringToDateTime(string customstring) => new(int.Parse(customstring[..4]), int.Parse(customstring[4..5]), int.Parse(customstring[6..]));
    #endregion
}
