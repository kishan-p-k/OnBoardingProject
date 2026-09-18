using BT.Models;

namespace BT;

public interface IUserService
{
    public Task<List<string>> UserSearch(string value);
}
