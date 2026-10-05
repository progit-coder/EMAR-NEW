using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public  class InboundDashboardCustomEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ResidentName { get; set; }
        public int Status { get; set;}
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
    public class OutboundDashboardCustomEntity
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }
}
