using Dto.Models.Base;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoCustomerAddress;
using Dto.Models.DtoSendProductMethod;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoOrder
{
    public class ResultOrder : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("مشتری")]
        public int? CustomerID { get; set; }
        [DisplayName("روش ارسال محصول")]
        public int? SendProductMethodID { get; set; }
        [DisplayName("وضعیت سفارش")]
        public string? OrderStatus { get; set; }
        [DisplayName("وضعیت سفارش")]
        public OrderStatus OrderStatus2 { get; set; }
        [DisplayName("وضعیت پرداخت")]
        public string? PaymentStatus { get; set; }
        public PaymentStatus PaymentStatus2 { get; set; }
        [DisplayName("مبلغ کل سبد")]
        public Int64 TotalAmount { get; set; }
        [DisplayName("تخفیف")]
        public Int64 Discount { get; set; }
        [DisplayName("مبلغ نهایی")]
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

        public ResultCustomerAddress? ResultCustomerAddress { get; set; }=new ResultCustomerAddress();
        public ResultCustomer? ResultCustomer { get; set; }=new ResultCustomer();
        public ResultSendProductMethod? ResultSendProductMethod { get; set; } = new ResultSendProductMethod(); 
    }
}
