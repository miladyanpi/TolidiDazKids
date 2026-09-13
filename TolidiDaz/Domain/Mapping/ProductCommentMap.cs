using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class ProductCommentMap : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.HasOne(x => x.Product)
           .WithMany(x => x.ProductComments)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Customer)
         .WithMany(x => x.ProductComments)
         .HasForeignKey(x => x.CustomerID)
         .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
