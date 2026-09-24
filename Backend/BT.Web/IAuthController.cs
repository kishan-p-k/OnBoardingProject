using BT.Models;
using Microsoft.AspNetCore.Mvc;

namespace BT.Web;

public interface IAuthController
{
    LoginResponseModel? Login(Users request);

    IActionResult CreateUser(Users request);
}