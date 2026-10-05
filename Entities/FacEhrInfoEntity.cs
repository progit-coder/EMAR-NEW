using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class FacEhrInfoEntity
    {
        public int FacEhrInfo_Id { get; set; }
        public int Facility_Id { get; set; }
        public string PINRequired { get; set; }
        public string ScanProgram { get; set; }
        public Nullable<int> RecordLocking { get; set; }
        public int FacEhrInfo_CreatedBy { get; set; }
        public System.DateTime FacEhrInfo_CreatedDate { get; set; }
    }
}
