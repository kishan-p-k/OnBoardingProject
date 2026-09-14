using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web.Interfaces.Controllers;

public interface IBugDetailController
{
	Bug? GetBugById(string ref_id);
	IActionResult DeleteBug(string ref_id);
}

