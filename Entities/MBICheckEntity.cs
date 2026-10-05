using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class MBICheckEntity
    {
        public int MBICheck_Id { get; set; }
        public string MBICheck_Desc { get; set; }
        public int MBICheck_Status { get; set; }
        public int MBICheck_CreatedBy { get; set; }
        public System.DateTime MBICheck_CreatedDate { get; set; }
    }
}
