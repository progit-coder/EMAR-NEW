using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserRoleFacilityConfigEntity
    {
        public int UserRole_Id { get; set; }
        public int Role_Id { get; set; }
        public int User_Id { get; set; }
        public int Facility_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string User_DisplayName { get; set; }
        public int UserRole_Status { get; set; }
        public Nullable<int> UserRole_CreatedBy { get; set; }
        public System.DateTime UserRole_CreatedDate { get; set; }
        public string UserName { get; set; }
        public string FacilitName { get; set; }
        public string CreatedUser { get; set; }
        public string RoleName { get; set; }
        public string Users { get; set; }
        public string Facilities { get; set; }
        public string NurseStationName { get; set; }
        public string NurseStations { get; set; }

    }
    public class UserRoleConfigIds
    {
        public int Role_Id { get; set; }
        public int User_Id { get; set; }
        public int Facility_Id { get; set; }
    }
    public class UserRoleFacilityConfigCustomEntity
    {
        public int UserRole_Id { get; set; }
        public int Role_Id { get; set; }
        public int User_Id { get; set; }
        public int Facility_Id { get; set; }
        public Nullable<int> UserRole_CreatedBy { get; set; }
        public System.DateTime UserRole_CreatedDate { get; set; }
        public string NurseStations { get; set; }
        public int OldRole_Id { get; set; }
        public int OldUser_Id { get; set; }
        public int OldFacility_Id { get; set; }
    }
  
}
