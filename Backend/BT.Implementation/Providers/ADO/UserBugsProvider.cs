using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO;

public class UserBugsProvider : IUserBugsProvider
{
    private readonly DatabaseConnection _databaseConnection;
    private readonly ILogger<UserBugsProvider> _logger;

    public UserBugsProvider(DatabaseConnection databaseConnection, ILogger<UserBugsProvider> logger)
    {
        _databaseConnection = databaseConnection;
        _logger = logger;
    }

    public List<Bug> GetUserBugs(string referenceId)
    {
        var bugs = new List<Bug>();

        try
        {
            using SqlConnection connection = 
                _databaseConnection.CreateConnection();

            using SqlCommand command = 
                new SqlCommand("GetUserBugs", connection);
            
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(
                "@RefId", 
                SqlDbType.UniqueIdentifier
                ).Value = Guid.Parse(referenceId);

            connection.Open();
            _logger.LogDebug(
              "Database connection opened. Executing stored procedure {ProcedureName}.",
              "GetUserBugs");

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
                };
                bugs.Add(bug);
            }
            _logger.LogInformation(
                "Successfully retrieved {BugCount} bugs for user with reference ID {ReferenceId}.",
                bugs.Count, referenceId);

            return bugs;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving user bugs.");
            throw;
        }
    }
    public Bug CreateBug(Bug bug)
    {
        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();

            using SqlCommand command =
                new SqlCommand("CreateBug", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@Title", SqlDbType.VarChar).Value = bug.Title;

            command.Parameters.Add("@Description", SqlDbType.VarChar).Value = bug.Description;

            command.Parameters.Add("@Priority", SqlDbType.VarChar).Value = bug.Priority;

            command.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Open";

            command.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = bug.CreatedBy;

            command.Parameters.Add("@Assignee", SqlDbType.VarChar).Value = bug.Assignee;

            connection.Open();

            _logger.LogInformation(
                "Database connection opened successfully.");

            _logger.LogInformation(
                "Executing stored procedure {ProcedureName}.",
                "UpdateBugField");

            using SqlDataReader reader = command.ExecuteReader();

            _logger.LogInformation(
                "Stored procedure executed successfully.");

            if (!reader.Read())
            {
                _logger.LogWarning(
                    "Stored procedure returned no rows");
                 return null;
            }

            _logger.LogInformation(
                "Stored procedure returned a bug row. Reading result.");

            var newbug = new Bug
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

            _logger.LogInformation(
                "Bug successfully read after update. RefId: {ref_id}, Status: {Status}, Priority: {Priority}, Assignee: {Assignee}",
                newbug.Reference_id,
                newbug.Status,
                newbug.Priority,
                newbug.Assignee);

            return newbug;
        }
        catch (FormatException ex)
        {
            _logger.LogError(
                ex,
                "Invalid");

            throw;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while executing CreateBug");

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while creating bug.");

            throw;
        }
    }
}