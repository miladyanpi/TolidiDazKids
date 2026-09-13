using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class PersonelMap : IEntityTypeConfiguration<Personel>
    {
        public void Configure(EntityTypeBuilder<Personel> builder)
        {
            builder.HasOne(x => x.Position)
           .WithMany(x=>x.Personels)
           .HasForeignKey(x => x.PositionID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
