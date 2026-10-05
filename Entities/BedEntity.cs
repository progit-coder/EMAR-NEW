using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class BedEntity
    {
        public int Bed_Id { get; set; }
        public string Bed_Name { get; set; }
        public string Bed_Code { get; set; }
        public int Bed_Status { get; set; }
        public Nullable<int> Bed_CreatedBy { get; set; }
        public System.DateTime Bed_CreatedDate { get; set; }
        public virtual UserEntity User { get; set; }
        public Int32 PatientCount { get; set; }
    }
}
