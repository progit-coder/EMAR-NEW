using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class PrcReportsSetUpConfigData_ResultEntity
    {
        public string CompanyName { get; set; }
        public string HL7_Configure { get; set; }
        public string HL7DirectionalWay { get; set; }
        public string FteCategory_Desc { get; set; }
        public string Events { get; set; }
    }
    public class PrcReportSetUpConfigGridEntity
    {
        public List<PrcReportsSetUpConfigData_ResultEntity> GridData { get; set; }
        public int TotalRecordsCount { get; set; }
    }
}
