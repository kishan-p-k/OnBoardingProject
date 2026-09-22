using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web;

public interface ICommentController
{
    List<Comment> GetCommentByBug(string ref_id);
    Comment UpdateComment(string reference_id, Comment comment);
    public Comment CreateComment(string ref_id,Comment comment);

}