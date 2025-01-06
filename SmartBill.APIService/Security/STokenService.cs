using SmartBill.APIService.Entities;
using System.Security.Claims;

namespace SmartBill.APIService.Security
{
    public interface STokenService
    {
        string GenerateAccessToken(UserSecurity userSecurity);
        string GenerateSecurityStamp();
        ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string accessToken);
        void GetSecurityHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool IsValidCredential(string password, UserSecurity userSecurity);
        List<Claim> ParseClaimsFromJwtToken(string jwt);
        string ParseTokenFromJwtToken(string token);
    }
}
