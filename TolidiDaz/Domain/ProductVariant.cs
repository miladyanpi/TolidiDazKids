namespace Domain
{
    public class ProductVariant:Base
    {
        public int ProductID { get; set; }
        public string? SkuCode { get; set; } 
        public Int64 Price { get; set; }
        public int Count { get; set; }
        #region Relation
        public virtual Product? Product { get; set; }
        public virtual ICollection<ProductVariantValue>? ProductVariantValues { get; set; } 
        #endregion
    }
}
