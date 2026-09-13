namespace Domain
{
    public class CustomerAddress:Base
    {
        public CustomerAddress()
        {
            Default = true;
        }
        public int? CityID { get; set; }
        public int? CustomerID { get; set; }
        public string? Address { get; set; }
        public string? Plaque { get; set; }
        public string? BuildingUnit { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }
        public bool Default { get; set; }

        #region RelationShip
        public virtual City? City { get; set; }
        public virtual Customer? Customer { get; set; }
        #endregion
    }
}
