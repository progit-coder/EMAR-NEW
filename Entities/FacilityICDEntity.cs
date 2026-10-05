using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FacilityICDEntity
    {
        public int FacilityICD_Id { get; set; }
        public int Facility_Id { get; set; }
        public Nullable<int> ICDMonths { get; set; }
        public Nullable<System.DateTime> ICDDate { get; set; }
        public int FacilityICD_CreatedBy { get; set; }
        public System.DateTime FacilityICD_CreatedDate { get; set; }
    }
}
