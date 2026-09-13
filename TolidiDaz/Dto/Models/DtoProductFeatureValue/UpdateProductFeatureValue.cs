using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoProductFeature
{
    public class UpdateProductFeatureValue:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("عنوان ویژگی")]
        public int? ProductFeatureID { get; set; }
        [DisplayName("مقدار")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Value { get; set; }
        public ResultProductFeature? ResultProductFeature { get; set; }

    }
}
