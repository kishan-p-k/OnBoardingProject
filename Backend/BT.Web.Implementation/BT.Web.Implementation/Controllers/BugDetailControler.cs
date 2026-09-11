using BT.Models;
using BT.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers
{
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
	}
}