namespace TaskManagamentSystem.Api.Helpers;

using System.Security.Claims;

public static class ClaimsPrincipleExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(sub))
            throw new InvalidOperationException("User Id claim not found");

        return Guid.Parse(sub);
    }
}