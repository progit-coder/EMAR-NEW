using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class RoomEntity
    {
        public int Room_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Code { get; set; }
        public int Room_Status { get; set; }
        public Nullable<int> Room_CreatedBy { get; set; }
        public System.DateTime Room_CreatedDate { get; set; }
        public virtual UserEntity User { get; set; }
        public Int32 PatientCount { get; set; }
    }
}
