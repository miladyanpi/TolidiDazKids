namespace Domain
{
    public class CategoryTrait : Base
    {
        public int CategoryID { get; set; }
        public int TraitID { get; set; }
        #region Relations
        public virtual Category? Category { get; set; } 
        public virtual Trait? Trait { get; set; }
        #endregion
    }
}
