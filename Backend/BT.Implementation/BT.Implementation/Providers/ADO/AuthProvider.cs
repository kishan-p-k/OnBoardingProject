using BT.Models;
using BT.Implementation.Providers.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO
{
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

        public Users? GetUserForLogin(string UsernameOrMail)
        {
            _logger.LogInformation(
                "Starting database operation to Authenticate User.");
            // Implementation for retrieving a user by username or email
            try
            {
                using (var connection = _databaseConnection.CreateConnection())
                {
                    using SqlCommand command =
                    new SqlCommand("GetUserForLogin", connection);

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(
                    "@UsernameOrMail",
                    SqlDbType.VarChar,
                    100
                ).Value = UsernameOrMail;

                    connection.Open();
                    _logger.LogDebug(
                   "Database connection opened. Executing stored procedure {ProcedureName}.",
                   "GetUserForLogin");
                    using SqlDataReader reader = command.ExecuteReader();
                    Users user = null;
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            Username = reader["username"].ToString(),
                            Password = reader["password"].ToString(),
                            Mail = reader["mail"].ToString()
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
    }
}
