using LTCPro.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace LTCPro.Entities
{
    public class CompanyCustomEntity
    {
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public string Company_Addr1 { get; set; }
        public string Company_Addr2 { get; set; }
        public string Company_Zip { get; set; }
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
        public Nullable<int> Company_CreatedBy { get; set; }
        public System.DateTime Company_CreatedDate { get; set; }
        public string Country_Code { get; set; }
        public string UserName { get; set; }
        public string Company_UniqueId { get; set; }
        public int ApprovalFlag { get; set; }
        public int Fingersdesc_Id { get; set; }
        public string FingerDesc { get; set; }
        public Nullable<int> TimeFormat { get; set; }
        public string Company_City { get; set; }
        public string Company_State { get; set; }
        public Int32 PatientCount { get; set; }
    }
    public class CompanyDropEntity
    {
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
    }
    public class CompanyUIDDropEntity
    {
        public string UIDValue { get; set; }
        public string UIDText { get; set; }
    }
    public class CompanyFlagsEntity
    {
        public Nullable<int> ApprovalFlag { get; set; }
        public Nullable<int> Fingersdesc_Id { get; set; }
        public Nullable<int> TimeFormat { get; set; }
        public Nullable<int> StockReport_Id { get; set; }
        public Nullable<int> DrFirstRequired { get; set; }
        public Nullable<int> Physician_Id { get; set; }
        public string PhysicianNPI { get; set; }

    }
}
