using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  Dto.Models.DtoRawProduct
{
    public class ResultRawProduct : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        public string? Title { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
