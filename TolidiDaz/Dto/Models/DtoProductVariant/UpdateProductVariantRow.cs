using Dto.Models.Base;
using Dto.Models.DtoTraitValue;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoProductVariant
{
    public class UpdateProductVariantRow : BaseModel
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
        public bool IsArchived { get; set; }   // false = فعال و فروش; true = از رده خارج
        public int? VariantId { get; set; }
        public bool ExistsInDb { get; set; }
        public bool WasArchived { get; set; }
        public string Status { get; set; } = "موجود";
        public List<ResultTraitValue> Chips { get; set; } = new();
        public string? SkuPreview { get; set; }
        public bool PriceTouched { get; set; }
        public bool StockTouched { get; set; }
    }
}
