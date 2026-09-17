using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web;

public interface IBugFilterController
{
    List<Bug> FilterBugs(BugFilter filter);
}