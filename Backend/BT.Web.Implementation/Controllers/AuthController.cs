using BT.Models;
using BT;
using Microsoft.AspNetCore.Mvc;
using BT.Web;
namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("userauth")]
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

    [HttpPost("login")]
    public LoginResponseModel? Login([FromBody] Users request)
    {
        _logger.LogInformation(
            "POST request received for user login.");

        try
        {
            LoginResponseModel? loginResponse = _authService.GetUserForLogin(request.Mail, request.Password);

            if (loginResponse == null)
            {
                return null;
            }

            _logger.LogInformation(
                "Successfully authenticated user {Reference_id}.",
                loginResponse.User.Reference_id);

            return loginResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET /userauth/{Mail}.",
                request.Mail);

            return null;
        }
    }

    [HttpPost("register")]
    public IActionResult CreateUser([FromBody] Users request)
    {
        _logger.LogInformation(
            "POST request received for user registration.");

        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Users? user = _authService.CreateUser(request.Username, request.Mail, request.Password,request.role);

            if (user == null)
            {
                return Conflict("A user with this email already exists.");
            }

            var userResponse = new
            {
                user.Reference_id,
                user.Username,
                user.Mail,
                user.role
            };

            _logger.LogInformation(
                "Successfully created user {Reference_id}.",
                user.Reference_id);

            return Ok(userResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing POST /userauth/register.");

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.");
        }
    }
}


