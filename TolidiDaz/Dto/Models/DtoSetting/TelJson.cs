using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtoSetting
{
    public class TelJson
    {
        public Guid Id { get; set; }
        [DisplayName("شماره ثابت")]
        //[RegularExpression(@"^0[0-9]{9,12}$", ErrorMessage = " | تلفن ثابت را با فرمت درست وارد نمایید")]
        public string? Tel { get; set; }
    }
}
