using Microsoft.AspNetCore.Mvc;
using BT.Models;

namespace BT.Web;

public interface IUserController
{
	Task<List<string>> UserSearch();
}