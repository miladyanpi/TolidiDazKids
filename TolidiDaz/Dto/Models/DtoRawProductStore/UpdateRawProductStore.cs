using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoRawProductStore
{
    public class UpdateRawProductStore:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان پارچه")]
        public int? RawProductID { get; set; }
        [DisplayName("نوع اندازه گیری")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? MessurmentType { get; set; }
        /// <summary>
        /// مقدار یا تعداد به ازای هر کیلو یا متر
        /// </summary>
        [DisplayName("مقدار")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Amount { get; set; }
        [DisplayName("مبلغ هر واحد(تومان)")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("تاریخ خرید")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? BuyDate { get; set; }

    }
}
