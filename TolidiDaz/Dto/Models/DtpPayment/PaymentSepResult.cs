using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtpPayment
{
    public class PaymentSepResult
    {
        public TransactionDetail? TransactionDetail { get; set; }=new TransactionDetail();
        public int ResultCode { get; set; }
        public string? ResultDescription { get; set; }
        public bool Success { get; set; }
    }

    public class TransactionDetail
    {
        public string? RRN { get; set; }
        public string? RefNum { get; set; }
        public string? MaskedPan { get; set; }
        public string? HashedPan { get; set; }
        public int TerminalNumber { get; set; }
        public int OrginalAmount { get; set; }
        public int AffectiveAmount { get; set; }
        public string? StraceDate { get; set; }
        public string? StraceNo { get; set; }
    }

}
