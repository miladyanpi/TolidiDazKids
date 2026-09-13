using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models.Report
{
    public class ResultReportDashboard
    {
        public int ProductCounAll { get; set; }
        public int ProductCounAllToday { get; set; }
        public int CustomerCount { get; set; }
        public int OrderCount { get; set; }
        public int OrdorderCountDelivereder { get; set; }
        public Int64 OrderSumFinalPriceToday { get; set; }
        public Int64 OrderSumFinalPriceYesterday { get; set; }
    }
}
