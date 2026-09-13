namespace Domain
{
    public class CartItem:Base
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        #region RelationShip
        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
        #endregion
    }
}
