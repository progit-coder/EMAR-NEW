using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [CustomAuthorizationFilter]
    [RoutePrefix("AuditTables")]

    public class AuditController : ApiController
    {
        private readonly IAuditService _auditService;
        private readonly ILogger _log;
        public AuditController(IAuditService auditService, ILogger log)
        {
            this._auditService = auditService;
            this._log = log;
        }

        [Route("GetAuditTableData/{TableName}/{RecordId}")]
        [HttpGet]
        public JsonResult<AuditCustomEntity> GetAuditTableData(string tableName, int recordId)
        {
            this._log.Debug("---Executing GetAuditTableData() in AuditController----");
            var records = this._auditService.GetAuditTableData(tableName, recordId).Result;
            this._log.Debug("---Executed Successfully GetAuditTableData() in AuditController----");
            return Json<AuditCustomEntity>(records);
        }
        [Route("GetUserRoleConfigAudit/{userId}/{roleId}/{facilityId}")]
        [HttpGet]
        public JsonResult<AuditCustomEntity> GetUserRoleConfigAudit(int userId, int roleId, int facilityId)
        {
            this._log.Debug("---Executing GetUserRoleConfigAudit() in AuditController----");
            var records = this._auditService.GetUserRoleConfigAudit(userId, roleId, facilityId).Result;
            this._log.Debug("---Executed Successfully GetUserRoleConfigAudit() in AuditController----");
            return Json<AuditCustomEntity>(records);
        }
    }
}
