using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dto.Enum.EnumConstant;

namespace Dto.Models.DtoCustomer
{
    public class ResultCustomerUserInfo
    {
       
        [DisplayName("نام کاربری")]
        public string? UserName { get; set; }

        [DisplayName("پست الکترونیکی")]
        public string? Email { get; set; }
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }


    }
}
