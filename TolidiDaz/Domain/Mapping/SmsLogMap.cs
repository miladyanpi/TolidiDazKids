using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class SmsLogMap : IEntityTypeConfiguration<SmsLog>
    {
        public void Configure(EntityTypeBuilder<SmsLog> builder)
        {
            builder.HasOne(x => x.Customer)
           .WithMany(x => x.SmsLogs)
           .HasForeignKey(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
