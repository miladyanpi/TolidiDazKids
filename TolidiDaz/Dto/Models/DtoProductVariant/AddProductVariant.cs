using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoProductVariant
{
    public class AddProductVariant:BaseModel
    {
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }
        [DisplayName("کد")]
        public string? SkuCode { get; set; }
        [DisplayName("هزینه")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("تعداد محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Count { get; set; }
    }
}
