using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain
{
    /// <summary>
    ///  تبلیغات تک بخشی
    /// </summary>
    public class AdvertisementSingle : Base
    {
        public string?  Title { get; set; }
        public string?  SiteName { get; set; }
        public string? SiteUrl { get; set; }
        public string? JsonPicture { get; set; }
        public string? Description { get; set; }

    }
}
