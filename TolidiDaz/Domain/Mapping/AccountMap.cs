using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasOne(x => x.Customer)
                   .WithOne(x => x.Account)
                   .HasForeignKey<Account>(x => x.CustomerID)
                   .OnDelete(DeleteBehavior.Cascade);



        }

    }
}
