using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO;

public class BugProvider : IBugProvider
{
    private readonly DatabaseConnection _databaseConnection;
    private readonly ILogger<BugProvider> _logger;

    public BugProvider(
        DatabaseConnection databaseConnection,
        ILogger<BugProvider> logger)
    {
        _databaseConnection = databaseConnection;
        _logger = logger;
    }

    public List<Bug> GetAllBugs()
    {
        _logger.LogInformation(
            "Starting database operation to retrieve all bugs.");

        List<Bug> bugs = new List<Bug>();

        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();

            using SqlCommand command =
                new SqlCommand("GetAllBugs", connection);

            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            _logger.LogDebug(
                "Database connection opened. Executing stored procedure {ProcedureName}.",
                "GetAllBugs");

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Bug bug = new Bug
                {
                    Reference_id = reader["reference_id"].ToString()!,
                    Title = reader["title"].ToString()!,

                    Description = reader["description"] == DBNull.Value
                        ? null
                        : reader["description"].ToString(),

                    Priority = reader["priority"].ToString()!,
                    Status = reader["status"].ToString()!,

                    CreatedBy = reader["created_by"].ToString()!,

                    Assignee = reader["assignee"] == DBNull.Value
                        ? null
                        : reader["assignee"].ToString(),

                    CreatedDate = Convert.ToDateTime(
                        reader["created_date"])
                };

                bugs.Add(bug);
            }

            _logger.LogInformation(
                "Successfully retrieved {BugCount} bugs from the database.",
                bugs.Count);

            return bugs;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving all bugs.");

            throw;
        }
    }
    public List<Bug> FilterBugs(BugFilter filter)
    {
        _logger.LogInformation(
            "Starting database operation to filter bugs with criteria: {@Filter}.", filter);
        List<Bug> filteredBugs = new List<Bug>();
        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();
            using SqlCommand command =
                new SqlCommand("GetFilteredBugs", connection);
            command.CommandType = CommandType.StoredProcedure;
            // Add parameters for filtering
            command.Parameters.AddWithValue("@Keyword", (object?)filter.Keyword ?? DBNull.Value);
            command.Parameters.AddWithValue("@Status", (object?)filter.Status ?? DBNull.Value);
            command.Parameters.AddWithValue("@Priority", (object?)filter.Priority ?? DBNull.Value);
            command.Parameters.AddWithValue("@Assignee", (object?)filter.Assignee ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedBy", (object?)filter.CreatedBy ?? DBNull.Value);
            connection.Open();
            _logger.LogDebug(
                "Database connection opened. Executing stored procedure {ProcedureName}.",
                "GetFilteredBugs");
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Bug bug = new Bug
                {
                    Reference_id = reader["reference_id"].ToString()!,
                    Title = reader["title"].ToString()!,
                    Description = reader["description"] == DBNull.Value
                        ? null
                        : reader["description"].ToString(),
                    Priority = reader["priority"].ToString()!,
                    Status = reader["status"].ToString()!,
                    CreatedBy = reader["created_by"].ToString()!,
                    Assignee = reader["assignee"] == DBNull.Value
                        ? null
                        : reader["assignee"].ToString(),
                    CreatedDate = Convert.ToDateTime(
                        reader["created_date"])
                };
                filteredBugs.Add(bug);
            }
            _logger.LogInformation(
                "Successfully filtered bugs. Count: {Count}.",
                filteredBugs.Count);
            return filteredBugs;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while filtering bugs with criteria: {@Filter}.", filter);
            throw;
        }
    }
}