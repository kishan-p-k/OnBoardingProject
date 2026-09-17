using BT.Models;

namespace BT.Implementation.Providers;

public interface IUserProvider
{
    public Task<List<string>> UserSearch(string value);
}