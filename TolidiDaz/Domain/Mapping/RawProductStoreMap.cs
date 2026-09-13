using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class RawProductStoreMap : IEntityTypeConfiguration<RawProductStore>
    {
        public void Configure(EntityTypeBuilder<RawProductStore> builder)
        {
            builder.HasOne(x => x.RawProduct)
           .WithMany(x=>x.RawProductStores)
           .HasForeignKey(x => x.RawProductID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
