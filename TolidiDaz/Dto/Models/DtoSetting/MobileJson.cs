using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtoSetting
{
    public class MobileJson
    {
        public Guid Id { get; set; }
        [DisplayName("شماره موبایل")]
        //[RegularExpression(@"^0([0-9]{10})$", ErrorMessage = "| شماره همراه را با فرمت درست وارد نمایید")]
        //[StringLength(11, ErrorMessage = "| شماره همراه 11 رقمی می باشد ")]
        public string? Mobile { get; set; }
    }
}
