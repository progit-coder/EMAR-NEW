using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IAuditService
    {
        Task<AuditCustomEntity> GetAuditTableData(string tableName, int recordId);
        Task<AuditCustomEntity> GetUserRoleConfigAudit(int userId, int roleId, int facilityId);
    }
}
