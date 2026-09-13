using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain
{
    /// <summary>
    ///  تبلیغات دوبخشی
    /// </summary>
    public class Advertisement : Base
    {
        public string?  Title { get; set; }
        public string?  SiteName { get; set; }
        public string? SiteUrl { get; set; }
        public string? JsonPicture { get; set; }
        public string? Description { get; set; }

    }
}
