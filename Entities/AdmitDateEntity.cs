using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public partial class AdmitDateEntity
    {
        public Int64 PVisit_Id { get; set; }
        public Nullable<System.DateTime> AdmitDate { get; set; }
        public int Patient_Id { get; set; }
    }
}
