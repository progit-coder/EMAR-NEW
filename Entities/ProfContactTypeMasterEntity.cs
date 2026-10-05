using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ProfContactTypeMasterEntity
    {
        public int ProfContact_Id { get; set; }
        public string ProfContact_Desc { get; set; }
        public int ProfContact_Status { get; set; }
        public int ProfContact_CreatedBy { get; set; }
        public System.DateTime ProfContact_CreatedDate { get; set; }
    }
}
