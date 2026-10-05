using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class SessionTypeEntity
    {
        public int SessionType_id { get; set; }
        public Nullable<int> SessionType_Type { get; set; }
        public string SessionType_Desc { get; set; }
    }
}
