using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class AddUser
    {

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Display(Name ="ایمیل")]
        public string? Email { get; set; }
        [DisplayName("موبایل")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Mobile { get; set; }
        [DisplayName("رمزعبور")]
        [Required(ErrorMessage = "| الزامی است")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DisplayName("تکرار رمزعبور")]
        [Required(ErrorMessage = "| الزامی است")]
        [DataType(DataType.Password)]
        [Compare(otherProperty:nameof(Password), ErrorMessage ="دو رمز وارد شده مطابقت ندارد")]
        public string? ConfirmPassword { get; set; }
        public string? RoleName { get; set; }

        public int? CustomerID { get; set; }
        [DisplayName("نقش")]
        public string? roleID { get; set; }
    }
}
