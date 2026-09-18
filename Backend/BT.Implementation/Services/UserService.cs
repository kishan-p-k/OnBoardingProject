using BT.Models;
using BT;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;

namespace BT.Implementation.Services
{
    public class UserService : IUserService
    {
        private readonly IUserProvider _userProvider;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserProvider userProvider, ILogger<UserService> logger)
        {
            _userProvider = userProvider;
            _logger = logger;
        }

        public async Task<List<string>> UserSearch(string value)
        {
            _logger.LogInformation(
                "Fetching usernames for : {value}", value);

            try
            {
                List<string> users = await _userProvider.UserSearch(value);

                _logger.LogInformation(
                    "Successfully fetched {BugCount} usernames",
                   users.Count);

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching usernames");
                throw;
            }
        }
    }
}
