using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class ResetPasswordAccount
    {
        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string UserName { get; set; }
    }
}
