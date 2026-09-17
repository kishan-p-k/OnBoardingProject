using BT.Models;

namespace BT;

public interface IBugService
{
    public List<Bug> GetAllBugs();
    public List<Bug> FilterBugs(BugFilter filter);
}