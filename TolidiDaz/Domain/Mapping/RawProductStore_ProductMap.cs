using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class RawProductStore_ProductMap : IEntityTypeConfiguration<RawProductStore_Product>
    {
        public void Configure(EntityTypeBuilder<RawProductStore_Product> builder)
        {
            builder.HasOne(x => x.RawProductStore)
           .WithMany(x=>x.RawProductStore_Products)
           .HasForeignKey(x => x.RawProductStoreID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
           .WithMany(x => x.RawProductStore_Products)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
