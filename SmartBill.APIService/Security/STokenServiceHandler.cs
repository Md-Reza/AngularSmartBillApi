using Microsoft.IdentityModel.Tokens;
using SmartBill.APIService.Entities;
using SmartBill.APIService.Handlers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SmartBill.APIService.Security
{
    public sealed class STokenServiceHandler(IConfiguration configuration) : STokenService
    {

        public readonly SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(configuration.GetSection("ApplicationSecurity:Key").Value));

        public string GenerateAccessToken(UserSecurity userSecurity)
        {
            Claim[] claims = [
               new Claim(JwtRegisteredClaimNames.NameId, userSecurity.User.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, userSecurity.User.UserName.ToUpper()),
                new Claim(JwtRegisteredClaimNames.GivenName, userSecurity.User.DisplayName.ToUpper()),
                new Claim(JwtRegisteredClaimNames.Sid, userSecurity.User.EmployeeID.ToString().ToLower()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())];


            var key = Encoding.ASCII.GetBytes(configuration.GetSection("ApplicationSecurity:Key").Value);
            var symmetricSecurityKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(SConstantHandler.TokenExpireMin),
                Audience = configuration.GetSection("ApplicationSecurity:Audience").Value,
                Issuer = configuration.GetSection("ApplicationSecurity:Issuer").Value,
                NotBefore = DateTime.Now,
                IssuedAt = DateTime.Now,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public string GenerateSecurityStamp()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public void GetSecurityHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using HMACSHA512 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public bool IsValidCredential(string password, UserSecurity userSecurity)
        {
            using var hmac = new HMACSHA512(userSecurity.PasswordSalt);
            byte[] computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != userSecurity.PasswordHash[i]) return false;
            }
            return true;
        }
        public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetSection("ApplicationSecurity:Issuer").Value,
                ValidAudience = configuration.GetSection("ApplicationSecurity:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("ApplicationSecurity:Key").Value)),
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            return jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase)
                ? throw new SecurityTokenException("Invalid access token")
                : principal;
        }
        public List<Claim> ParseClaimsFromJwtToken(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())).ToList();
        }
        public string ParseTokenFromJwtToken(string token)
        {
            _ = AuthenticationHeaderValue.TryParse(token, out AuthenticationHeaderValue headerValue);
            return headerValue?.Parameter;
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

    }
}
