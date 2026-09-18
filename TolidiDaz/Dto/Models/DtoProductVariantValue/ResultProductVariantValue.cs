using Dto.Models.Base;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProductVariant;
using Dto.Models.DtoTraitValue;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoProductVariantValue
{
    public class ResultProductVariantValue : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شناسه محصولات مختلف")]
        public int? ProductVariantID { get; set; }
        [DisplayName("شناسه مقدار ویژگی")]
        public int? TraitValueID { get; set; }
        public ResultProductVariant? ResultProductVariant { get; set; }
        public ResultTraitValue? ResultTraitValue { get; set; }

    }
}
