using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AcknowledgeOrdersCustomEntity
    {
        public int POrder_Id { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string PatientName { get; set; }
        public string DrugName { get; set; }
        public string OrderType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public int ApprovalStatus { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
    }
}
