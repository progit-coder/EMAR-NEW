using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OnLeaveEntity
    {
        public int Onleave_Id { get; set; }
        public int IsOnLeave { get; set; }
        public Nullable<long> PVisit_Id { get; set; }
        public Nullable<System.DateTime> LeaveFrom { get; set; }
        public Nullable<System.DateTime> LeaveTo { get; set; }
        public string Reason { get; set; }
        public Nullable<int> OnLeave_Status { get; set; }
        public Nullable<int> OnLeave_CreatedBy { get; set; }
        public System.DateTime OnLeave_CreatedOn { get; set; }

    }
}
