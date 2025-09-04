using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class AuthHelper
{
    public static string GenerateToken(string secret, string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),

            //We use USR as a default role for testing purposes
            new Claim(ClaimTypes.Role, "USER")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            issuer: "TestIssuer",
            audience: "TaskManagementAudience",
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}