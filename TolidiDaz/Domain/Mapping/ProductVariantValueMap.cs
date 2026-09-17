using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class ProductVariantValueMap : IEntityTypeConfiguration<ProductVariantValue>
    {
        public void Configure(EntityTypeBuilder<ProductVariantValue> builder)
        {
            builder.HasOne(x => x.ProductVariant)
           .WithMany(x => x.ProductVariantValues)
           .HasForeignKey(x => x.ProductVariantID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TraitValue)
           .WithMany(x => x.ProductVariantValues)
           .HasForeignKey(x => x.TraitValueID)
           .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
