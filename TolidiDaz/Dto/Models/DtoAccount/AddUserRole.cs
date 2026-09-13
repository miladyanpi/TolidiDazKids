using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dto.Models.DtoAccount
{
    public class AddUserRole
    {

        public int CusomerID { get; set; }
        //public string UserID { get; set; }
        public List<ResultRole> ListResultRoles { get; set; }
    }
}
