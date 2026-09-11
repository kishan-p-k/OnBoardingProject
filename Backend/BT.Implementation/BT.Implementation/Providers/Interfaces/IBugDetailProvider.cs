using BT.Models;

namespace BT.Implementation.Providers.Interfaces
{
    public interface IBugDetailProvider
    {
        public Bug? GetBugById(string ref_id);
    }
}