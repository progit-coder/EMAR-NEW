using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class PrcReportsAdminUsers_ResultEntity
    {
        public string company_Name { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string RoleName { get; set; }
        public string userStatus { get; set; }
    }
    public class PrcReportAdminUsersGridEntity
    {
        public List<PrcReportsAdminUsers_ResultEntity> GridData { get; set; }
        public int TotalRecordsCount { get; set; }
    }
}
