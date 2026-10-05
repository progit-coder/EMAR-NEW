using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.Entities;
using System.Web.Http.Results;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("FTEConfiguration")]
    [CustomAuthorizationFilter]
    public class FTEConfigurationController : ApiController
    {
        private readonly IFTEConfigurationService _fteConfigService;
        private readonly ILogger _log;
        public FTEConfigurationController(IFTEConfigurationService fteConfigService, ILogger log)
        {
            this._fteConfigService = fteConfigService;
            this._log = log;
        }

        [Route("GetAllFTEConfigurations")]
        [HttpGet]
        public JsonResult<List<FTConfigurationGridEntity>> GetAllFTEConfigurationsList()
        {
            this._log.Debug("---Executing GetAllFTEConfigurationsList() in FTEConfigurationController----");
            var fteConfigs = this._fteConfigService.GetAllFTEConfigurationsList().Result;
            this._log.Debug("---Executed Successfully GetAllFTEConfigurationsList() in FTEConfigurationController----");
            return Json<List<FTConfigurationGridEntity>>(fteConfigs);
        }
        [Route("GetFTEConfigurationById/{FTEConfigId}")]
        [HttpGet]
        public JsonResult<FTEConfigurationEntity> GetFTEConfigurationDetailsByID(int fteConfigId)
        {
            this._log.Debug("---Executing GetFTEConfigurationDetailsByID() in FTEConfigurationController----");
            var fteConfig = this._fteConfigService.GetFTEConfigurationDetailsByID(fteConfigId).Result;
            this._log.Debug("---Executed Successfully GetFTEConfigurationDetailsByID() in FTEConfigurationController----");
            return Json<FTEConfigurationEntity>(fteConfig);
        }
        [Route("InsertFTEConfiguration")]
        [HttpPost]
        public int InsertUpdateFTEConfiguration(FTEConfigurationEntity entity)
        {
            this._log.Debug("---Executing Insert/Update() in FTEConfigurationController----");
            int result = this._fteConfigService.InsertUpdateFTEConfiguration(entity).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in FTEConfigurationController----");
            return result;
        }

    }
}
