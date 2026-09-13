using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Dto.Models.DtoAccount
{
    public class LoginAccount
    {
        public LoginAccount()
        {
            SecurityStamp=Guid.NewGuid().ToString();
        }
        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? UserName { get; set; }

        [DisplayName("رمزعبور")]
        [Required(ErrorMessage = "| الزامی است")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DisplayName("مرا خارج نکن")]
        public bool RememberMe { get; set; }
        public string? RoleName { get; set; } = null;

        public string? SecurityStamp { get; set; }


    }
}
