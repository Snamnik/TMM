namespace TMM.API.Authentication
{
    public static class AuthenticationRegisteration
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ITokenService, TokenService>();

            services.Configure<JWTOptions>(configuration.GetSection("JWT"));
            services.Configure<List<Admin>>(configuration.GetSection("Admins"));

            services.AddAuthentication(AuthenticationScheme.TMMScheme).AddScheme<TMMAuthenticationOptions, TMMAuthenticationHandler>(AuthenticationScheme.TMMScheme, _ => { });

            return services;
       }
    }
}