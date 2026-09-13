namespace Domain
{
    public class GroupQuestion:Base
    {
        public string? Title { get; set; }
        #region RelationShip
        public virtual ICollection<Question>? Questions { get; set; }
        #endregion
    }
}
