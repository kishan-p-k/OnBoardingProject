using BT.Models;

namespace BT.Implementation.Providers.Interfaces
{
    public interface IBugProvider
    {
        public List<Bug> GetAllBugs();
    }
}