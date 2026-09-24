using BT.Models;

namespace BT;

public interface IAuthService
{
    public UserRequestModel? GetUserForLogin(string username, string password);

    public CreateUserResult? CreateUser(string username, string mail, string password, string role);
}