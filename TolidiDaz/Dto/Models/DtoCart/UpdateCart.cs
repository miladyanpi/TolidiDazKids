using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCart
{
    public class UpdateCart : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("مشتری")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CustomerID { get; set; }
        [DisplayName("روش ارسال محصول")]
        public int? SendProductMethodID { get; set; }
        [DisplayName("وضعیت سبد")]
        [Required(ErrorMessage = "| الزامی است")]
        public CartStatus CartStatus { get; set; }

    }
}
