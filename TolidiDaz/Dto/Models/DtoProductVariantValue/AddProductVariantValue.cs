using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace  Dto.Models.DtoProductVariantValue
{
    public class AddProductVariantValue:BaseModel
    {

        [DisplayName("شناسه محصولات مختلف")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductVariantID { get; set; }
        [DisplayName("شناسه مقدار ویژگی")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? TraitValueID { get; set; }
    }
}
