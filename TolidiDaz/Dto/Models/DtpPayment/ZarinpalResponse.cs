using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtpPayment
{
    public class ZarinpalResponse
    {
        public ZarinpalData? Data { get; set; }
        public List<ZarinpalError>? Errors { get; set; } // اصلاح به List<ZarinpalError>
    
    }

    public class ZarinpalData
    {
        public string? Authority { get; set; }
        public int Fee { get; set; }
        public string? Fee_Type { get; set; }
        public int Code { get; set; }
        public string? Message { get; set; }
    }

    public class ZarinpalError
    {
        public string? Message { get; set; }
        public int Code { get; set; }
        public List<string>? Validations { get; set; }
    }

}
