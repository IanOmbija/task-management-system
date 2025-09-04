using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManagamentSystem.Api.Models;
using TaskManagamentSystem.Api.Security;

namespace TaskManagamentSystem.Api.Services;

public class JwtTokenService
{
    private readonly JwtOptions _settings;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IOptions<JwtOptions> options, ILogger<JwtTokenService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }
    /// <summary>
    /// Generates a signed JWT token for a user
    /// Returns both the token and expiry timestamp
    /// </summary>

    public (string token, DateTime expiresAt) GenerateToken(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiresMin);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Success: Generated JWT successfully");
            return (jwtToken, expires);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error: An error while generating JWT Token");
            throw;
        }
    }
}