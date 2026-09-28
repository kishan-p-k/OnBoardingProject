using BT.Models;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web;

public interface IAuthController
{
    IActionResult Login(Users request);

    IActionResult CreateUser(Users request);
}