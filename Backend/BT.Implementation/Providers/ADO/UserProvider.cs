using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO;

public class UserProvider : IUserProvider
{
    private readonly DatabaseConnection _databaseConnection;
    private readonly ILogger<UserProvider> _logger;

    public UserProvider(
        DatabaseConnection databaseConnection,
        ILogger<UserProvider> logger)
    {
        _databaseConnection = databaseConnection;
        _logger = logger;
    }
    public List<UserRequestModel> GetAllUsers()
    {
        _logger.LogInformation(
            "Starting database operation to retrieve all users.");
        List<UserRequestModel> users = new List<UserRequestModel>();
        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();
            using SqlCommand command =
                new SqlCommand("GetAllUsers", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            _logger.LogDebug(
                "Database connection opened. Executing stored procedure {ProcedureName}.",
                "GetAllUsers");
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                UserRequestModel user = new UserRequestModel
                {
                    Reference_id = reader["reference_id"].ToString()!,
                    Username = reader["username"].ToString()!,
                    Mail = reader["mail"].ToString()!,
                    role = reader["role"].ToString()!
                };
                users.Add(user);
            }
            _logger.LogInformation(
                "Successfully retrieved {UserCount} users from the database.",
                users.Count);
            return users;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving users.");
            throw;
        }
    }
    public async Task<List<string>> UserSearch(string value)
    {
        _logger.LogInformation(
            "Starting database operation to retrieve all Comments.");

        List<string> users = new List<string>();

        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();

            using SqlCommand command =
                new SqlCommand("UserSearch", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@SearchString", SqlDbType.VarChar).Value = value;

            await connection.OpenAsync();

            _logger.LogDebug(
                "Database connection opened. Executing stored procedure {ProcedureName}.",
                "UserSearch");

            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(reader["username"].ToString()!);
            }

            _logger.LogInformation(
                "Successfully retrieved {CommnetCount} users from the database.",
                users.Count);

            return users;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving comments.");

            throw;
        }
    }
}
