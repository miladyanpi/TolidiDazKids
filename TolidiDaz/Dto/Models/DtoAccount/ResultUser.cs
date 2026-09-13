using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class ResultUser
    {
        public string ID { get; set; }

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string UserName { get; set; }

        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
