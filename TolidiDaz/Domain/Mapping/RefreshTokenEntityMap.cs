using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class RefreshTokenEntityMap : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.HasOne(x => x.Account)
           .WithMany(x=>x.RefreshTokenEntitys)
           .HasForeignKey(x => x.UserId)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
