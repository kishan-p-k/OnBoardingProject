using BT.Models;

namespace BT.Implementation.Providers;

public interface IAuthProvider
{
    public Users? GetUserForLogin(string Mail);

    public CreateUserResult CreateUser(string username, string mail, string password,string role);
}