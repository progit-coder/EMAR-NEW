using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentsEntity
    {
        public int Patient_Id { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientAddress2 { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string PhoneHome { get; set; }
        public byte[] ImageLocation { get; set; }
        public string imgchk { get; set; }
        public int Patient_Status { get; set; }
        public Nullable<int> Patient_CreatedBy { get; set; }
        public System.DateTime Patient_CreatedDate { get; set; }
        public string UserName { get; set; }
        public string NurseStationName { get; set; }
        public string FloorName { get; set; }
        public string WingName { get; set; }
        public string RoomName { get; set; }
        public string BedName { get; set; }
        public string PatientGender { get; set; }
        public string PatientDOB { get; set; }
        public string AliasName { get; set; }
        public string PatientName { get; set; }
        public string ImgPath { get; set; }
        public Nullable<DateTime> DOB { get; set; }
    }
    public class ResidentGridEntity
    {
        public int TotalRecords { get; set; }
        public List<ResidentsEntity> Data { get; set; }
    }
    public class ResidentDropEntity
    {
        public int Patient_Id { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string PatientName { get; set; }
        public int PVisit_Status { get; set; }
    }
    public class ResidentDataEntity
    {        
        public List<ResidentDropEntity> ResidentDrop { get; set; }
        public List<NurseStationDropEntity> NSDrop { get; set; }
        public string FacilityName { get; set; }
        public int NursingStationId { get; set; }

    }
}
