using BT.Models;

namespace BT.Interfaces.Services
{
    public interface IUserBugsService
    {
        public List<Bug> GetUserBugs(string Reference_id);
    }
}