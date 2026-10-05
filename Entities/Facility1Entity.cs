using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class Facility1Entity
    {
        public int AuditFacility_id { get; set; }
        public int Facility_id { get; set; }
        public int Company_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Addr1 { get; set; }
        public string Facility_Addr2 { get; set; }
        public string Facility_Zip { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        //public Nullable<int> Facility_CityId { get; set; }
        //public Nullable<int> Facility_StateId { get; set; }
        //public Nullable<int> Facility_CountryId { get; set; }
        public string Facility_Phone { get; set; }
        public string Facility_Fax { get; set; }
        public string Facility_ContactName { get; set; }
        public string Facility_ContactPhone { get; set; }
        public string Facility_ShortName { get; set; }
        public byte[] Facility_Logo { get; set; }
        public int Facility_Status { get; set; }
        //  public Nullable<int> Facility_UpdatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime Facility_UpdatedOn { get; set; }

    }
}
