using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;


namespace  Dto.Models.DtoTrait
{
    public class ResultTrait : BaseModel
    {
        public int ID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("نوع")]
        public string? DisplayType { get; set; }
        public int? DisplayType2 { get; set; }
    }
}
