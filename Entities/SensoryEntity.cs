using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class SensoryEntity
    {
        public int Sensory_Id { get; set; }
        public string Sensory_Type { get; set; }
        public string Sensory_Desc { get; set; }
        public int Sensory_Status { get; set; }
        public int Sensory_CreatedBy { get; set; }
        public System.DateTime Sensory_CreatedDate { get; set; }

    }
}
