using BT.Models;
using BT;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("comment")]
public class CommentController : ControllerBase, ICommentController
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

    [HttpPut("{reference_id}")]
    public Comment UpdateComment(
    [FromRoute] string reference_id,
    [FromBody] Comment comment)
    {
        _logger.LogInformation(
            "Starting comment update for Reference ID: {ReferenceId}",
            reference_id);

        try
        {
            _logger.LogDebug(
                "Received comment update request for Reference ID: {ReferenceId}",
                reference_id);
            Console.WriteLine($"Received comment update request for Reference ID: {reference_id}");

            Comment updatedComment = _commentService.UpdateComment(
                reference_id,
                comment.comment);

            _logger.LogInformation(
                "Successfully updated comment with Reference ID: {ReferenceId}",
                reference_id);

            Console.WriteLine($"Successfully updated comment with Reference ID: {reference_id}");

            return updatedComment;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating comment with Reference ID: {ReferenceId}",
                reference_id);

            throw;
        }
    }
}