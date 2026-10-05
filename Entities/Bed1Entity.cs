using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class Bed1Entity
    {
        public int AuditBed_Id { get; set; }
        public int Bed_Id { get; set; }
        public string Bed_Name { get; set; }
        public string Bed_Code { get; set; }
        public int Bed_Status { get; set; }
       // public Nullable<int> Bed_UpdatedBy { get; set; }
        public System.DateTime Bed_UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
