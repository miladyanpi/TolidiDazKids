using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class QuestionMap : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasOne(x => x.GroupQuestion)
           .WithMany(x => x.Questions)
           .HasForeignKey(x => x.GroupQuestionID)
           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
