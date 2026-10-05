using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AuditCustomEntity
    {
        public List<string> ColumnNames { get; set; }
        public IList Records { get; set; }
    }
}
