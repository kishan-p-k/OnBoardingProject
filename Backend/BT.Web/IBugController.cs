using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web;

public interface IBugController
{
    List<Bug> GetAllBugs();
}