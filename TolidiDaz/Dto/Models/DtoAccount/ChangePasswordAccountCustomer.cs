using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class ChangePasswordAccountCustomer
    {

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? UserName { get; set; }

        [DisplayName("رمز عبور جدید")]
        [Required(ErrorMessage = "| الزامی است")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DisplayName("تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "| الزامی است")]
        [DataType(DataType.Password)]
        [Compare(otherProperty: nameof(NewPassword), ErrorMessage = "دو رمز وارد شده مطابقت ندارد")]
        public string? ConfirmNewPassword { get; set; }
      
    }
}
