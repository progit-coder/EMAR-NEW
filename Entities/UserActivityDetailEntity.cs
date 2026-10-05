using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserActivityDetailEntity
    {
        public long UserActivity_Id { get; set; }
        public Nullable<long> Session_Id { get; set; }
        public Nullable<int> Screen_Id { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<int> Activity_Id { get; set; }
        public string Comments { get; set; }
    }
}
