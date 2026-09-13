using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.DtpPayment
{

    public class AddPayment
    {
        public string? merchant_id { get; set; }
        public Int64 amount { get; set; }
        public string? currency { get; set; } = "IRT";
        public string? description { get; set; }
        public string? callback_url { get; set; }
        public PaymetMetaData? metadata { get; set; }
        public string? mobile { get; set; }
        public string? email { get; set; }
        public string? order_id { get; set; }

    }
}
