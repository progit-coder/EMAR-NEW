using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class LanguageEntity
    {
        public int Language_Id { get; set; }
        public string Language_Code { get; set; }
        public string Language_Desc { get; set; }
        public int Language_Status { get; set; }
        public int Language_CreatedBy { get; set; }
        public System.DateTime Language_CreatedDate { get; set; }
    }
}
