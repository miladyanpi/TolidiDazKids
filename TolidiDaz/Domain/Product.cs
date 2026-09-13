namespace Domain
{
    public class Product:Base
    {
        public int? CategoryID { get; set; }
        public string? ProductCode { get; set; }
        public string? Title { get; set; }
        public string? Brand { get; set; }
        public string? SkuCode { get; set; }
        public Int64 Price { get; set; }
        public Int64 Discount { get; set; }
        public int Count { get; set; }
        public int? ProductExistStatus { get; set; }
        public string? JsonPicture { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public int ViewCount { get; set; } = 0;
        #region RelationShip
        public virtual Category? Category { get; set; }
        public virtual List<CartItem>? CartItems { get; set; }
        public virtual List<OrderItem>? OrderItems { get; set; }
        public virtual List<ProductFeatureValue>? ProductFeatureValues { get; set; }
        public virtual ICollection<PricingRule>? PricingRules { get; set; }
        public virtual ICollection<Product_CountAction_CostType>? Product_CountAction_CostTypes { get; set; }
        public virtual ICollection<RawProductStore_Product>? RawProductStore_Products { get; set; }
        public virtual ICollection<FavoritUserProduct>? FavoritUserProducts { get; set; }
        public virtual ICollection<ProductComment>? ProductComments { get; set; }

        #endregion
    }
}
