using System.Security.Claims;

namespace BookWorm.ServiceDefaults;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.NameIdentifier);

    public static string? GetUserEmail(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Email);

    public static string? GetUserRole(this ClaimsPrincipal principal) =>
        principal.FindFirstValue(ClaimTypes.Role);
}
