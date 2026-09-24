using BT.Models;
using BT;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Implementation.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(
        IUserService userService,
        ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    [HttpGet]
    public List<UserRequestModel> GetAllUsers()
    {
        _logger.LogInformation(
            "GET request received for all users.");
        try
        {
            List<UserRequestModel> users = _userService.GetAllUsers();
            _logger.LogInformation(
                "Returning {UserCount} users.", users.Count);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET users.");
            return new List<UserRequestModel>();
        }
    }
    [HttpGet("{value}")]
    public async Task<List<string>> UserString(string value)
    {
        _logger.LogInformation(
            "GET request received for usernames for {value}.",value);
        try
        {
            List<string> users = await _userService.UserSearch(value);
            _logger.LogInformation(
                "Returning {BugCount} usernames.",users.Count);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while processing GET users/{value}.", value);
            return new List<string>();
        }
    }
}
