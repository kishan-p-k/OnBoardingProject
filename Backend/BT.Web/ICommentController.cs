using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web;

public interface ICommentController
{
    List<Comment> GetCommentByBug();
}