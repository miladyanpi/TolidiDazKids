using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CartMap : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasOne(x => x.Customer)
           .WithMany(x => x.Carts)
           .HasForeignKey(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SendProductMethod)
             .WithMany(x => x.Carts)
             .HasForeignKey(x => x.SendProductMethodID)
             .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
