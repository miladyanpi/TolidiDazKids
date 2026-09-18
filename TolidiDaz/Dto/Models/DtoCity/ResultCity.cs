using Dto.Models.Base;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProvince;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoCity
{
    public class ResultCity : BaseModel
    {
        public int ID { get; set; }
        public int? ProvinceID { get; set; }
        [DisplayName("نام شهر")]
        public string? Title { get; set; }
        [DisplayName("نام استان")]
        public ResultProvince? ResultProvince { get; set; } = new();
    }
}
