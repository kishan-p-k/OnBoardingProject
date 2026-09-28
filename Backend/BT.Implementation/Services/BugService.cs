using BT.Models;
using BT;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;

namespace BT.Implementation.Services
{
    public class BugService : IBugService
    {
        private readonly IBugProvider _bugProvider;
        private readonly ILogger<BugService> _logger;

        public BugService(
            IBugProvider bugProvider,
            ILogger<BugService> logger)
        {
            _bugProvider = bugProvider;
            _logger = logger;
        }

        public List<Bug> GetAllBugs()
        {
            _logger.LogInformation("Fetching all bugs.");

            try
            {
                List<Bug> bugs = _bugProvider.GetAllBugs();

                _logger.LogInformation(
                    "Successfully fetched {BugCount} bugs.",
                    bugs.Count);

                return bugs;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to fetch all bugs.");

                throw;
            }
        }
        public List<Bug> FilterBugs(BugFilter filter,string role)
        {
            _logger.LogInformation(
                "Filtering bugs with criteria: {@Filter}.", filter);
            try
            {

                if(filter.Status != null && (filter.Status=="Closed" || filter.Status=="Resolved") && role == "Developer")
                {
                    _logger.LogInformation(
                        "Developer role detected. Overriding status filter to include only 'Open' and 'In Progress' bugs.");
                    throw new UnauthorizedAccessException("Developers are not allowed to filter bugs with status 'Closed' or 'Resolved'.");
                }
                List<Bug> filteredBugs = _bugProvider.FilterBugs(filter);
                _logger.LogInformation(
                    "Successfully filtered bugs. Count: {Count}.",
                    filteredBugs.Count);

                return filteredBugs;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Unauthorized access attempt while filtering bugs with criteria: {@Filter}.", filter);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to filter bugs with criteria: {@Filter}.", filter);
                throw;
            }
        }
    }
}