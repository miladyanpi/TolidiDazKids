using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoTrait
{
    public class AddTrait:BaseModel
    {
        public AddTrait()
        {
            Visible = true;
        }
        [DisplayName("عناون")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }

    }
}
