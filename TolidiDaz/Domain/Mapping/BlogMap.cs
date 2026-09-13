using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class BlogMap : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasOne(x => x.GroupBlog)
           .WithMany(x => x.Blogs)
           .HasForeignKey(x => x.GroupBlogID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Team)
             .WithMany(x => x.Blogs)
             .HasForeignKey(x => x.TeamID)
             .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
