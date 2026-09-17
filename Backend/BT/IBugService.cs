using BT.Models;

namespace BT;

public interface IBugService
{
    public List<Bug> GetAllBugs();
    //public Bug CreateBug(Bug newBug);
}