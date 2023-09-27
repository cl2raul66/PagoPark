using LiteDB;
using PagoPark.Models;

namespace PagoPark.Services;

public interface IAuthService
{
    bool LoadCurrentUser();
    bool Login(string username, string password);
    bool PasswordChange(string newPassword);
    bool Register(string username, string password);
}

public class AuthService : IAuthService
{
    readonly ILiteCollection<User> collection;
    public CurrentUser currentUser;

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
        var newUser = new User(username, password)
        {
            Id = ObjectId.NewObjectId().ToString()
        };
        bool resul = collection.Insert(newUser) is not null;
        if (resul)
        {
            Preferences.Set(nameof(currentUser), newUser.Id);
        }
        return resul;
    }

    public bool LoadCurrentUser()
    {
        if (Preferences.ContainsKey(nameof(currentUser)))
        {
            string id = Preferences.Get(nameof(currentUser), string.Empty);
            var u = collection.FindById(id);
            currentUser = new(u.Id, u.Username);
        }

        return currentUser is not null;
    }

    public bool Login(string username, string password)
    {
        User u = collection.FindOne(x => x.Username == username && x.Password == password);

        if (u is not null)
        {
            Preferences.Set(nameof(currentUser), u.Id);
            return true;
        }

        return false;
    }

    public bool PasswordChange(string newPassword)
    {
        string id = Preferences.Get(nameof(currentUser), string.Empty);
        var u = collection.FindById(id);
        u.Password = newPassword;
        return collection.Update(u);
    }
}
