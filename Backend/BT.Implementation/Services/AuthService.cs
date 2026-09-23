using BT.Models;
using BT;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;

namespace BT.Implementation.Services;

public class AuthService : IAuthService
{
    private readonly IAuthProvider _authProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IAuthProvider authProvider,
        ILogger<AuthService> logger)
    {
        _authProvider = authProvider;
        _logger = logger;
    }

    public UserRequestModel? GetUserForLogin(string username, string password)
    {
        _logger.LogInformation(
            "Starting authentication for user.");
        try
        {
            Users? user = _authProvider.GetUserForLogin(username);

            if (user == null)
            {
                _logger.LogWarning(
                    "Authentication failed: user not found for username/email {Username}.",
                    username);
                return null;
            }

            // Verify password - in production, this should use proper hashing comparison
            // For now, basic string comparison (WARNING: NOT SECURE - for demo only)
            if (!user.Password.Equals(password, StringComparison.Ordinal))
            {
                _logger.LogWarning(
                    "Authentication failed: password mismatch for user {Username}.",
                    username);
                return null;
            }

            _logger.LogInformation(
                "Successfully authenticated user {Reference_id}.",
                user.Reference_id);

            return new UserRequestModel
            {
                Reference_id = user.Reference_id,
                Username = user.Username,
                Mail = user.Mail
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred during authentication.");
            throw;
        }
    }

    public Users? CreateUser(string username, string mail, string password)
    {
        _logger.LogInformation(
            "Starting user registration for {Mail}.", mail);
        try
        {
            Users? user = _authProvider.CreateUser(username, mail, password);

            if (user == null)
            {
                _logger.LogWarning(
                    "User registration failed for {Mail}. Mail may already be in use.",
                    mail);
                return null;
            }

            _logger.LogInformation(
                "Successfully registered user {Reference_id}.",
                user.Reference_id);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred during user registration.");
            throw;
        }
    }
}
