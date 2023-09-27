namespace PagoPark.Models;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public User() { }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public override string ToString()
    {
        return Username;
    }
}
