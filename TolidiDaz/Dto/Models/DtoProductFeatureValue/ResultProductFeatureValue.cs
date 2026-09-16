using Dto.Models.Base;
using Dto.Models.DtoCategory;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProductFeature;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoProductFeatureValue
{
    public class ResultProductFeatureValue : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("عنوان ویژگی")]
        public int? ProductFeatureID { get; set; }
        [DisplayName("مقدار")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Value { get; set; }
        public ResultProduct? ResultProduct { get; set; }
        public ResultProductFeature? ResultProductFeature { get; set; }

    }
}
