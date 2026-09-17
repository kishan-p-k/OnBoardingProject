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
        public List<Bug> FilterBugs(BugFilter filter)
        {
            _logger.LogInformation(
                "Filtering bugs with criteria: {@Filter}.", filter);
            try
            {
                List<Bug> filteredBugs = _bugProvider.FilterBugs(filter);
                _logger.LogInformation(
                    "Successfully filtered bugs. Count: {Count}.",
                    filteredBugs.Count);

                return filteredBugs;
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