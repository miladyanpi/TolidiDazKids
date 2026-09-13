using Dto.Models.Base;
using Dto.Models.DtoProduct;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoCartItem
{
    public class ResultCartItem : BaseModel
    {
        public int ID { get; set; }
        public int CartID { get; set; }
        [DisplayName("محصول")]
        public int ProductID { get; set; }
        [DisplayName("تعداد")]
        public int Quantity { get; set; }
        [DisplayName("مبلغ نهایی")]
        public Int64 TotalAmount { get; set; }

        public ResultProduct? ResultProduct{ get; set; }

    }
}
