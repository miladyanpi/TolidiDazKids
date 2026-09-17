using Dto.Models.Base;
using Dto.Models.DtoPosition;
using Dto.Models.DtoProduct;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoProductVariant
{
    public class ResultProductVariant : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("محصول")]
        public int? ProductID { get; set; }
        [DisplayName("کد")]
        public string? SkuCode { get; set; }
        [DisplayName("هزینه")]
        public Int64 Price { get; set; }
        [DisplayName("تعداد محصول")]
        public int Count { get; set; }
        public ResultProduct? ResultProduct { get; set; }
    }
}
