using Dto.Models.Base;
using Dto.Models.DtoProvince;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoTraitValue
{
    public class UpdateTraitValue:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شناسه ویژگی")]
        public int? TraitID { get; set; }
        [DisplayName("مقادیر")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Value { get; set; }
        [DisplayName("نمایش کد رنگ")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? DisplayColorHex { get; set; }
        
    }
}
