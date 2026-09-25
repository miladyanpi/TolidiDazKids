using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

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
    [Index(nameof(SkuCode), IsUnique = true)]
    public class ProductVariant:Base
    {
        public int ProductID { get; set; }

        public string? SkuCode { get; set; }
        public string? Signature { get; set; }
        public Int64 Price { get; set; } // قیمت اصلی
        public int Stock { get; set; }//موجودی
        public Int64? SalePrice { get; set; }            // null = این کالا حراج نیست
        public DateTime? SaleStartsDate { get; set; }
        public DateTime? SaleEndsDate { get; set; }
        public bool IsArchived { get; set; }   // false = فعال و فروش; true = از رده خارج
        #region Relation
        public virtual Product? Product { get; set; }
        public virtual ICollection<ProductVariantValue>? ProductVariantValues { get; set; } 
        #endregion
    }
}
