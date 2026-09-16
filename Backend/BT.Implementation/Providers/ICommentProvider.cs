using BT.Models;

namespace BT.Implementation.Providers;

public interface ICommentProvider
{
    public List<Comment> GetCommentByBug(string ref_id);
}