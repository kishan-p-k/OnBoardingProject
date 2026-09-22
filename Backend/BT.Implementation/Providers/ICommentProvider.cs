using BT.Models;

namespace BT.Implementation.Providers;

public interface ICommentProvider
{
    public List<Comment> GetCommentByBug(string ref_id);
    public Comment UpdateComment(string reference_id, string updatedComment);
    public Comment CreateComment(string reference_id, string comment, string author);
}