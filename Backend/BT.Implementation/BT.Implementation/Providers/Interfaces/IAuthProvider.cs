using BT.Models;

namespace BT.Implementation.Providers.Interfaces
{
    public interface IAuthProvider
    {
        public Users? GetUserForLogin(string usernameOrMail);
    }
}