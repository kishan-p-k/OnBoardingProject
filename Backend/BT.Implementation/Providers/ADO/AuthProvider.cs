using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO;

public class AuthProvider : IAuthProvider
{
    private readonly DatabaseConnection _databaseConnection;
    private readonly ILogger<AuthProvider> _logger;
    public AuthProvider(
        DatabaseConnection databaseConnection,
        ILogger<AuthProvider> logger)
    {
        _databaseConnection = databaseConnection;
        _logger = logger;
    }

    public Users? GetUserForLogin(string Mail)
    {
        _logger.LogInformation(
            "Starting database operation to Authenticate User.");
        // Implementation for retrieving a user by username
        try
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                using SqlCommand command =
                new SqlCommand("GetUserForLogin", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                "@Mail",
                SqlDbType.VarChar,
                100
            ).Value = Mail;

                connection.Open();
                _logger.LogDebug(
               "Database connection opened. Executing stored procedure {ProcedureName}.",
               "GetUserForLogin");
                using SqlDataReader reader = command.ExecuteReader();
                Users? user = null;
                if (reader.Read())
                {
                    user = new Users
                    {
                        Reference_id = reader["reference_id"].ToString(),
                        Username = reader["username"].ToString(),
                        Password = reader["password"].ToString(),
                        Mail = reader["mail"].ToString(),
                        role = reader["role"].ToString()
                    };
                    _logger.LogInformation(
                "Successfully retrieved UserInfo users from the database.");
                    return user;

                }

                _logger.LogWarning(
                "No user found for the provided username or email.");

                return null;

            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user for login.");
            throw; // Rethrow the exception to be handled by the caller
        }
    }

    public Users? CreateUser(string username, string mail, string password, string role)
    {
        _logger.LogInformation(
            "Starting database operation to create a new user.");
        try
        {
            using (var connection = _databaseConnection.CreateConnection())
            {
                using SqlCommand command =
                new SqlCommand("CreateUser", connection);

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(
                "@Username",
                SqlDbType.VarChar,
                100
                ).Value = username;

                command.Parameters.Add(
                "@Mail",
                SqlDbType.VarChar,
                100
                ).Value = mail;

                command.Parameters.Add(
                "@Password",
                SqlDbType.VarChar,
                200
                ).Value = password;

                command.Parameters.Add(
                "@Role",
                SqlDbType.VarChar,
                50
                ).Value = role;

                connection.Open();
                _logger.LogDebug(
               "Database connection opened. Executing stored procedure {ProcedureName}.",
               "CreateUser");
                using SqlDataReader reader = command.ExecuteReader();
                Users? user = null;
                if (reader.Read())
                {
                    user = new Users
                    {
                        Reference_id = reader["reference_id"].ToString(),
                        Username = reader["username"].ToString(),
                        Mail = reader["mail"].ToString(),
                        role = reader["role"].ToString()
                    };
                    _logger.LogInformation(
                "Successfully created user in the database.");
                    return user;
                }

                _logger.LogWarning(
                "User could not be created. The mail may already be registered.");

                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user.");
            throw; // Rethrow the exception to be handled by the caller
        }
    }
}
