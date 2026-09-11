using Microsoft.AspNetCore.Mvc;

namespace BT.Web.Interfaces.Controllers;

public interface IBugController
{
    IActionResult GetAllBugs();
}