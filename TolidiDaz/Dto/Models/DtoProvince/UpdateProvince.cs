using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoProvince
{
    public class UpdateProvince:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نام استان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
    }
}
