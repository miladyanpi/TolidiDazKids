using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class WalletMap : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasOne(x => x.Customer)
           .WithOne(x=>x.Wallet)
           .HasForeignKey<Wallet>(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
