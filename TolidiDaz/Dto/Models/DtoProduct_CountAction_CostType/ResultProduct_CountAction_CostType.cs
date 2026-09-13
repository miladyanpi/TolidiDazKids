using Dto.Models.Base;
using Dto.Models.DtoPosition;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoProduct_CountAction_CostType
{
    public class ResultProduct_CountAction_CostType : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نوع هزینه برای ایجاد محصول")]
        public int? PositionID { get; set; }
        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("تعدا کار خاص روی محصول")]
        public int CountAction { get; set; }
        [DisplayName("هزینه هر عدد(تومان)")]
        public Int64 Price { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        public ResultPosition? ResultPosition { get; set; }
        public ResultProduct? ResultProduct { get; set; }
    }
}
