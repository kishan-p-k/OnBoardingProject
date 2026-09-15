using BT.Models;

namespace BT.Implementation.Providers.Interfaces
{
    public interface IUserBugsProvider
    {
        public List<Bug> GetUserBugs(string referenceId);
    }
}