using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoLable
{
    public class ResultJsonLable
    {
        public Guid ID { get; set; }
        [DisplayName("برچسب")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Text { get; set; }
    }
}
