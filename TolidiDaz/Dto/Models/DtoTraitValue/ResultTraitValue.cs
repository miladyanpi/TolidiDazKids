using Dto.Models.Base;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProvince;
using Dto.Models.DtoTrait;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoTraitValue
{
    public class ResultTraitValue : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("شناسه ویژگی")]
        public int? TraitID { get; set; }
        [DisplayName("مقادیر")]
        public string? Value { get; set; }
        [DisplayName("نمایش کد رنگ")]
        public string? DisplayColorHex { get; set; }
        public ResultTrait? ResultTrait { get; set; } = new();
    }
}
