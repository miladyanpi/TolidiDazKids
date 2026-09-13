using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dto.Models.DtpPayment
{
    public class ResponseTokenSep
    {
        public string? token { get; set; }
        public int status { get; set; }

        public string? errorCode { get; set; }

        public string? errorDesc { get; set; }
    }
    
}
