using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
  public class ResidentEntity
    {
        public long ResidentId { get; set; }
        public Nullable<int> Status { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public Nullable<int> Prefix { get; set; }
        public Nullable<int> Suffix { get; set; }
        public string PIC { get; set; }
        public string MaidenName { get; set; }
        public string MotherMaidenName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public Nullable<int> Sex { get; set; }
        public Nullable<int> Citizenship { get; set; }
        public Nullable<int> EthnicOrigin { get; set; }
        public Nullable<int> Language { get; set; }
        public string ReligousPref { get; set; }
        public Nullable<int> MaritalStatus { get; set; }
        public string County { get; set; }
        public string SSN { get; set; }
        public string MedicareNum { get; set; }
        public string MedicaidNum { get; set; }
        public string Occupation { get; set; }
        public Nullable<int> MilitaryService { get; set; }
        public Nullable<System.DateTime> DateDisch { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public Nullable<int> PPS { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string PPSStartDtSame { get; set; }
        public Nullable<System.DateTime> PPSStartDate { get; set; }
        public Nullable<int> WaitlistedForBed { get; set; }
        public string PictureLocation { get; set; }
        public Nullable<System.DateTime> MDSAdmitDate { get; set; }
        public Nullable<System.DateTime> NextAssmtDueDate { get; set; }
        public string NextAssmtDueType { get; set; }
        public string Nickname { get; set; }
        public string MBI { get; set; }

        //Payor
        public int ResParty_Id { get; set; }
        //public int ResResParty_Status { get; set; }
        //Advance Directive
        public int AdvDirectives_Id { get; set; }
        //public int ResAdvDirectives_Status { get; set; }
        //Race
        public int Race_Id { get; set; }
        //public int Resrace_Status { get; set; }


    }
    public class ResidentResPartyEntity
    {
        public long ResidentId { get; set; }
        public int ResParty_Id { get; set; }
        public int ResResParty_Status { get; set; }
    }

    public class UpdatePregnecyFeedingEntity
    {
        public long PatientID { get; set; }
        public int Type { get; set; }

        public Boolean IScheck { get; set; }
    }
    public class ResidentAdvanceDirEntity
    {
        public long ResidentId { get; set; }
        public int AdvDirectives_Id { get; set; }
        public int ResAdvDirectives_Status { get; set; }
    }
    public class ResidentRaceEntity
    {
        public long ResidentId { get; set; }
        public int Race_Id { get; set; }
        public int Resrace_Status { get; set; }
    }
}
