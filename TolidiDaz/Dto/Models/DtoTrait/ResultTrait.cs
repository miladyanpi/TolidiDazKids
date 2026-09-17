using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace  Dto.Models.DtoTrait
{
    public class ResultTrait : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("روش ارسال(عنوان)")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
