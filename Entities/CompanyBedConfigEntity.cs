using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CompanyBedConfigEntity
    {
        public int BedConfig_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> Floor_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public Nullable<int> Wing_Id { get; set; }
        public Nullable<int> Room_Id { get; set; }
        public Nullable<int> Bed_Id { get; set; }
        public int BedConfig_Status { get; set; }
        public Nullable<int> BedConfig_CreatedBy { get; set; }
        public System.DateTime BedConfig_CreatedDate { get; set; }

        public string Company_Name { get; set; }
        public string Facility_Name { get; set; }
        public string Floor_Name { get; set; }
        public string NurseStation_Name { get; set; }
        public string Wing_Name { get; set; }
        public string Room_Name { get; set; }
        public string Bed_Name { get; set; }
        public string UserName { get; set; }
        public string NurseStations { get; set; }
    }
}
