namespace Domain
{
    /// <summary>
    /// مقادیر ویژگی 
    /// مثلا 
    //Id TraitID Value DisplayColorHex   
    //1   1       سرمه ای     #F5F5DC
    //2   1         مشکی      #F5F5DC
    //3   2          XL        Null
    /// </summary>
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
