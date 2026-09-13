using Dto.Models.Base;
using Dto.Models.DtoCustomer;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoCart
{
    public class ResultCart : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("مشتری")]
        public int? CustomerID { get; set; }
        [DisplayName("روش ارسال محصول")]
        public int? SendProductMethodID { get; set; }
        [DisplayName("وضعیت سبد")]
        public string? CartStatus { get; set; }
        public CartStatus CartStatus2 { get; set; }
        [DisplayName("مبلغ کل سبد")]
        public Int64 TotalAmount { get; set; }
        [DisplayName("تخفیف")]
        public Int64 Discount { get; set; }
        [DisplayName("مبلغ نهایی")]
        public Int64 FinalAmount { get; set; }
        [DisplayName("تعداد اقلام")]
        public int? CountCartItems{ get; set; }
        public ResultCustomer? ResultCustomer { get; set; } = new ResultCustomer();


    }
}
