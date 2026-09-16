using BT.Models;

namespace BT;

public interface IBugDetailService
{
    public Bug? GetBugById(string ref_id);
    public bool DeleteBug(string ref_id);
    public Bug UpdateBugField(string ref_id, string bug_field, string bugvalue);
}