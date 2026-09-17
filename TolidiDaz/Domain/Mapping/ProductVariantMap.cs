using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class ProductVariantMap : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasOne(x => x.Product)
           .WithMany(x => x.ProductVariants)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
