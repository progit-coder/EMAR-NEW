using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ApprovalPendingCustomEntity
    {
        //Approval table Id column
        public long ApprovalId { get; set; }
        //Id column in respected table
        public int Record_Id { get; set; }
        public int Patient_Id { get; set; }
        public string Patient_Name { get; set; }
        //Matching table
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string UserName { get; set; }
        public Nullable<int> DUom { get; set; }
        public Nullable<int> Sig2DUom { get; set; }
        public Nullable<int> Sig3DUom { get; set; }
        public Nullable<int> Sig4DUom { get; set; }
    }
    public class ApprovalChangesCustomEntity
    {
        public string ColumnName { get; set; }
        //Matching table
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
