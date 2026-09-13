using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoCity
{
    public class AddCity:BaseModel
    {
        [DisplayName("استان")]
        public int? ProvinceID { get; set; }
        [DisplayName("نام شهر")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
     
    }
}
