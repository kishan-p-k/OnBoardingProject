using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web;

public interface IUserBugsController
{
    List<Bug> GetUserBugs(string ReferenceId);
}