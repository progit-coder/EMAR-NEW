using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class PrcGetFreqMappingData_ResultEntity
    {
        public int NursingFreq_Id { get; set; }
        public string FacilityName { get; set; }
        public string NurseStationName { get; set; }
        public string Frequency { get; set; }
        public string Start_Time { get; set; }
        public string Hours { get; set; }
        public int Facility_Status { get; set; }
        public int NurseStation_Status { get; set; }
    }
}
