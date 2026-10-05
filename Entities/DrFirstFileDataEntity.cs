using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class DrFirstFileDataEntity
    {
        public long DrFirstFile_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public string PatientMRNumber { get; set; }
        public string FilePath { get; set; }
        public Nullable<System.DateTime> ReceivedOn { get; set; }
        public string CompanyName { get; set; }
        public string PatientName { get; set; }

        public virtual Company Company { get; set; }
    }
}
