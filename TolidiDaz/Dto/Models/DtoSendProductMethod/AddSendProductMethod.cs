using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoSendProductMethod
{
    public class AddSendProductMethod:BaseModel
    {
        [DisplayName("روش ارسال(عنوان)")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }

    }
}
