using Dto.Models.Base;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoPricingRule
{
    public class ResultPricingRule : BaseModel
    {
        public int ID { get; set; }

        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("نوع مشتری")]
        public string? RoleID { get; set; }
        [DisplayName("نوع نقش قیمت گذاری")]
        public string? RuleType { get; set; }
        public int? RuleType2 { get; set; }

        [DisplayName("حداقل تعداد خرید")]
        public int MinQuantity { get; set; }
        [DisplayName("قیمت محصول")]
        public Int64 Price { get; set; }
        [DisplayName("عنوان مناسبت")]
        public string? Title { get; set; }

        [DisplayName("از تاریخ")]
        public string? FromDate { get; set; }
        [DisplayName("تا تاریخ")]
        public string? ToDate { get; set; }

    }
}
