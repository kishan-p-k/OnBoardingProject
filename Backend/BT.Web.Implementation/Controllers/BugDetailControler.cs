using BT.Models;
using BT;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("bug")]
public class BugDetailsController : ControllerBase
{
    private readonly IBugDetailService _bugDetailService;
    private readonly ILogger<BugDetailsController> _logger;

    public BugDetailsController(
        IBugDetailService bugDetailService,
        ILogger<BugDetailsController> logger)
    {
        _bugDetailService = bugDetailService;
        _logger = logger;
    }

    [HttpGet("{ref_id}")]
    public Bug? GetBugById(string ref_id)
    {
        _logger.LogInformation(
            "GET request received for bug with ID {ref_id}.", ref_id);

        try
        {
            Bug? bug = _bugDetailService.GetBugById(ref_id);

            _logger.LogInformation(
                "Returning bug with ID {ref_id} to the client.", ref_id);

            return bug;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET bug/{ref_id}.", ref_id);

            return null;
        }
    }


    [HttpDelete("{ref_id}")]
    public IActionResult DeleteBug(string ref_id)
    {
        _logger.LogInformation(
            "DELETE request received for bug with ID {ref_id}.",
            ref_id);

        try
        {
            bool deleted = _bugDetailService.DeleteBug(ref_id);

            if (!deleted)
            {
                _logger.LogWarning(
                    "Bug with ID {ref_id} was not found.",
                    ref_id);

                return NotFound(new
                {
                    message = "Bug not found."
                });
            }

            _logger.LogInformation(
                "Successfully deleted bug with ID {ref_id}.",
                ref_id);

            return Ok(new
            {
                message = "Bug deleted successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing DELETE bug/{ref_id}.",
                ref_id);

            return StatusCode(500, new
            {
                message = "An error occurred while deleting the bug."
            });
        }
    }


    [HttpPut("{ref_id}")]
    public Bug? UpdateBugField(string ref_id, [FromBody] UpdateBugField update)
    {
        _logger.LogInformation(
            "PUT request received for bug with ID {ref_id}.", ref_id);

        try
        {
            Bug? bug = _bugDetailService.UpdateBugField(
                ref_id,
                update.BugField,
                update.BugValue);

            _logger.LogInformation(
                "Returning updated bug with ID {ref_id} to the client.", ref_id);

            return bug;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing PUT bug/{ref_id}.", ref_id);

            return null;
        }
    }
}