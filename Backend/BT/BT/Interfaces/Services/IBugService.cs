using BT.Models;

namespace BT.Interfaces.Services
{
    public interface IBugService
    {
        public List<Bug> GetAllBugs();
    }
}