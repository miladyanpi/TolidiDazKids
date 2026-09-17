namespace Domain
{
    public class TraitValue:Base
    {
        public int TraitID { get; set; }
        public string? Value { get; set; } 
        public string? DisplayColorHex { get; set; }     
        #region Relations
        public virtual Trait? Trait { get; set; }
        public  virtual ICollection<ProductVariantValue>? ProductVariantValues { get; set; }

        #endregion

    }
}
