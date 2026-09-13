using Dto.Models.Base;
using Dto.Models.DtoRawProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoRawProductStore
{
    public class ResultRawProductStore : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان پارچه")]
        public int? RawProductID { get; set; }
        [DisplayName("نوع اندازه گیری")]
        public string? MessurmentType { get; set; }
        public MessurmentType MessurmentType2 { get; set; }
        /// <summary>
        /// مقدار یا تعداد به ازای هر کیلو یا متر
        /// </summary>
        [DisplayName("مقدار")]
        public int Amount { get; set; }
        [DisplayName("مبلغ هر واحد(تومان)")]
        public Int64 Price { get; set; }
        [DisplayName("تعداد برش")]
        public int SliceCount { get; set; } = 0;
        [DisplayName("تاریخ خرید")]
        public string? BuyDate { get; set; }
        [DisplayName("مبلغ کل خرید(تومان)")]
        public Int64 SumPrice { get; set; }
        [DisplayName("مبلغ کل هزینه(تومان)")]
        public Int64 SumPriceCastRaw { get; set; }

        [DisplayName("میانگین قیمت تمام شده هر محصول(تومان)")]
        public Int64 FinishPriceForAnyProduct{ get; set; }
        public ResultRawProduct? ResultRawProduct { get; set; }
    }
}
