using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FlotChartEntity
    {
        public int count { get; set; }
        public DateTime date { get; set; }
        public TimeSpan lastAccess { get; set; }
    }
}
