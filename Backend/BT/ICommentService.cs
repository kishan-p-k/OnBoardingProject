using BT.Models;

namespace BT;

public interface ICommentService
{
    public List<Comment> GetCommentByBug(string ref_id);
    public Comment UpdateComment(string reference_id, string updatedComment);
    //public Comment? CreateComment(string ref_id, string comment_text,string user_reference);
    //public Comment EditComment(string ref_id, string comment_id, string comment_text,string user_)
    //public bool DeleteBug(string ref_id);
    //public Bug UpdateBugField(string ref_id, string bug_field, string bugvalue);
}