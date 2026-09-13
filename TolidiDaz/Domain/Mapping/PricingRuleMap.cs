using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class PricingRuleMap : IEntityTypeConfiguration<PricingRule>
    {
        public void Configure(EntityTypeBuilder<PricingRule> builder)
        {
            builder.HasOne(x => x.Product)
           .WithMany(x=>x.PricingRules)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
