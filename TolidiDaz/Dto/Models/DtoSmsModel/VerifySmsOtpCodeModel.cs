using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtoSmsModel
{
    public class VerifySendSmsOtpCode
    {
        [RegularExpression(@"^0([0-9]{10})$", ErrorMessage = "| شماره همراه را با فرمت درست وارد نمایید")]
        [StringLength(11, ErrorMessage = "| شماره همراه 11 رقمی می باشد ")]
        [DisplayName("موبایل")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "| الزامی است")]
        public string? Code { get; set; }
        public bool IsFlash { get; set; } = false;
        public long[]? RecId { get; set; } = null;
        public byte[]? Status { get; set; } = null;
    }
}
