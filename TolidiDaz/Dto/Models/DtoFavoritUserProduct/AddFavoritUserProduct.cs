using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace  Dto.Models.DtoFavoritUserProduct
{
    public class AddFavoritUserProduct:BaseModel
    {

        [DisplayName("کاربر")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CustomerID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }

    }
}
