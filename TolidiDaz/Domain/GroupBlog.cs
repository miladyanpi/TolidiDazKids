namespace Domain
{
    public class GroupBlog:Base
    {
        public string? Title { get; set; }
        #region RelationShip
        public virtual ICollection<Blog>? Blogs { get; set; }
        #endregion
    }
}
