using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtpPayment
{
    public class PaymentResponse
    {
        public string? MID { get; set; }
        public string? State { get; set; }
        public int Status { get; set; }
        public string? RRN { get; set; }
        public string? RefNum { get; set; }
        public string? ResNum { get; set; }
        public string? TerminalId { get; set; }
        public string? TraceNo { get; set; }
        public string? Amount { get; set; }
        public string? Wage { get; set; }
        public string? SecurePan { get; set; }
        public string? HashedCardNumber { get; set; }
    }

}
