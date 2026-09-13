using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dto.Models.DtpPayment
{


    public class VerifyPaymentResponse
    {
        public int? id { get; set; }

        public string? status { get; set; }

        public int? statusCode { get; set; }

        public VerifyPayment? entity { get; set; }

        public string? message { get; set; }

        public string? token { get; set; }

        public int count { get; set; }
    }

    public class VerifyPayment
    {
        public DataModel? data { get; set; }

        public object? errors { get; set; }  // چون گاهی [] است، گاهی ممکنه {}

    }

    public class DataModel
    {
        public List<object> wages { get; set; } = new List<object>();
        public int code { get; set; }
        public string? message { get; set; }
        public string? card_hash { get; set; }
        public string? card_pan { get; set; }
        public long ref_id { get; set; }
        public string? fee_type { get; set; }
        public Int64 fee { get; set; }
        public string? shaparak_fee { get; set; }
        public string? order_id { get; set; }
    }
}
    
