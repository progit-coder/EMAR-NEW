using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class Company1Entity
    {
        public int AuditCompany_Id { get; set; }
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public string Company_Addr1 { get; set; }
        public string Company_Addr2 { get; set; }
        public string Company_Zip { get; set; }
        //public Nullable<int> Company_CityId { get; set; }
        //public Nullable<int> Company_StateId { get; set; }
        //public Nullable<int> Company_CountryId { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Company_Phone { get; set; }
        public string Company_Fax { get; set; }
        public string Company_Email { get; set; }
        public string Com_ContactPerson { get; set; }
        public string Com_ContactPhone { get; set; }
        public string Company_EIN { get; set; }
        public byte[] Company_Logo { get; set; }
        public int Company_Status { get; set; }
        //public Nullable<int> Company_UpdatedBy { get; set; } 
        public string UpdatedBy { get; set; }
        public System.DateTime Company_UpdatedOn { get; set; }



    }
}
