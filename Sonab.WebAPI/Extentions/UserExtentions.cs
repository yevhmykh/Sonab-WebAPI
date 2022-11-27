using System.Security.Claims;

namespace Sonab.WebAPI.Extentions;

public static class UserExtentions
{
    public static string GetUserId(this ClaimsPrincipal user) =>
        user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
}
