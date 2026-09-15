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
}