using BT.Models;
using BT.Implementation.Providers.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO
{
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
                        BugId = Convert.ToInt32(reader["bug_id"]),
                        Title = reader["title"].ToString()!,

                        Description = reader["description"] == DBNull.Value
                            ? null
                            : reader["description"].ToString(),

                        Priority = reader["priority"].ToString()!,
                        Status = reader["status"].ToString()!,

                        CreatedBy = Convert.ToInt32(reader["created_by"]),

                        Assignee = reader["assignee"] == DBNull.Value
                            ? null
                            : Convert.ToInt32(reader["assignee"]),

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
    }
}