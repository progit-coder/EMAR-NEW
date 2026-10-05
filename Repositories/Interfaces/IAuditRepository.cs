using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IAuditRepository
    {
        AuditCustomEntity GetAuditTableData(string tableName, int recordId);
        AuditCustomEntity GetUserRoleConfigAudit(int userId, int roleId, int facilityId);
    }
}
