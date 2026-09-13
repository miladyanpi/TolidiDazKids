using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoContactUs
{
    public class AddContactUs:BaseModel
    {
        [DisplayName("نام کامل")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? FullName { get; set; }
        [DisplayName("ایمیل")]
        public string? Email { get; set; }
        [DisplayName("موبایل")]
        [Required(ErrorMessage = "| الزامی است")]
        [RegularExpression(@"^0([0-9]{10})$", ErrorMessage = "| شماره همراه را با فرمت درست وارد نمایید")]
        [StringLength(11, ErrorMessage = "| شماره همراه 11 رقمی می باشد ")]
        public string? Mobile { get; set; }
        [DisplayName("موضوع")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Subject { get; set; }
        [DisplayName("پیام شما")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Message { get; set; }

        [DisplayName("آی پی")]
        public string? IpAddress { get; set; }
        [DisplayName("وضعیت پیام")]
        public bool IsRead { get; set; }
        [DisplayName("کد امنیتی")]
        [Required(ErrorMessage = "| الزامی است")]
        [Compare(nameof(CaptchaText1), ErrorMessage = "کد امنیتی را درست وارد کنید")]
        public string? EnteredCaptchaText1 { set; get; }

        public string? CaptchaText1 { set; get; }

    }
}
