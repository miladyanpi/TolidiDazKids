using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoOrderItem
{
    public class AddOrderItem : BaseModel
    {
        public AddOrderItem()
        {
             Visible = true;    
        }
        public int OrderID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int ProductID { get; set; }
        [DisplayName("تعداد")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Quantity { get; set; }
        [DisplayName("قیمت")]
        [Required(ErrorMessage = "| الزامی است")]
        public Int64 PriceAtOrder { get; set; }

    }
}
