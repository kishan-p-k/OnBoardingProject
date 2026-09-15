using Microsoft.AspNetCore.Mvc;

namespace BT.Web;

public interface IAuthController
{
    IActionResult Login(string Mail, string password);
}