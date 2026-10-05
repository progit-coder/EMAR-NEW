using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrderHoldEntity
    {
        public int OrderHold_Id { get; set; }
        public int PQuantity_Id { get; set; }
        public Nullable<System.DateTime> HoldFrom { get; set; }
        public Nullable<System.DateTime> HoldTo { get; set; }
        public int OrderHold_Status { get; set; }
        public Nullable<int> OrderHold_CreatedBy { get; set; }
        public Nullable<System.DateTime> OrderHold_CreatedDate { get; set; }
        public string HoldReason { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
}
