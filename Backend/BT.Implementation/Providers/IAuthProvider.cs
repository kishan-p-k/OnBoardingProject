using BT.Models;

namespace BT.Implementation.Providers;

public interface IAuthProvider
{
    public Users? GetUserForLogin(string Mail);
}