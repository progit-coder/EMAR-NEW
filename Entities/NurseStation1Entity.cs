using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NurseStation1Entity
    {
        public int AuditNurseStation_Id { get; set; }
        public int NurseStation_Id { get; set; }
        public string NurseStation_Code { get; set; }
        public string NurseStation_Name { get; set; }
        public int NurseStation_Status { get; set; }
       // public Nullable<int> NurseStation_UpdatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime NurseStation_UpdatedOn { get; set; }
        
    }
}
