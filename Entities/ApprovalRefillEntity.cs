using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ApprovalRefillEntity
    {
        public long Refill_Id { get; set; }
        public Nullable<int> Porder_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string NumberOfRefillsRemaining { get; set; }
        public Nullable<int> Refill_Status { get; set; }
        public Nullable<int> Refill_CreatedBy { get; set; }
        public Nullable<System.DateTime> Refill_CreatedDate { get; set; }
        public Nullable<int> POOutBoundFileStatus { get; set; }
        public Nullable<int> POOutBoundApproval { get; set; }
        public Nullable<int> POOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> POOutBoundApprovalOn { get; set; }
        public Nullable<int> File_Id { get; set; }
    }
}
