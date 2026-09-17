using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class TraitValueMap : IEntityTypeConfiguration<TraitValue>
    {
        public void Configure(EntityTypeBuilder<TraitValue> builder)
        {
            builder.HasOne(x => x.Trait)
           .WithMany(x => x.TraitValues)
           .HasForeignKey(x => x.TraitID)
           .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
