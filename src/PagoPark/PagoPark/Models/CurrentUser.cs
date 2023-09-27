namespace PagoPark.Models;

public class CurrentUser
{
    public string Id { get; set; }
    public string Username { get; set; }

    public CurrentUser(string id, string username)
    {
        Id = id;
        Username = username;
    }

    public override string ToString()
    {
        return Username;
    }
}
