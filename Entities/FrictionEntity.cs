using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
  public class FrictionEntity
    {
        public int Friction_Id { get; set; }
        public string Friction_Type { get; set; }
        public string Friction_Desc { get; set; }
        public int Friction_Status { get; set; }
        public int Friction_CreatedBy { get; set; }
        public System.DateTime Friction_CreatedDate { get; set; }
    }
}
