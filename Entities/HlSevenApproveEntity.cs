using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
     public class HlSevenApproveEntity
    {
         public int ApprovalStatus { get; set; }
         public int ApprovedBy { get; set; }
         public Nullable<System.DateTime> ApprovedDate { get; set; }
         public int POrderId { get; set; }
         public int DAdminId { get; set; }
    }
}
