using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class RegisterCostRawProductStoreMap : IEntityTypeConfiguration<RegisterCostRawProductStore>
    {
        public void Configure(EntityTypeBuilder<RegisterCostRawProductStore> builder)
        {
            builder.HasOne(x => x.RawProductStore_Product)
           .WithMany(x => x.RegisterCostRawProductStores)
           .HasForeignKey(x => x.RawProductStore_ProductID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Personel)
          .WithMany(x => x.RegisterCostRawProductStores)
          .HasForeignKey(x => x.PersonelID)
          .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
