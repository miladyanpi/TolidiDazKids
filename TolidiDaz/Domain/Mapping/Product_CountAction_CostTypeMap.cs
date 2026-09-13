using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Mapping
{
    public class Product_CountAction_CostTypeMap : IEntityTypeConfiguration<Product_CountAction_CostType>
    {
        public void Configure(EntityTypeBuilder<Product_CountAction_CostType> builder)
        {
            builder.HasOne(x => x.Position)
           .WithMany(x=>x.Product_CountAction_CostTypes)
           .HasForeignKey(x => x.PositionID)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
           .WithMany(x => x.Product_CountAction_CostTypes)
           .HasForeignKey(x => x.ProductID)
           .OnDelete(DeleteBehavior.Restrict);
        }
        
        }
}
