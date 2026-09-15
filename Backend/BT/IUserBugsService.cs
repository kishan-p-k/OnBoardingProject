using BT.Models;

namespace BT;

public interface IUserBugsService
{
    public List<Bug> GetUserBugs(string Reference_id);
}