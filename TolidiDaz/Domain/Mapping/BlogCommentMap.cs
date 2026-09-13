using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class BlogCommentMap : IEntityTypeConfiguration<BlogComment>
    {
        public void Configure(EntityTypeBuilder<BlogComment> builder)
        {
            builder.HasOne(x => x.Blog)
           .WithMany(x => x.BlogComments)
           .HasForeignKey(x => x.BlogID)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Customer)
          .WithMany(x => x.BlogComments)
          .HasForeignKey(x => x.CustomerID)
          .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
