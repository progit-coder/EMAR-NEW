using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class UserActivityReportEntity
    {
        public string Screen_Name { get; set; }
        public string Activity_Name { get; set; }
        public DateTime Activity_Date { get; set; }
        public string Activity_Time { get; set; }
        public string Comments { get; set; }
    }
}
