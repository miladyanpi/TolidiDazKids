namespace Domain
{
    /// <summary>
    ///  مقالات
    /// </summary>
    public class Blog : Base
    {
        public int? GroupBlogID { get; set; }
        public int? TeamID { get; set; }
        public string?  Title { get; set; }
        public string? StudyDuration { get; set; }
        public string? Description { get; set; }
        public int NumberOfVisits { get; set; }
        public string? JsonPicture { get; set; }
        #region RelationShip
        public virtual GroupBlog? GroupBlog { get; set; }
        public virtual Team? Team { get; set; }
        public virtual List<BlogComment>? BlogComments { get; set; }
        #endregion


    }
}
