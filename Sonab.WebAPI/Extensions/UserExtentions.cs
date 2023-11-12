using System.Security.Claims;

namespace Sonab.WebAPI.Extensions;

public static class UserExtentions
{
    public static string GetUserId(this ClaimsPrincipal user) =>
        user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.ToUpper();
    public static string GetUserId(this IHttpContextAccessor accessor) =>
        accessor.HttpContext.User.GetUserId();

    public static bool TryGetUserId(this ClaimsPrincipal user, out string id)
    {
        Claim claim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claim != null)
        {
            id = claim.Value.ToUpper();
            return true;
        }
        else
        {
            id = null;
            return false;
        }
    }
    public static bool TryGetUserId(this IHttpContextAccessor accessor, out string id) =>
        accessor.HttpContext.User.TryGetUserId(out id);
}
