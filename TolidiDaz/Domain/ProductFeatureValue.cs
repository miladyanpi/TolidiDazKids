namespace Domain
{
    public class ProductFeatureValue:Base
    {
        public int? ProductFeatureID { get; set; }
        public int? ProductID { get; set; }
        public string? Value { get; set; }
        #region RelationShip
        public virtual Product? Product { get; set; }
        public virtual ProductFeature? ProductFeature { get; set; }
        #endregion
    }
}
