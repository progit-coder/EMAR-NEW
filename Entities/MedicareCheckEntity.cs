using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class MedicareCheckEntity
    {
        public int MedicareCheck_Id { get; set; }
        public string MedicareCheck_Desc { get; set; }
        public int MedicareCheck_Status { get; set; }
        public int MedicareCheck_CreatedBy { get; set; }
        public System.DateTime MedicareCheck_CreatedDate { get; set; }
    }
}
