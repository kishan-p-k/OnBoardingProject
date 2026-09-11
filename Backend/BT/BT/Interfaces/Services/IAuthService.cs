using BT.Models;

namespace BT.Interfaces.Services
{
    public interface IAuthService
    {
        public Users? GetUserForLogin(string username, string password);
    }
}