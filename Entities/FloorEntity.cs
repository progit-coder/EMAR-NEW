using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FloorEntity
    {
        public int Floor_Id { get; set; }
        public string Floor_Name { get; set; }
        public int Floor_Status { get; set; }
        public Nullable<int> Floor_CreatedBy { get; set; }
        public System.DateTime Floor_CreatedDate { get; set; }
        public virtual UserEntity User { get; set; }
        public Int32 PatientCount { get; set; }
    }
}
