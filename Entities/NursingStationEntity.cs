using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NursingStationEntity
    {
        public Nullable<int> Facility_Id { get; set; }
        public int NurseStation_Id { get; set; }
        public string NurseStation_Code { get; set; }
        public string NurseStation_Name { get; set; }
        public int NurseStation_Status { get; set; }
        public Nullable<int> NurseStation_CreatedBy { get; set; }
        public System.DateTime NurseStation_CreatedDate { get; set; }
        public string UserName { get; set; }
        public string FacilityName { get; set; }
        public string NurseStationShifts { get; set; }
        public int FacilityStatus { get; set; }
        public List<NurseShiftEntity> NurseStationShiftList{ get; set;}
        public string NurseStationName { get; set; }
        public Nullable<int> DefaultPhysician_Id { get; set; }
        public string PhysicianName { get; set; }
        public int PhysicianStatus { get; set; }
        public int CompanyHlsevenFlag { get; set; }
        public string PhysicianNPI { get; set; }
        public int Biometric_Status { get; set; }
        public int Default_Pharmacy { get; set; }
        public string Backup_Pharmacy { get; set; }

    }
    public class NurseShiftEntity
    {
        public int NurseShifts_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string NurseShifts_Name { get; set; }
        public Nullable<int> Fromtime_hoursId { get; set; }
        public Nullable<int> Totime_hoursId { get; set; }
        public Nullable<int> NurseShifts_Status { get; set; }
        public Nullable<int> NurseShifts_CreatedBy { get; set; }
        public Nullable<System.DateTime> NurseShifts_CreatedOn { get; set; }
        public string FromHours { get; set; }
        public string ToHours { get; set; }
        public string FromTimeFormat { get; set; }
        public string ToTimeFormat { get; set; }

    }
    public class NurseShiftsEntity
    {
        public int NurseShifts_Id { get; set; }
        public string NurseShifts_Name { get; set; }
    }
    public class ProcessKeyEntity
    {
        public int ProcessID { get; set; }
        public int NurseStationId { get; set; }
        public string ComputerName { get; set; }
        public int ProcessID_CreatedBy { get; set; }
    }
    public class ProcessMasterEntity
    {
        public int ProcessID { get; set; }
        public int? NurseStationId { get; set; }
        public string ComputerName { get; set; }
        public string NurseStationName { get; set; }
        public string ProcessKey { get; set; }
    }
}
