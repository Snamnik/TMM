namespace TMM.Infrastructure.Data
{
    public static class DBRegiteration
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(TMMRepository<>));
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(TMMRepository<>));

            services.AddDbContextPool<TMMDbContext>(options =>
            {
                options.EnableThreadSafetyChecks(false);
                options.EnableSensitiveDataLogging();

                if (bool.Parse(configuration["DB:UseInMemory"]) == true)
                {
                    options.UseInMemoryDatabase(configuration["DB:InMemoryDBName"]);
                }
                else
                {
                    options.UseSqlServer(configuration["DB:SQLConnection"], opt =>
                    {
                        opt.CommandTimeout(30);
                        opt.MigrationsAssembly("TMM.Infrastructure.Data");
                        opt.MigrationsHistoryTable("MigrationsHistory", TMMDbContext.SCHEMA);
                        opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                }

            });

            return services;
        }
    }
}
