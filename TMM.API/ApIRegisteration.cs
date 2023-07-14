namespace TMM.Application
{
    public static class ApIRegisteration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<ExceptionHandler>();
            services.AddSingleton<LoggingFilter>();
            services.AddSingleton<InputValidationFilter>();
                
            return services;
        }
    }
}
