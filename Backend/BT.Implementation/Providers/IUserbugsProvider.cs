using BT.Models;

namespace BT.Implementation.Providers;

public interface IUserBugsProvider
{
    public List<Bug> GetUserBugs(string referenceId);
    public Bug CreateBug(Bug bug);
}