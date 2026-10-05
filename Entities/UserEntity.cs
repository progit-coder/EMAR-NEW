using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserEntity
    {
        public int User_Id { get; set; }
        public string User_Suffix { get; set; }
        public string User_Fname { get; set; }
        public string User_Lname { get; set; }
        public string User_Mname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> User_Gender { get; set; }
        public Nullable<int> User_MaritalStatus { get; set; }
        public string User_DisplayName { get; set; }
        public string User_Email { get; set; }
        public string User_Phone { get; set; }
        public int User_PwdCount { get; set; }
        public int User_Lock { get; set; }
        public int User_Status { get; set; }
        public Nullable<int> User_CreatedBy { get; set; }
        public System.DateTime User_CreatedDate { get; set; }
        public string UserFullName { get; set; }
        public string UserGender_Desc { get; set; }
       public string CreatedBy_UserName { get;set; }
        public string User_Token { get; set; }
        public Nullable<int> StkReportReq { get; set; }
        public Nullable<int> NewUserFlag { get; set; }
        public int RoleId { get; set; }
        public string Facilities { get; set; }
        public string NurseStations { get; set; }
        public Nullable<int> ProcessKey { get; set; }
        public string ComputerName { get; set; }
        public List<UserFacilityNurseStationEntity> UserFacilityNurseList { get; set; }
        public string PhysicianNPI { get; set; }
        public Nullable<int> PastDueAlertFlag { get; set; }

    }
    public class UserDropEntity
    {
        public int User_Id { get; set; }
        public string User_DisplayName { get; set; }
    }
    public class UserFacilityNurseStationEntity
    {
       public int FacilityId { get; set; }
       public string FacilityName { get; set; }
       public string NurseStatioNames { get; set; }
       public string NurseStationIds { get; set; }
    }
}
