using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentAdmitDischargeEntity
    {
        public long PVisit_Id { get; set; }
        public int Patient_Id { get; set; }
        public DateTime AdmitDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public int PVisit_Status { get; set; }
        public int NoofDays { get; set; }
    }
}
