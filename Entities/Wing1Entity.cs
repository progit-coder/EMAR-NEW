using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class Wing1Entity
    {
        public int AuditWing_Id { get; set; }
        public int Wing_Id { get; set; }
        public string Wing_Desc { get; set; }
        public int Wing_Status { get; set; }
       // public int Wing_UpdatedBy { get; set; }
        public System.DateTime Wing_Updatedon { get; set; }
        public string UpdatedBy { get; set; }

    }
}
