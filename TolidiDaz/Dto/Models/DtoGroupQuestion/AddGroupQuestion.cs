using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoGroupQuestion
{
    public class AddGroupQuestion : BaseModel
    {
        public AddGroupQuestion()
        {
        }
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }

    }
}
