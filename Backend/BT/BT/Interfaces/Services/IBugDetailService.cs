using BT.Models;

namespace BT.Interfaces.Services
{
    public interface IBugDetailService
    {
        public Bug? GetBugById(string ref_id);
    }
}