using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class DefaultScreenEntity
    {
        public int DefaultScreen_Id { get; set; }
        public Nullable<int> Screen_Id { get; set; }
        public Nullable<int> DefaultScreen_Status { get; set; }
        public Nullable<int> DefaultScreen_CreatedBy { get; set; }
        public Nullable<System.DateTime> DefaultScreen_CreatedOn { get; set; }
        public string Screen_Desc { get; set; }
    }
}
