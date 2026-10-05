using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class StockReportEntity
    {
        public int StockReport_Id { get; set; }
        public string StockReportFor { get; set; }
        public Nullable<int> StockReport_Status { get; set; }
        public Nullable<int> StockReport_CreatedBy { get; set; }
        public System.DateTime StockReport_CreatedOn { get; set; }
    }
}
