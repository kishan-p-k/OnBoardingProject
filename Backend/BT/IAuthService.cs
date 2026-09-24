using BT.Models;

namespace BT;

public interface IAuthService
{
    public LoginResponseModel? GetUserForLogin(string username, string password);

    public Users? CreateUser(string username, string mail, string password);
}