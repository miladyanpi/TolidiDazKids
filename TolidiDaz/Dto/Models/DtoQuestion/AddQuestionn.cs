using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoQuestion
{
    public class AddQuestion : BaseModel
    {
        public int? GroupQuestionID { get; set; }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("تیتر")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? SubTitle { get; set; }
        [DisplayName("توضیحات")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Description { get; set; }


    }
}
