namespace Domain
{
    public class RawProductStore_Product:Base
    {
        public int? RawProductStoreID { get; set; }
        public int? ProductID { get; set; }
        public int? Count { get; set; }
        #region RelationShip
        public virtual RawProductStore? RawProductStore { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ICollection<RegisterCostRawProductStore>? RegisterCostRawProductStores { get; set; }
        #endregion
    }
}
