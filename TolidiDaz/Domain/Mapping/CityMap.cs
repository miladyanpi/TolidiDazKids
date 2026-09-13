using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CityMap : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasOne(x => x.Province)
           .WithMany(x => x.Citys)
           .HasForeignKey(x => x.ProvinceID)
           .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
