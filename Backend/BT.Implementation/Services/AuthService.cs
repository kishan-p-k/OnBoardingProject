using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

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

    public UserRequestModel? GetUserForLogin(
        string username,
        string password)
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

            var hasher = new PasswordHasher<object>();

            if (hasher.VerifyHashedPassword(
                null!,
                user.Password,
                password) == PasswordVerificationResult.Failed)
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
                Mail = user.Mail,
                role = user.role
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

    public CreateUserResult CreateUser(
        string username,
        string mail,
        string password,
        string role)
    {
        _logger.LogInformation(
            "Starting user registration for {Mail}.",
            mail);

        try
        {
            var hasher = new PasswordHasher<object>();

            string hashedPassword =
                hasher.HashPassword(null!, password);

            CreateUserResult result =
                _authProvider.CreateUser(
                    username,
                    mail,
                    hashedPassword,
                    role);

            if (result.User == null)
            {
                _logger.LogWarning(
                    "User registration failed for {Mail}.",
                    mail);

                return result;
            }

            _logger.LogInformation(
                "Successfully registered user {Reference_id}.",
                result.User.Reference_id);

            return result;
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