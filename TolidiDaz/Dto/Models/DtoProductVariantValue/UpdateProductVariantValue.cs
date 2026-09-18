using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoProductVariantValue
{
    public class UpdateProductVariantValue:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شناسه محصولات مختلف")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? ProductVariantID { get; set; }
        [DisplayName("شناسه مقدار ویژگی")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? TraitValueID { get; set; }

    }
}
