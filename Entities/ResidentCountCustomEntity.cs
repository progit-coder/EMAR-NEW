using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public class ResidentCountCustomEntity
    {
        public int Admit { get; set; }
        public int Readmit { get; set; }
        public int Inactive { get; set; }
        public int Transfer { get; set; }
        public int Discharge { get; set; }

    }
}
