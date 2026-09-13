using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Dto.Models.DtoAccount
{
    public class UpdateUser
    {
        public string ID { get; set; }

        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string UserName { get; set; }
        public string OldUserName { get; set; }

        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }


    }
}
