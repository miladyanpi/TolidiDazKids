using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
           // builder.HasOne(x => x.Job)
           //.WithMany(x => x.Customers)
           //.HasForeignKey(x => x.JobID)
           //.OnDelete(DeleteBehavior.Restrict);

        }

    }
}
