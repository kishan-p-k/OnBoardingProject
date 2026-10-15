using BT;
using BT.Models;
using Microsoft.AspNetCore.Mvc;
using BT.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("bugs")]
[Authorize]
public class BugFilterController : ControllerBase, IBugFilterController
{
    private readonly IBugService _bugService;
    private readonly ILogger<BugFilterController> _logger;
    public BugFilterController(
        IBugService bugService,
        ILogger<BugFilterController> logger)
    {
        _bugService = bugService;
        _logger = logger;
    }
    [HttpGet("filter")]
    public IActionResult FilterBugs([FromQuery] BugFilter filter)
    {
        _logger.LogInformation(
            "GET request received for filtering bugs.");

        try
        {
            List<Bug> filteredBugs = _bugService.FilterBugs(filter);

            _logger.LogInformation(
                "Successfully filtered bugs. Count: {Count}.",
                filteredBugs.Count);

            return Ok(filteredBugs);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(
                ex,
                "Unauthorized attempt to filter bugs.");

            return StatusCode(403, new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET /bugs/filter.");

            return StatusCode(500, new
            {
                message = "An unexpected error occurred."
            });
        }
    }
}