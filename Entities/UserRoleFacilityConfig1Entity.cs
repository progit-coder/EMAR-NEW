using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public  class UserRoleFacilityConfig1Entity
    {
        public int AuditUserRole_Id { get; set; }
        public int UserRole_Id { get; set; }
        public int Role_ID { get; set; }
        public int User_Id { get; set; }
        public int Facility_id { get; set; }
        public int UserRole_Status { get; set; }
       // public Nullable<int> UserRole_UpdatedBy { get; set; }
        public System.DateTime UserRole_UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
