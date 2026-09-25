using Dto.Models.Base;
using Dto.Models.DtoPosition;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoProductVariant
{
    public class ResultProductVariant : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("کد انبار SKU")]
        public string? SkuCode { get; set; }
        [DisplayName("امضا")]
        public string? Signature { get; set; }
        [DisplayName("هزینه")]
        public Int64 Price { get; set; }
        [DisplayName("موجودی محصول")]
        public int Stock { get; set; }
        [DisplayName("مبلغ تخفیف")]//null = این کالا حراج نیست
        public Int64? SalePrice { get; set; }
        [DisplayName("تاریخ شروع تخفیف")]
        public DateTime? SaleStartsDate { get; set; }
        [DisplayName("تاریخ پایان تخفیف")]
        public DateTime? SaleEndsDate { get; set; }
        [DisplayName("بایگانی")]
        public bool IsArchived { get; set; }
        public ResultProduct? ResultProduct { get; set; }
    }
}
