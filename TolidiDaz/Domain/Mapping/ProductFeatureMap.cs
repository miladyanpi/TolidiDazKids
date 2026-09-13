using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class ProductFeatureMap : IEntityTypeConfiguration<ProductFeature>
    {
        public void Configure(EntityTypeBuilder<ProductFeature> builder)
        {
            builder.HasOne(x => x.Category)
           .WithMany(x=>x.ProductFeatures)
           .HasForeignKey(x => x.CategoryID)
           .OnDelete(DeleteBehavior.Cascade);
        }
        
        }
}
