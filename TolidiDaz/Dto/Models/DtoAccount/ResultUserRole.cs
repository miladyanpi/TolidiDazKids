using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class ResultUserRole
    {

        public string UserID { get; set; }
        public List<ResultRole> ListResultRoles { get; set; }
    }
}
