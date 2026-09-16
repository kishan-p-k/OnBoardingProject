using BT.Models;
using BT;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentController> _logger;

    public CommentController(
        ICommentService commentService,
        ILogger<CommentController> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    [HttpGet("{ref_id}")]
    public List<Comment> GetCommentByBug(string ref_id)
    {
        _logger.LogInformation(
            "GET request received for all comments.");

        try
        {
            List<Comment> comments = _commentService.GetCommentByBug(ref_id);

            _logger.LogInformation(
                "Returning {CommentCount} comments to the client.",
                comments.Count);

            return comments;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET /api/comments/refid.");

            return new List<Comment>();
        }
    }
}