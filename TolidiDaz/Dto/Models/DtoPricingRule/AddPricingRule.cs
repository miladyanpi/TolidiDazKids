using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoPricingRule
{
    public class AddPricingRule: BaseModel
    {
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }
        [DisplayName("نوع مشتری")]
        public string? RoleID { get; set; }
        [DisplayName("نوع نقش قیمت گذاری")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? RuleType { get; set; }
        [DisplayName("حداقل تعداد خرید")]
        [Required(ErrorMessage = "| الزامی است")]
        public int MinQuantity { get; set; } = 1; 
        [DisplayName("قیمت محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("عنوان مناسبت")]
        public string? Title { get; set; }
        [DisplayName("از تاریخ")]
        public string? FromDate { get; set; }
        [DisplayName("تا تاریخ")]
        public string? ToDate { get; set; }
    }
}
