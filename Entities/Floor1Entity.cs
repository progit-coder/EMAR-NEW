using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class Floor1Entity
    {
        public int AuditFloor_Id { get; set; }
        public int Floor_Id { get; set; }
        public string Floor_Name { get; set; }
        public int Floor_Status { get; set; }
       // public Nullable<int> Floor_UpdatedBy { get; set; }
        public System.DateTime Floor_UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

    }
}
