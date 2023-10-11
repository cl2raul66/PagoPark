using LiteDB;
using PagoPark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagoPark.Services;

public class LiteDbDailyPaymentLogService
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
        return collection.Find(x => x.PaymentDate >= startOfWeek && x.PaymentDate < endOfWeek);
    }

    public IEnumerable<DailyPaymentLog> GetByWeek(int year, int numberweek) {
        var weekBeginEnd = dateServ.GetWeekDates(year, numberweek);
        return collection.Find(x => x.PaymentDate >= weekBeginEnd.Item1 && x.PaymentDate < weekBeginEnd.Item2);
    }

    public bool Insert(DailyPaymentLog entity) => collection.Insert(entity) is not null;
    public bool Insert(IEnumerable<DailyPaymentLog> entities) => collection.InsertBulk(entities) > 0;

    public bool Delete(string id) => collection.Delete(id);

    public bool Exist(string id) => collection.FindById(id) is not null;
}
