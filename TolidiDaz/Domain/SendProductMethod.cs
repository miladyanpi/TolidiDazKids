namespace Domain
{
    public class SendProductMethod : Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        #region RelationShip
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        #endregion

    }
}
