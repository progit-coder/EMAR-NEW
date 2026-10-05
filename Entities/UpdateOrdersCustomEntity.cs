using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class UpdateOrdersCustomEntity
    {
        public int PorderId { get; set; }
        public int PQuantityId { get; set; }
        public int PatientId { get; set; }
        public int HoldChangeFlag { get; set; }
        public int HoldStatus { get; set; }
        public int DcChangeFlag { get; set; }
        public int DcStatus { get; set; }
        public int DcsPlit { get; set; }
        public int DcAllSplits { get; set; }
        public int UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
    public class OrdersHoldDCCustomEntity
    {
        public Nullable<System.DateTime> HoldFrom { get; set; }
        public Nullable<System.DateTime> HoldTo { get; set; }
        public string HoldReason { get; set; }
        public string DcReason { get; set; }
    }
    public class MultipleOrdersCustomEntity
    {
        public List<UpdateOrdersCustomEntity> records { get; set; }
        public OrdersHoldDCCustomEntity HoldDC { get; set; }
    }
}
