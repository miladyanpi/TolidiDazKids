namespace Domain
{
    public class FavoritUserProduct:Base
    {
        public int? CustomerID { get; set; }
        public int? ProductID { get; set; }

        #region RelationShip
        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
        #endregion

    }
}
