using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class MilitaryMasterEntity
    {
        public int Military_Id { get; set; }
        public string Military_Code { get; set; }
        public string Military_Desc { get; set; }
        public int Military_Status { get; set; }
        public Nullable<int> Military_CreatedBy { get; set; }
        public System.DateTime Military_CreatedDate { get; set; }
    }
}
