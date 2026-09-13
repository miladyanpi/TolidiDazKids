using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class WalletTransactionMap : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.HasOne(x => x.Wallet)
           .WithMany(x=>x.WalletTransactions)
           .HasForeignKey(x => x.WalletID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
