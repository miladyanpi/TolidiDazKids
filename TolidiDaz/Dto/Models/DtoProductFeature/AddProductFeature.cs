using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoProductFeature
{
    public class AddProductFeature:BaseModel
    {
        [DisplayName("دسته بندی")]
        public int? CategoryID { get; set; }
        [DisplayName("عنوان ویژگی")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
      
    }
}
