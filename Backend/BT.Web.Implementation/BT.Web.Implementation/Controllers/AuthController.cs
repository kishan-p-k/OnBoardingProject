using BT.Models;
using BT.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using BT.Web.Interfaces.Controllers;
namespace BT.Web.Implementation.Controllers
{
    [ApiController]
    [Route("api/userauth")]
    public class AuthController : ControllerBase,IAuthController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet("{Mail}/{password}")]
        public IActionResult Login(string Mail, string password)
        {
            _logger.LogInformation(
                "GET request received for user login.");

            try
            {
                Users? user = _authService.GetUserForLogin(Mail, password);

                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                // Return user without password for security
                var userResponse = new
                {
                    user.Reference_id,
                    user.Username,
                    user.Mail
                };

                _logger.LogInformation(
                    "Successfully authenticated user {Reference_id}.",
                    user.Reference_id);

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while processing GET /userauth/{Mail}.",
                    Mail);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.");
            }
        }
    }
}


