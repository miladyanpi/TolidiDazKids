using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CartItemMap : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(x => x.Cart)
           .WithMany(x=>x.CartItems)
           .HasForeignKey(x => x.CartID)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
          .WithMany(x => x.CartItems)
          .HasForeignKey(x => x.ProductID)
          .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
