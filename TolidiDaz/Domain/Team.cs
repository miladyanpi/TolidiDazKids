
namespace Domain
{
    public class Team:Base
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? JsonPictures { get; set; }
        public bool ShowInAbout { get; set; }
        #region RelationShip
        public virtual ICollection<Blog>? Blogs { get; set; }
        #endregion

    }
}
