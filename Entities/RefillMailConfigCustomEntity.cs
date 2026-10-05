using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class RefillMailConfigCustomEntity
    {
        public int Rdc_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public string NurseStation_Id { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string TimeIDs { get; set; }
    }
    public class RefillMailConfigGridData
    {
        public Nullable<int> Facility_Id { get; set; }
        public string NurseStation_Id { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string FacilityName { get; set; }
        public string NursingStationNames { get; set; }
        public string TimeIds { get; set; }
        public string TimeText { get; set; }
    }
}
