using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class AdvancedDirectiveEntity
    {
        public int AdvDirectives_Id { get; set; }
        public string AdvDirectives_Desc { get; set; }
        public int AdvDirectives_Status { get; set; }
        public int AdvDirectives_CreatedBy { get; set; }
        public System.DateTime AdvDirectives_CreatedDate { get; set; }

    }
}
