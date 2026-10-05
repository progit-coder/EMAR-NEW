using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class PrcReportGetStockData_ResultEntity
    {
        public string NurseStationName { get; set; }
        public string DrugName { get; set; }
        public Nullable<decimal> QuantityInHand { get; set; }
        public Nullable<decimal> QuantityRemaining { get; set; }
        public Nullable<decimal> QuantityUsed { get; set; }
        public string Company_Name { get; set; }
        public string Facility_Name { get; set; }
        public int status { get; set; }
    }
    public class PrcReportGetStockDataGridEntity
    {
        public List<PrcReportGetStockData_ResultEntity> GridData { get; set; }
        public int TotalRecordsCount { get; set; }
    }
}
