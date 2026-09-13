namespace Domain
{
    public class RegisterCostRawProductStore:Base
    {
        public int? RawProductStore_ProductID { get; set; }
        public int? PersonelID { get; set; }
        public int Count { get; set; }
        public Int64 Price { get; set; }

        #region RelationShip
        public virtual RawProductStore_Product? RawProductStore_Product { get; set; }
        public virtual Personel? Personel { get; set; }

        #endregion
        
    }
}
