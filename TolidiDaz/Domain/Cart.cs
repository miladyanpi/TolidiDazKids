namespace Domain
{
    public class Cart:Base
    {
        public int? CustomerID { get; set; }
        public int? SendProductMethodID { get; set; }
        public int CartStatus { get; set; }
        #region RelationShip
        public virtual Customer? Customer { get; set; }
        public virtual SendProductMethod? SendProductMethod { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
        #endregion
    }
}
