using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web.Interfaces.Controllers
{
    public interface IUserBugsController
    {
        List<Bug> GetUserBugs(string ReferenceId);
    }
}