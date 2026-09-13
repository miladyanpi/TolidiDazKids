using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCartItem
{
    public class AddCartItem : BaseModel
    {
        public AddCartItem()
        {
             Visible = true;    
        }
        public int CartID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int ProductID { get; set; }
        [DisplayName("تعداد")]
        [Required(ErrorMessage = "| الزامی است")]
        public int Quantity { get; set; }

    }
}
