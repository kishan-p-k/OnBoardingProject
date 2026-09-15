using BT.Models;

namespace BT.Implementation.Providers;

public interface IBugProvider
{
    public List<Bug> GetAllBugs();
}