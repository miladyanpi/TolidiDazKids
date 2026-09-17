using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class CategoryTraitMap : IEntityTypeConfiguration<CategoryTrait>
    {
        public void Configure(EntityTypeBuilder<CategoryTrait> builder)
        {
            builder.HasOne(x => x.Category)
           .WithMany(x => x.CategoryTraits)
           .HasForeignKey(x => x.CategoryID)
           .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
