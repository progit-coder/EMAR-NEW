using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class AuditService : IAuditService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger _log;

        public AuditService(IAutoMapper autoMapper, IAuditRepository auditRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._auditRepository = auditRepository;
            this._log = log;
        }

        public async Task<AuditCustomEntity> GetAuditTableData(string tableName, int recordId)
        {
            this._log.Debug("---Executing GetAuditTableData() in AuditService----");
            return await Task.FromResult<AuditCustomEntity>(this._auditRepository.GetAuditTableData(tableName, recordId));
        }
        public async Task<AuditCustomEntity> GetUserRoleConfigAudit(int userId, int roleId, int facilityId)
        {
            this._log.Debug("---Executing GetUserRoleConfigAudit() in AuditService----");
            return await Task.FromResult<AuditCustomEntity>(this._auditRepository.GetUserRoleConfigAudit(userId, roleId, facilityId));
        }
    }
}
