namespace Domain
{
    /// <summary>
    ///  مشاغل
    /// </summary>
    public class Position:Base
    {
        public string? Title { get; set; }
        public int? CostType { get; set; }
        public Int64 Price { get; set; }
        public string? Description { get; set; }
        #region RelationShip
        public virtual ICollection<Personel>? Personels { get; set; }
        public virtual ICollection<Product_CountAction_CostType>? Product_CountAction_CostTypes { get; set; }

        #endregion
    }
}
