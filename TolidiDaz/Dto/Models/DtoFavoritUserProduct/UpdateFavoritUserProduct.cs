using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoFavoritUserProduct
{
    public class UpdateFavoritUserProduct:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("کاربر")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CustomerID { get; set; }
        [DisplayName("محصول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductID { get; set; }

    }
}
