using BT;
using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace BT.Implementation.Services;

public class AuthService : IAuthService
{
    private readonly IAuthProvider _authProvider;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;

    public AuthService(
        IAuthProvider authProvider,
        ILogger<AuthService> logger,
        IConfiguration configuration)
    {
        _authProvider = authProvider;
        _logger = logger;
        _configuration = configuration;
    }

    public LoginResponseModel? GetUserForLogin(string username, string password)
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
            if (hasher.VerifyHashedPassword(null!, user.Password, password) == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning(
                    "Authentication failed: password mismatch for user {Username}.",
                    username);
                return null;
            }

            _logger.LogInformation(
                "Successfully authenticated user {Reference_id}.",
                user.Reference_id);

            var claims = new List<Claim>
            {
                new Claim("Reference_id", user.Reference_id),
                new Claim("Username", user.Username),
                new Claim("Mail", user.Mail),
                new Claim(ClaimTypes.Role, user.role)
            };

            var key = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "30")
                    ),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseModel
            {
                User = new UserRequestModel
                {
                    Reference_id = user.Reference_id,
                    Username = user.Username,
                    Mail = user.Mail,
                    role = user.role
                },
                Token = tokenString

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
            var hasher = new PasswordHasher<object>();

            string hashedPassword = hasher.HashPassword(null!, password);
          
            Users? user = _authProvider.CreateUser(username, mail, hashedPassword);

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
