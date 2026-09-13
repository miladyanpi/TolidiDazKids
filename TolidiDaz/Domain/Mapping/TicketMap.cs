using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class TicketMap : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(x => x.Department)
           .WithMany(x => x.Tickets)
           .HasForeignKey(x => x.DepartmentID)
           .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
