using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoProductVariant
{
    public class UpdateProductVariant:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }
        [DisplayName("کد انبار SKU")]
        public string? SkuCode { get; set; }
        [DisplayName("امضا")]
        public string? Signature { get; set; }
        [DisplayName("هزینه")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("تعداد محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Stock { get; set; }
        [DisplayName("مبلغ تخفیف")]//null = این کالا حراج نیست
        public Int64? SalePrice { get; set; }
        [DisplayName("تاریخ شروع تخفیف")]
        public DateTime? SaleStartsDate { get; set; }
        [DisplayName("تاریخ پایان تخفیف")]
        public DateTime? SaleEndsDate { get; set; }
        [DisplayName("بایگانی")]
        public bool IsArchived { get; set; }

    }
}
