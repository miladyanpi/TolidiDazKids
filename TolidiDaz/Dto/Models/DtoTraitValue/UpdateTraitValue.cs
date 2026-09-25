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
        [DisplayName("مقدار/عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Value { get; set; }
        [DisplayName("سازنده SKU")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Code { get; set; }
        [DisplayName("نمایش کد رنگ")]
        public string? DisplayColorHex { get; set; }
        
    }
}
