using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FacilityEntity
    {
        public int Facility_Id { get; set; }
        public int Company_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Addr1 { get; set; }
        public string Facility_Addr2 { get; set; }
        public string Facility_Zip { get; set; }
        public Nullable<int> Facility_CityId { get; set; }
        public Nullable<int> Facility_StateId { get; set; }
        public Nullable<int> Facility_CountryId { get; set; }
        public string Facility_Phone { get; set; }
        public string Facility_Fax { get; set; }
        public string Facility_ContactName { get; set; }
        public string Facility_ContactPhone { get; set; }
        public string Facility_ShortName { get; set; }
        public byte[] Facility_Logo { get; set; }
        public int Facility_Status { get; set; }
        public Nullable<int> Facility_CreatedBy { get; set; }
        public System.DateTime Facility_CreatedDate { get; set; }
        public string FacilityName { get; set; }
        public string Facility_City { get; set; }
        public string Facility_State { get; set; }
        public Nullable<int> TZ_Id { get; set; }

        public virtual CityEntity City { get; set; }
        public virtual CompanyEntity Company { get; set; }
        public virtual CountryEntity Country { get; set; }
        public virtual StateEntity State { get; set; }

    }
}
