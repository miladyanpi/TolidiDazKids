using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoOrder
{
    public class UpdateOrder: BaseModel
    {
        public int ID { get; set; }
        [DisplayName("مشتری")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CustomerID { get; set; }
        [DisplayName("روش ارسال محصول")]
        public int? SendProductMethodID { get; set; }
        [DisplayName("وضعیت سفارش")]
        [Required(ErrorMessage = "| الزامی است")]
        public OrderStatus OrderStatus { get; set; }
        [DisplayName("وضعیت پرداخت")]
        [Required(ErrorMessage = "| الزامی است")]
        public PaymentStatus PaymentStatus { get; set; }
        [DisplayName("مبلغ کل سبد")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 TotalAmount { get; set; }
        [DisplayName("تخفیف")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Discount { get; set; }
        [DisplayName("مبلغ نهایی")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 FinalAmount { get; set; }
        [DisplayName("کد سفارش")]
        public long OrderCode { get; set; }
        [DisplayName("شماره کارت")]
        public string? CardPen { get; set; }
        [DisplayName("شماره ارجاع")]
        public string? RefId { get; set; }
        [DisplayName("شماره پذیرنده")]
        public string? ResNum { get; set; }
        [DisplayName(" مشخصات آدرس")]
        public string? JsonAddress { get; set; }
        [DisplayName("توضیحات")]
        public string? StatusDescription { get; set; }
    }
}
