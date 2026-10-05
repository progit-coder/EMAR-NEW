using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserCustomEntity
    {
        public int User_Id { get; set; }
        public string User_Suffix { get; set; }
        public string UserName { get; set; }
        public Nullable<int> User_Gender { get; set; }
        public Nullable<int> User_MaritalStatus { get; set; }
        public string User_DisplayName { get; set; }
        public string User_Email { get; set; }
        public string User_Phone { get; set; }
        public int User_Lock { get; set; }
        public string UserFullName { get; set; }
        public string UserGender_Desc { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<int?> NurseStation_Id { get; set; }
        public List<int> Facility_Id { get; set; }
        public int DefaultScreenId { get; set; }
    }
    public class UserDetailsCustomEntity
    {
        public int User_Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    }
