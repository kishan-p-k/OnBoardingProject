using BT.Models;

namespace BT;

public interface IAuthService
{
    public Users? GetUserForLogin(string username, string password);
}