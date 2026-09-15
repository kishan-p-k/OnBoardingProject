using BT.Models;
using BT;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("userbugs")]
public class UserBugsController : ControllerBase
{
    private readonly IUserBugsService _userBugsService;
    private readonly ILogger<UserBugsController> _logger;
    public UserBugsController(
        IUserBugsService userBugsService,
        ILogger<UserBugsController> logger)
    {
        _userBugsService = userBugsService;
        _logger = logger;
    }
    [HttpGet("{ReferenceId}")]
    public List<Bug> GetUserBugs(string ReferenceId)
    {
        _logger.LogInformation(
            "GET request received for bugs reported by user with ID {ReferenceId}.", ReferenceId);
        try
        {
            List<Bug> userBugs = _userBugsService.GetUserBugs(ReferenceId);
            _logger.LogInformation(
                "Returning {BugCount} bugs reported by user with ID {ReferenceId} to the client.",
                userBugs.Count, ReferenceId);
            return userBugs;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET userbugs/{ReferenceId}.", ReferenceId);
            return new List<Bug>();
        }
    }
}