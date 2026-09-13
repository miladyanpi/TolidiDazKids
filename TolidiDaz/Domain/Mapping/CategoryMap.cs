using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasOne(x => x.Parent)
           .WithMany(x => x.Categories)
           .HasForeignKey(x => x.ParentID)
           .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
