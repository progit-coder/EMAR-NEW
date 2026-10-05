using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class RoleConfig1Entity
    {
        public int AuditRoleConfig_Id { get; set; }
        public int RoleConfig_Id { get; set; }
        public int Role_Id { get; set; }
        public Nullable<int> Screen_Id { get; set; }
        public Nullable<int> AccessRead { get; set; }
        public Nullable<int> AccessWrite { get; set; }
        public Nullable<int> PrintPdf { get; set; }
        public Nullable<int> PrintExcel { get; set; }
        public int RoleConfig_Status { get; set; }
      //  public Nullable<int> RoleConfig_UpdatedBy { get; set; }
        public System.DateTime RoleConfig_UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
