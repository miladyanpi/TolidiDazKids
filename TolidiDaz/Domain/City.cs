namespace Domain
{
    public class City:Base
    {
        public int? ProvinceID { get; set; }
        public string? Title { get; set; }
        #region RelationShip
        public virtual Province? Province { get; set; }
        public virtual ICollection<CustomerAddress>? CustomerAddresss { get; set; }
        #endregion
    }
}
