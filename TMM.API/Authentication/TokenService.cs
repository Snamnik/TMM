using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TMM.API.Authentication
{
    public record LoginDTO(string MobileNo, string Password);
    public record TokenDTO(string AccessToken, DateTime expires);

    public interface ITokenService
    {
        Task<TokenDTO> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken);
    }

    public class TokenService : ITokenService
    {
        private readonly IReadOnlyRepository<Customer> _customerRepository;
        private readonly JWTOptions _jwtOptions;
        private readonly List<Admin> _admins;

        public TokenService(IReadOnlyRepository<Customer> customerRepository, IOptionsMonitor<JWTOptions> jwtOptions, IOptionsMonitor<List<Admin>> admins)
        {
            _jwtOptions = jwtOptions.CurrentValue;
            _admins = admins.CurrentValue;
            _customerRepository = customerRepository;
        }
        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            var admin = _admins.Where(admin => admin.MobileNo == loginDTO.MobileNo && admin.Password.ToLower() == loginDTO.Password.ToLower()).FirstOrDefault();
            if (admin != null)
            {
                return GeneratJWT(admin.UserId, loginDTO.MobileNo, Roles.Admin);
            }

            var customer = await _customerRepository.FirstOrDefaultAsync(new CustomerSpec(loginDTO.MobileNo), cancellationToken);
            if (customer != null && customer.Forename.ToLower() == loginDTO.Password.ToLower())
            {
                return GeneratJWT(customer.Id, loginDTO.MobileNo, Roles.Customer);
            }


            throw new LoginFailedException(loginDTO.MobileNo);

        }
        private TokenDTO GeneratJWT(int userId, string username, string role)
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
