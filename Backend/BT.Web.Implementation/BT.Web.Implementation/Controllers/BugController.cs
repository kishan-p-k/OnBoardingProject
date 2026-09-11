using BT.Models;
using BT.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers
{
    [ApiController]
    [Route("bugs")]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;
        private readonly ILogger<BugController> _logger;

        public BugController(
            IBugService bugService,
            ILogger<BugController> logger)
        {
            _bugService = bugService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllBugs()
        {
            _logger.LogInformation(
                "GET request received for all bugs.");

            try
            {
                List<Bug> bugs = _bugService.GetAllBugs();

                _logger.LogInformation(
                    "Returning {BugCount} bugs to the client.",
                    bugs.Count);

                return Ok(bugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while processing GET /api/bugs.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.");
            }
        }
    }
}