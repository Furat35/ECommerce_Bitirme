using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ECommerce.Business.Helpers.TokenServices
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateToken(List<Claim> authClaims);
    }
}
