using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class Room1Entity
    {
        public int AuditRoom_Id { get; set; }
        public int Room_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Code { get; set; }
        public int Room_Status { get; set; }
       // public Nullable<int> Room_UpdatedBy { get; set; }
        public System.DateTime Room_UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
