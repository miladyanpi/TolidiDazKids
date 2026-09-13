using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(x => x.Order)
           .WithMany(x=>x.OrderItems)
           .HasForeignKey(x => x.OrderID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
          .WithMany(x => x.OrderItems)
          .HasForeignKey(x => x.ProductID)
          .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
