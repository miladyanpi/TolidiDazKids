using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class FavoritUserProductMap : IEntityTypeConfiguration<FavoritUserProduct>
    {
        public void Configure(EntityTypeBuilder<FavoritUserProduct> builder)
        {
            builder.HasOne(x => x.Customer)
           .WithMany(x => x.FavoritUserProducts)
           .HasForeignKey(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
           .WithMany(x => x.FavoritUserProducts)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
