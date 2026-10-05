using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrdersTypeCountEntity
    {
        public int Active { get; set; }
        public int Discontinue { get; set; }
        public int Refill { get; set; }
        public int Destroy { get; set; }
        public int Hold { get; set; }
        public int Update { get; set; }
    }
}
