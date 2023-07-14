using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace TMM.API.Authentication
{

    public class AuthenticationScheme
    {
        public const string TMMScheme = nameof(TMMScheme);
    }
    public class TMMAuthenticationOptions : AuthenticationSchemeOptions { }

    public class TMMAuthenticationHandler : AuthenticationHandler<TMMAuthenticationOptions>
    {
        private readonly JWTOptions _jwtOptions;

        public TMMAuthenticationHandler(IOptionsMonitor<TMMAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock
            , IOptionsMonitor<JWTOptions> jwtOptions) : base(options, logger, encoder, clock)
        {
            _jwtOptions = jwtOptions.CurrentValue;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorization = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            string accessToken = string.Empty;
            if (authorization.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
            {
                accessToken = authorization.Substring("bearer ".Length).Trim();
            }

            if (string.IsNullOrEmpty(accessToken))
            {
                return AuthenticateResult.Fail("Invalid Authorization Haeder");
            }

            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(accessToken, TokenValidationParameters, out var _);
                var ticket = new AuthenticationTicket(claimsPrincipal, AuthenticationScheme.TMMScheme);
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Invalid Access Token");
            }

        }
    }
}

