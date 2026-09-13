using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class AddRole
    {
        [DisplayName("عنوان نقش")]
        [Required(ErrorMessage = "| الزامی است")]
        public string RoleName { get; set; }
    }
}
