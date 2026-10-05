using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class MobilityEntity
    {
        public int Mobility_Id { get; set; }
        public string Mobility_Type { get; set; }
        public string Mobility_Desc { get; set; }
        public int Mobility_Status { get; set; }
        public int Mobility_CreatedBy { get; set; }
        public System.DateTime Mobility_CreatedDate { get; set; }
    }
}
