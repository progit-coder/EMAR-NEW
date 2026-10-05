using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MedicaidCheckEntity
    {
        public int MedicaidCheck_Id { get; set; }
        public string MedicaidCheck_Desc { get; set; }
        public int MedicaidCheck_Status { get; set; }
        public int MedicaidCheck_CreatedBy { get; set; }
        public System.DateTime MedicaidCheck_CreatedDate { get; set; }
    }
}
