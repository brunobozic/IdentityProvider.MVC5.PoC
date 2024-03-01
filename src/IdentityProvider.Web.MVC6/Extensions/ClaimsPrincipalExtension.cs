using IdentityProvider.Web.MVC6.Helpers;
using System.Security.Claims;

namespace IdentityProvider.Web.MVC6.Extensions;

public static class ClaimsPrincipalExtension
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var userId = long.Parse(user.FindFirstValue(JwtClaimNameConstants.ID_CLAIM_NAME));

        return userId;
    }

    public static bool IsSuperUser(this ClaimsPrincipal user)
    {
        if (bool.TryParse(user.FindFirstValue(JwtClaimNameConstants.GUEST_CLAIM_NAME), out var isSuperUser))
            return isSuperUser;
        return false;
    }
}