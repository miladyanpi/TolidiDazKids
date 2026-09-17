namespace Domain
{
    /// <summary>
    /// قیمت محصولات مختلف
    /// Id	ProductId	     SkuCode	             Price	       Count
    //  501	   100	         SET-A1115-NAVY-L	     899,000	       5
    //  502	   100	         SET-A1115-NAVY-XL	     899,000	       2
    //  503	   100	         SET-A1115-CREAM-L	     920,000	       0
    //  504	   100	         SET-A1115-CREAM-XL	     950,000	       8
    /// </summary>
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
