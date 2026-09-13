namespace Domain
{
    public class OrderItem:Base
    {
        public int? OrderID { get; set; }
        public int? ProductID { get; set; }

        public int Quantity { get; set; }
        public Int64 PriceAtOrder { get; set; }
        #region RelationShip
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
        #endregion
    }
}
