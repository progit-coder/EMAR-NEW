using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MedicationReasonEntity
    {
        public int MedicationReason_ID { get; set; }
        public string MedicationReason_Desc { get; set; }
        public int MedicationReason_Status { get; set; }
        public Nullable<int> MedicationReason_CreatedBy { get; set; }
        public System.DateTime MedicationReason_CreatedOn { get; set; }
    }
}
