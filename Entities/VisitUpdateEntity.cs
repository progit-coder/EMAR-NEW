using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class VisitUpdateEntity
    {
        public int Patient_Id { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public Nullable<int> Wing { get; set; }
        public string Floor { get; set; }
    }
}
