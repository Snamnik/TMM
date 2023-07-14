using TMM.Infrastructure.Data.DBConfigurations;

namespace TMM.Infrastructure.Data
{
    public class TMMDbContext : DbContext
    {
        public const string SCHEMA = "TMM";
        public TMMDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA);

            modelBuilder.ApplyConfiguration(new CustomerDbConfig());
        }
    }
}