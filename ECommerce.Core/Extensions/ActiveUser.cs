using System.Security.Claims;

namespace ECommerce.Core.Extensions
{
    public static class ActiveUser
    {
        public static string GetActiveUserId(this ClaimsPrincipal claims)
        {
            var userClaims = claims.Identities.FirstOrDefault(_ => _.IsAuthenticated);
            return userClaims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string GetActiveUserFullName(this ClaimsPrincipal claims)
        {
            var userClaims = claims.Identities.FirstOrDefault(_ => _.IsAuthenticated);
            return userClaims?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetActivePhone(this ClaimsPrincipal claims)
        {
            var userClaims = claims.Identities.FirstOrDefault(_ => _.IsAuthenticated);
            return userClaims?.FindFirst(ClaimTypes.MobilePhone)?.Value;
        }
    }
}
