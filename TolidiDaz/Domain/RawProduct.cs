namespace Domain
{
    /// <summary>
    /// محصولات خام یا اولیه
    /// </summary>
    public class RawProduct:Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        #region RelationShip
        public virtual ICollection<RawProductStore>? RawProductStores { get; set; }
        #endregion
    }
}
