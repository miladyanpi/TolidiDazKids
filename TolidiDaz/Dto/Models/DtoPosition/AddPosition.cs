using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoPosition
{
    public class AddPosition:BaseModel
    {
        [DisplayName("عنوان شغل")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Title { get; set; }
        [DisplayName("نوع هزینه")]
        [Required(ErrorMessage = "| الزامی است")]
        public int? CostType { get; set; }
        [DisplayName("مبلغ هزینه(تومان)")]
        public Int64 Price { get; set; }

        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
