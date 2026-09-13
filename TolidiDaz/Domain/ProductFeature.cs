namespace Domain
{
    public class ProductFeature:Base
    {
        public int? CategoryID { get; set; }
        public string? Title { get; set; }
        #region RelationShip
        public virtual Category? Category { get; set; }
        public virtual List<ProductFeatureValue>? ProductFeatureValues { get; set; }
        #endregion
    }
}
