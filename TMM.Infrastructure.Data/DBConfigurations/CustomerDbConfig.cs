namespace TMM.Infrastructure.Data.DBConfigurations
{
    public class CustomerDbConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Title).IsRequired().HasMaxLength(20).IsUnicode(false);
            builder.Property(c => c.Forename).IsRequired().HasMaxLength(50).IsUnicode(false);
            builder.Property(c => c.Surname).IsRequired().HasMaxLength(50).IsUnicode(false);
            builder.Property(c => c.EmailAddress).IsRequired().HasMaxLength(75).IsUnicode(false);
            builder.Property(c => c.MobileNo).IsRequired().HasMaxLength(15).IsUnicode(false);
            builder.Property(c => c.IsActive).IsRequired();

            builder.HasIndex(c => c.MobileNo).IsUnique();
            builder.HasIndex(c => c.EmailAddress).IsUnique();

            builder.Ignore(c => c.MainAddress);

            builder.OwnsMany(
                    c => c.Addresses, ownedBuilder =>
                    {
                        ownedBuilder.WithOwner().HasForeignKey("CustomerId");
                        ownedBuilder.ToTable("Addresses");
                        ownedBuilder.HasKey(a => a.Id);
                        ownedBuilder.Property(a => a.AddressLine1).IsRequired().HasMaxLength(80).IsUnicode(false);
                        ownedBuilder.Property(a => a.AddressLine2).IsRequired(false).HasMaxLength(80).IsUnicode(false);
                        ownedBuilder.Property(a => a.Town).IsRequired().HasMaxLength(50).IsUnicode(false);
                        ownedBuilder.Property(a => a.County).IsRequired(false).HasMaxLength(50).IsUnicode(false);
                        ownedBuilder.Property(a => a.Postcode).IsRequired().HasMaxLength(10).IsUnicode(false);
                        ownedBuilder.Property(a => a.Country).IsRequired().HasMaxLength(50).IsUnicode(false);
                        ownedBuilder.Property(a => a.IsMain).IsRequired();
                    });
        }
    }
}

