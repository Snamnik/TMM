using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace TMM.Infrastructure.Data.DBConfigurations
{
    public class CustomerDbConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Forename).IsRequired().HasMaxLength(50);
            builder.Property(c => c.Surname).IsRequired().HasMaxLength(50);
            builder.Property(c => c.EmailAddress).IsRequired().HasMaxLength(75);
            builder.Property(c => c.MobileNo).IsRequired().HasMaxLength(15);

            builder.Ignore(c => c.MainAddress);

            //builder.OwnsMany(c => c.Addresses, address => { address.ToTable("Addresses"); });
            builder.OwnsMany(
                    c => c.Addresses, address =>
                    {
                        address.WithOwner().HasForeignKey("CustomerId");
                        address.Property(x=>x.Id);
                        address.HasKey(x => x.Id);
                        address.ToTable("Addresses");
                    });

            //builder
            //.OwnsMany(c => c.AddAddress, a =>
            //{
            //    // Explicit configuration of the shadow key property in the owned type 
            //    // as a workaround for a documented issue in EF Core 5: https://github.com/dotnet/efcore/issues/20740
            //    a.Id;
            //    .UseHiLo("customerseq", TMMDbContext.SHCEMA);
            //    a.WithOwner();
            //});
        }
    }
}
