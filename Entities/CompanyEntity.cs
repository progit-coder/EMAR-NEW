using LTCPro.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace LTCPro.Entities
{
    public class CompanyEntity
    {
        public CompanyEntity()
        {
            //this.Facilities = new HashSet<FacilityEntity>();
            //this.HLSevenCompanyConfigs = new HashSet<HLSevenCompanyConfigEntity>();
            //this.FTEConfigurations = new HashSet<FTEConfigurationEntity>();
            //this.FileInformations = new HashSet<FileInformationEntity>();
        }

        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public string Company_Addr1 { get; set; }
        public string Company_Addr2 { get; set; }
        public string Company_Zip { get; set; }
        public Nullable<int> Company_CityId { get; set; }
        public Nullable<int> Company_StateId { get; set; }
        public Nullable<int> Company_CountryId { get; set; }
        public string Company_Phone { get; set; }
        public string Company_Fax { get; set; }
        public string Company_Email { get; set; }
        public string Com_ContactPerson { get; set; }
        public string Com_ContactPhone { get; set; }
        public string Company_EIN { get; set; }
        public byte[] Company_Logo { get; set; }
        public byte[] Company_FooterLogo { get; set; }
        public int Company_Status { get; set; }
        public string Company_City { get; set; }
        public string Company_State { get; set; }
        //public Nullable<int> ApprovalFlag { get; set; }
        //public Nullable<int> Fingersdesc_Id { get; set; }
        public Nullable<int> Company_CreatedBy { get; set; }
        public System.DateTime Company_CreatedDate { get; set; }
        public string Company_UniqueId { get; set; }
        //public Int64 Session_Id { get; set; }
        //public Nullable<int> TimeFormat { get; set; }
        
        //[JsonIgnore]
        //public virtual ICollection<FacilityEntity> Facilities { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<HLSevenCompanyConfigEntity> HLSevenCompanyConfigs { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<FTEConfigurationEntity> FTEConfigurations { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<FileInformationEntity> FileInformations { get; set; }
        //public virtual UserEntity User { get; set; }

    }
}
