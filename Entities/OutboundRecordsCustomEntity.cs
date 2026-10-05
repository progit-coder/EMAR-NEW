using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OutboundRecordsCustomEntity
    {
        public int Patient_Id { get; set; }
        public string Patient_Name { get; set; }
        public int RecordId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public int UploadedBy { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}
