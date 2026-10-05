using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class ContactTypeEntity
    {
        public int ContactType_Id { get; set; }
        public string ContactType_Desc { get; set; }
        public int ContactType_Status { get; set; }
        public int ContactType_CreatedBy { get; set; }
        public System.DateTime ContactType_CreatedDate { get; set; }
    }
}
