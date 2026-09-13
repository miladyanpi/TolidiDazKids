using Dto.Models.Base;
using Dto.Models.DtoProvince;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCity
{
    public class UpdateCity:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("استان")]
        public int? ProvinceID { get; set; }
        [DisplayName("نام شهر")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        public ResultProvince? ResultProvince { get; set; }
    }
}
