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

        var username = User.FindFirst("Username")?.Value;
        var referenceId = User.FindFirst("Reference_id")?.Value;
        var mail = User.FindFirst("Mail")?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        _logger.LogInformation(
            "User details - Username: {username}, Reference_id: {referenceId}, Mail: {mail}, Role: {role}",
            username, referenceId, mail, role);

        try
        {
            List<Bug> filteredBugs = _bugService.FilterBugs(filter, role);

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