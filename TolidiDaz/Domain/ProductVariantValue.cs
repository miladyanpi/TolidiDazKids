namespace Domain
{
    /// <summary>
    /// ProductVariantId	TraitValueID	واریانت مربوطه	مقدار ویژگی متناظر
    //501	10	ست سرمه‌ای -L  رنگ: سرمه‌ای
    //501	20	ست سرمه‌ای -L  سایز: L
    //502	10	ست سرمه‌ای -XL رنگ: سرمه‌ای
    //502	21	ست سرمه‌ای -XL سایز: XL
    //503	11	ست کرم -L  رنگ: کرم
    //503	20	ست کرم -L  سایز: L
    //504	11	ست کرم -XL رنگ: کرم
    //504	21	ست کرم -XL سایز: XL
    /// </summary>
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
