using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoProduct_CountAction_CostType
{
    public class UpdateProduct_CountAction_CostType:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نوع هزینه برای ایجاد محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? PositionID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }
        [DisplayName("تعدا کار خاص روی محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int CountAction { get; set; }
        [DisplayName("هزینه")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 Price { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }

    }
}
