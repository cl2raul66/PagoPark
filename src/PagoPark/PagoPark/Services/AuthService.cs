using LiteDB;
using PagoPark.Models;

namespace PagoPark.Services;

public class AuthService
{
    readonly ILiteCollection<User> collection;
    CurrentUser currentUser;

    public AuthService()
    {
        var cnx = new ConnectionString()
        {
            Filename = Path.Combine(FileSystem.Current.AppDataDirectory, "Users.db")
        };

        LiteDatabase db = new(cnx);
        collection = db.GetCollection<User>();
    }

    public bool Register(string username, string password)
    {
        var newUser = new User(username,password);
        currentUser = new(newUser.Id, newUser.Username);
        
        return collection.Insert(newUser) is not null;
    }

    public bool Login(string username, string password)
    {
        if (Preferences.ContainsKey(nameof(currentUser)))
        {
            string id = Preferences.Get(nameof(currentUser), string.Empty);
            var u = collection.FindById(id);
            currentUser = new(u.Id, u.Username);
        }

        return currentUser is not null;
    }
}
