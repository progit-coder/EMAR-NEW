using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FacilityCustomEntity
    {
        public int Facility_Id { get; set; }
        public int Company_Id { get; set; }
        public string Facility_Name { get; set; }
        public string Facility_Addr1 { get; set; }
        public string Facility_Addr2 { get; set; }
        public string Facility_Zip { get; set; }
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
        public string Company_Name { get; set; }
        public int Company_Status { get; set; }
        public string Country_Code { get; set; }
        public string UserName { get; set; }
        public string Facility_City { get; set; }
        public string Facility_State { get; set; }
        public string Company_UniqueId { get; set; }
        public int CompanyHlsevenFlag { get; set; }
        public Int32 PatientCount { get; set; }
        public Nullable<int> TZ_Id { get; set; }
        public string TimeZone { get; set; }
    }
    public class FacilityDropEntity
    {
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
    }

    public class NurseStationDropEntity
    {
        public int NurseStation_Id { get; set; }
        public string NurseStation_Code { get; set; }
        public string NurseStation_Name { get; set; }
        public int NurseStation_Status { get; set; }
        public Int32 PatientCount { get; set; }
        public int? NewUserFlag { get; set; }
    }
    public class FacilitiesbyCompanyIdEntity
    {
        public int Facility_Id { get; set; }
        public string Facility_Name { get; set; }
    }
    public class FloorDropEntity
    {
        public int Floor_Id { get; set; }
        public string Floor_Name { get; set; }
    }
    public class WingDropEntity
    {
        public int Wing_Id { get; set; }
        public string Wing_Desc { get; set; }
    }
    public class RoomDropEntity
    {
        public int Room_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Code { get; set; }
        public string Room { get; set; }
    }
    public class BedDropEntity
    {
        public int Bed_Id { get; set; }
        public string Bed_Name { get; set; }
        public string Bed_Code { get; set; }
        public string Bed { get; set; }
    }
    public class CensusEntity
    {
        public string NursingStation { get; set; }
        public int CensusTypeId { get; set; }
        public string CensusType { get; set; }
        public int CensusCount { get; set; }
    }
}
