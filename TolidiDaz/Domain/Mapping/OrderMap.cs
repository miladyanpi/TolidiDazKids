using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(x => x.Customer)
           .WithMany(x=>x.Orders)
           .HasForeignKey(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SendProductMethod)
          .WithMany(x => x.Orders)
          .HasForeignKey(x => x.SendProductMethodID)
          .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
