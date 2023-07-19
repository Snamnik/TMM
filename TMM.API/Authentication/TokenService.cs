using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TMM.API.Authentication
{
    public record TokenDTO(string AccessToken, DateTime expires);

    public interface ITokenService
    {
        TokenDTO GenerateJWT(int userId, string username, string role);
    }

    public class TokenService : ITokenService
    {
        private readonly JWTOptions _jwtOptions;

        public TokenService(IOptionsMonitor<JWTOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.CurrentValue;
           
        }
        public TokenDTO GenerateJWT(int userId, string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role )
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryTime);
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: signingCredentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenDTO(accessToken, expires);
        }

    }
}
