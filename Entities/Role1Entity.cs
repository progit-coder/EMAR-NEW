using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class Role1Entity
    {
        public int AuditRole_Id { get; set; }
        public int Role_Id { get; set; }
        public string Role_Desc { get; set; }
        public int Role_Status { get; set; }
        //public Nullable<int> Role_UpdatedBy { get; set; }
        public System.DateTime Role_UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
