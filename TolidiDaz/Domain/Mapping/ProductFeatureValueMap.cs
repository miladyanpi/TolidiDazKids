using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class ProductFeatureValueMap : IEntityTypeConfiguration<ProductFeatureValue>
    {
        public void Configure(EntityTypeBuilder<ProductFeatureValue> builder)
        {
            builder.HasOne(x => x.Product)
           .WithMany(x=>x.ProductFeatureValues)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ProductFeature)
          .WithMany(x => x.ProductFeatureValues)
          .HasForeignKey(x => x.ProductFeatureID)
          .OnDelete(DeleteBehavior.Cascade);
        }
        
        }
}
