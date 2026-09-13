using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoProvince
{
    public class ResultProvince : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نام استان")]
        public string? Title { get; set; }
    }
}
