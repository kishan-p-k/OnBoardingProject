using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Interfaces.Controllers;

public interface IAuthController
{
    IActionResult Login(string usernameOrMail, string password);
}