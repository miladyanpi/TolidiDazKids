using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain
{
    /// <summary>
    /// اسلایدر
    /// </summary>
    public class Slider : Base
    {
        public string?  Title { get; set; }
        public string?  Url { get; set; }
        
        public string? JsonPicture { get; set; }
        public string? JsonVideo { get; set; }
        public string? Description { get; set; }

    }
}
