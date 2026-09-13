using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(x => x.Category)
           .WithMany(x => x.Products)
           .HasForeignKey(x => x.CategoryID)
           .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
