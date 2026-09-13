using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class ResultRole
    {
         public string? ID { get; set; }
        [DisplayName("نام نقش")]
        public string? RoleName { get; set; }
        [DisplayName("عنوان نقش")]

        public string? RoleTitle { get; set; }

    }
}
