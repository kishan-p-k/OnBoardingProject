using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using BT.Implementation.Services;

namespace BT.Implementation.Providers.ADO;

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

 


public Bug? UpdateBugField(string ref_id, string bugField, string bugValue)
    {
        _logger.LogInformation(
            "Starting UpdateBugField. RefId: {ref_id}, Field: {bugField}, Value: {bugValue}",
            ref_id,
            bugField,
            bugValue);

        try
        {
            _logger.LogDebug(
                "Parsing RefId {ref_id} as Guid.",
                ref_id);

            Guid refId = Guid.Parse(ref_id);

            _logger.LogDebug(
                "Successfully parsed RefId: {refId}",
                refId);

            using SqlConnection connection =
                _databaseConnection.CreateConnection();

            _logger.LogDebug(
                "SQL connection object created.");

            using SqlCommand command =
                new SqlCommand("UpdateBugField", connection);

            command.CommandType = CommandType.StoredProcedure;

            _logger.LogDebug(
                "Stored procedure configured: {ProcedureName}",
                "UpdateBugField");

            command.Parameters.Add("@RefId", SqlDbType.UniqueIdentifier).Value = refId;

            command.Parameters.Add("@FieldName", SqlDbType.VarChar).Value = bugField;

            command.Parameters.Add("@FieldValue", SqlDbType.VarChar).Value = bugValue;

            _logger.LogDebug(
                "Parameters added. RefId: {refId}, FieldName: {bugField}, FieldValue: {bugValue}",
                refId,
                bugField,
                bugValue);

            _logger.LogDebug(
                "Opening database connection.");

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
                    "Stored procedure returned no rows. RefId: {ref_id}",
                    ref_id);

                return null;
            }

            _logger.LogInformation(
                "Stored procedure returned a bug row. Reading result.");

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
                "Bug successfully read after update. RefId: {ref_id}, Status: {Status}, Priority: {Priority}, Assignee: {Assignee}",
                bug.Reference_id,
                bug.Status,
                bug.Priority,
                bug.Assignee);

            return bug;
        }
        catch (FormatException ex)
        {
            _logger.LogError(
                ex,
                "Invalid RefId format: {ref_id}",
                ref_id);

            throw;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while executing UpdateBugField. RefId: {ref_id}, Field: {bugField}, Value: {bugValue}",
                ref_id,
                bugField,
                bugValue);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while updating bug. RefId: {ref_id}, Field: {bugField}, Value: {bugValue}",
                ref_id,
                bugField,
                bugValue);

            throw;
        }
    }
}