using BT.Models;
using BT;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;

namespace BT.Implementation.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentProvider _commentProvider;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            ICommentProvider commentProvider,
            ILogger<CommentService> logger)
        {
            _commentProvider = commentProvider;
            _logger = logger;
        }

        public List<Comment> GetCommentByBug(string ref_id)
        {
            _logger.LogInformation("Fetching all comments.");

            try
            {
                List<Comment> comments = _commentProvider.GetCommentByBug(ref_id);

                _logger.LogInformation(
                    "Successfully fetched {CommentCount} comments.",
                    comments.Count);

                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to fetch all comments.");

                throw;
            }
        }
        
        public Comment UpdateComment(string reference_id, string updatedComment)
        {
            _logger.LogInformation("Updating comment with reference ID: {ReferenceId}", reference_id);

            try
            {
                Comment updated = _commentProvider.UpdateComment(reference_id, updatedComment);

                _logger.LogInformation("Successfully updated comment with reference ID: {ReferenceId}", reference_id);

                return updated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update comment with reference ID: {ReferenceId}", reference_id);
                throw;
            }
        }
    }
}