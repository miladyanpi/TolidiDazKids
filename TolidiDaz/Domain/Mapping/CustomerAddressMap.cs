using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CustomerAddressMap : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.HasOne(x => x.City)
           .WithMany(x => x.CustomerAddresss)
           .HasForeignKey(x => x.CityID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
           .WithMany(x => x.CustomerAddresss)
           .HasForeignKey(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
