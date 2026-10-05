using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserRoleFacilityConfigGridEntity
    {
        public int User_Id { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public int User_Status { get; set; }
        public string Company_Facility_Nursestation { get; set; }
        public int User_Lock { get; set; }
        public Nullable<int> Role_Status { get; set; }
    }
}
