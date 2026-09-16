using BT.Models;
using BT;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;

namespace BT.Implementation.Services;

public class BugDetailService : IBugDetailService
{
    private readonly IBugDetailProvider _bugDetailProvider;
    private readonly ILogger<BugDetailService> _logger;

    public BugDetailService(
        IBugDetailProvider bugDetailProvider,
        ILogger<BugDetailService> logger)
    {
        _bugDetailProvider = bugDetailProvider;
        _logger = logger;
    }
    public Bug? GetBugById(string ref_id)
    {
        _logger.LogInformation(
            "Fetching bug with ID {ref_id}.", ref_id);
        try
        {
            Bug? bug = _bugDetailProvider.GetBugById(ref_id);
            if (bug == null)
            {
                _logger.LogWarning(
                    "No bug found with ID {ref_id}.", ref_id);
            }
            else
            {
                _logger.LogInformation(
                    "Successfully fetched bug with ID {ref_id}.", ref_id);
            }
            return bug;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to fetch bug with ID {ref_id}.", ref_id);
            throw;
        }
    }


    public bool DeleteBug(string ref_id)
    {
        _logger.LogInformation(
            "Fetching bug with ID {ref_id}.", ref_id);
        try
        {
            return _bugDetailProvider.DeleteBug(ref_id);
            /*if (bug == null)
            {
                _logger.LogWarning(
                    "No bug found with ID {ref_id}.", ref_id);
            }
            else
            {
                _logger.LogInformation(
                    "Successfully fetched bug with ID {ref_id}.", ref_id);
            }*/
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to fetch bug with ID {ref_id}.", ref_id);
            throw;
        }
    }

        public Bug? UpdateBugField(string ref_id, string bugField, string bugValue)
        {
            _logger.LogInformation(
                "Updating bug with ID {ref_id}.", ref_id);
            try
            {
                Bug? bug = _bugDetailProvider.UpdateBugField(ref_id,bugField,bugValue);
                if (bug == null)
                {
                    _logger.LogWarning(
                        "No bug found with ID {ref_id}.", ref_id);
                }
                else
                {
                    _logger.LogInformation(
                        "Successfully updated bug with ID {ref_id}.", ref_id);
                }
                return bug;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to fetch bug with ID {ref_id}.", ref_id);
                throw;
            }
        }


}