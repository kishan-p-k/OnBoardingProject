using BT.Models;
using BT;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;

namespace BT.Implementation.Services;

    public class UserBugsService : IUserBugsService
    {
        private readonly IUserBugsProvider _userBugsProvider;
        private readonly ILogger<UserBugsService> _logger;

        public UserBugsService(IUserBugsProvider userBugsProvider, ILogger<UserBugsService> logger)
        {
            _userBugsProvider = userBugsProvider;
            _logger = logger;
        }

     public List<Bug> GetUserBugs(string Reference_id)
        {
            _logger.LogInformation(
                "Fetching user bugs for Reference_id: {Reference_id}", Reference_id);

            try
            {
                List<Bug> bugs = _userBugsProvider.GetUserBugs(Reference_id);

                _logger.LogInformation(
                    "Successfully fetched {BugCount} bugs for Reference_id: {Reference_id}", 
                    bugs.Count, Reference_id);

                return bugs;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Error fetching user bugs for Reference_id: {Reference_id}", Reference_id);
                throw;
            }
        }


        public Bug CreateBug(Bug bug)
        {
            _logger.LogInformation(
                "Creating a new bug");

            try
            {
                Bug bugs = _userBugsProvider.CreateBug(bug);

                _logger.LogInformation(
                    "Successfully created new bug");

                return bugs;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating new bug");
                throw;
            }
        }
}