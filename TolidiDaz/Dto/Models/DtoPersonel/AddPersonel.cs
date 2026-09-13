using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoPersonel
{
    public class AddPersonel:BaseModel
    {
        [DisplayName("نام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Name { get; set; }
        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? LastName { get; set; }
        [DisplayName("جنسیت")]
        [Required(ErrorMessage = "| الزامی است")]
        public Gender Gender { get; set; }
        [DisplayName("تاریخ تولد")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? BirthDate { get; set; }
        [RegularExpression(@"^0([0-9]{10})$", ErrorMessage = "| شماره همراه را با فرمت درست وارد نمایید")]
        [StringLength(11, ErrorMessage = "| شماره همراه 11 رقمی می باشد ")]
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }
        [DisplayName("جایگاه شغلی(سمت)")]
        public int? PositionID { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("آدرس")]
        public string? Address { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }

    }
}
