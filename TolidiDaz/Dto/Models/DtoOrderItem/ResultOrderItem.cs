using Dto.Models.Base;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoOrderItem
{
    public class ResultOrderItem : BaseModel
    {
        public int ID { get; set; }
        public int? OrderID { get; set; }
        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("تعداد")]
        public int Quantity { get; set; }
        [DisplayName("قیمت")]
        public Int64 PriceAtOrder { get; set; }
        [DisplayName("مبلغ نهایی")]
        public Int64 FinalAmount { get; set; }
        public ResultProduct? ResultProduct { get; set; }


    }
}
