using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DrFirstXMLTransEntity
    {
        public int PrescriptionID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public partial class  DrFirstEntity
    {
        public int type { get; set; }
        public string Prescription { get; set; }
        public int ApprovalBy { get; set; }
    }
 
}
