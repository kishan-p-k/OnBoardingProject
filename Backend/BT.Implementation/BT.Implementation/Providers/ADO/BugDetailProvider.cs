using BT.Models;
using BT.Implementation.Providers.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using BT.Implementation.Services;

namespace BT.Implementation.Providers.ADO
{
    public class BugDetailProvider : IBugDetailProvider
    {
        private readonly DatabaseConnection _databaseConnection;
        private readonly ILogger<BugDetailProvider> _logger;

        public BugDetailProvider(
            DatabaseConnection databaseConnection,
            ILogger<BugDetailProvider> logger)
        {
            _databaseConnection = databaseConnection;
            _logger = logger;
        }

        public Bug? GetBugById(string ref_id)
        {
            _logger.LogInformation(
                "Starting database operation to retrieve bug with ID {ref_id}.",
                ref_id);

            try
            {
                using SqlConnection connection =
                    _databaseConnection.CreateConnection();

                using SqlCommand command =
                    new SqlCommand("GetBugsById", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@ref_id", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ref_id);

                connection.Open();

                _logger.LogDebug(
                    "Database connection opened. Executing stored procedure {ProcedureName}.",
                    "GetBugsById");

                using SqlDataReader reader = command.ExecuteReader();

                if (!reader.Read())
                {
                    _logger.LogInformation(
                        "No bug found with ID {ref_id}.",
                        ref_id);

                    return null;
                }

                var bug = new Bug
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
                    "Successfully retrieved bug with ID {ref_id}.",
                    ref_id);

                return bug;
            }
            catch (SqlException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while retrieving bug with ID {ref_id}.",
                    ref_id);

                throw;
            }
        }
        public bool DeleteBug(string ref_id)
        {
            _logger.LogInformation(
                "Starting database operation to delete bug with ID {ref_id}.",
                ref_id);

            try
            {
                using SqlConnection connection =
                    _databaseConnection.CreateConnection();

                using SqlCommand command =
                    new SqlCommand("DeleteBug", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                    "@RefId",
                    SqlDbType.UniqueIdentifier
                ).Value = Guid.Parse(ref_id);

                connection.Open();

                _logger.LogDebug(
                    "Database connection opened. Executing stored procedure {ProcedureName}.",
                    "DeleteBug");

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    _logger.LogInformation(
                        "No bug found with ID {ref_id}.",
                        ref_id);

                    return false;
                }

                _logger.LogInformation(
                    "Successfully deleted bug with ID {ref_id}.",
                    ref_id);

                return true;
            }
            catch (SqlException ex)
            {
                _logger.LogError(
                    ex,
                    "Database error while deleting bug with ID {ref_id}.",
                    ref_id);

                throw;
            }
        }

    }
}
