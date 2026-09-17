using BT;
using BT.Models;
using Microsoft.AspNetCore.Mvc;
using BT.Web;
namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("bugs")]
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
    public List<Bug> FilterBugs([FromQuery] BugFilter filter)
    {
        _logger.LogInformation(
            "GET request received for filtering bugs.");
        try
        {
            List<Bug> filteredBugs = _bugService.FilterBugs(filter);
            _logger.LogInformation(
                "Successfully filtered bugs. Count: {Count}.",
                filteredBugs.Count);

            return filteredBugs;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET /bugfilter/filter.");
            return new List<Bug>();
        }
    }
}