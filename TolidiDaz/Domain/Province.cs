namespace Domain
{
    public class Province:Base
    {
        public string? Title { get; set; }
        #region RelationShip
        public virtual ICollection<City>? Citys { get; set; }
        #endregion
    }
}
