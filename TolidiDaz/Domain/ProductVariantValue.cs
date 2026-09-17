namespace Domain
{
    public class ProductVariantValue:Base
    {
        public int ProductVariantID { get; set; }
        public int TraitValueID { get; set; }
        #region Relation
        public virtual ProductVariant? ProductVariant { get; set; }
        public virtual TraitValue? TraitValue { get; set; }
        #endregion
    }
}
