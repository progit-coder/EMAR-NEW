using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using LTCPro.Entities;
using System.Web.Http.Results;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("HLSevenSegment")]
    [CustomAuthorizationFilter]
    public class HLSevenSegmentController : ApiController
    {
        private readonly IHLSevenSegmentService _hLSevenSegmentService;
        private readonly ILogger _log;

        public HLSevenSegmentController(IHLSevenSegmentService hLSevenSegmentService, ILogger log)
        {
            this._hLSevenSegmentService = hLSevenSegmentService;
            this._log = log;
        }
        [Route("GetHLSevenSegmentData")]
        [HttpGet]
        public JsonResult<List<HLSevenSegmentEntity>> GetHLSevenSegmentData()
        {
            this._log.Debug("---Executing GetHLSevenSegmentData() in HLSevenSegmentController----");
            var segments = this._hLSevenSegmentService.GetHLSevenSegmentData().Result;
            this._log.Debug("---Executed Successfully GetHLSevenSegmentData() in HLSevenSegmentController----");
            return Json<List<HLSevenSegmentEntity>>(segments);
        }
        [Route("GetHLSevenSegmentDetailData/{segmentID}")]
        [HttpGet]
        public JsonResult<List<HLSevenSegmentDetailEntity>> GetHLSevenSegmentDetailData(int segmentID)
        {
            this._log.Debug("---Executing GetHLSevenSegmentDetailData() in HLSevenSegmentController----");
            var segmentDetails = this._hLSevenSegmentService.GetHLSevenSegmentDetailData(segmentID).Result;
            this._log.Debug("---Executed Successfully GetHLSevenSegmentDetailData() in HLSevenSegmentController----");
            return Json<List<HLSevenSegmentDetailEntity>>(segmentDetails);
        }
        [Route("GetHLSevenCompanyConfigEntity/{CompanyID}/{SegmentID}")]
        [HttpGet]
        public JsonResult<List<HLSevenCompanyConfigEntity>> GetHLSevenCompanyConfigEntity(int CompanyID, int SegmentID)
        {
            this._log.Debug("---Executing GetHLSevenCompanyConfigEntity() in HLSevenSegmentController----");
            var segmentDetails = this._hLSevenSegmentService.GetHLSevenCompanyConfigEntity(CompanyID, SegmentID).Result;
            this._log.Debug("---Executed Successfully GetHLSevenCompanyConfigEntity() in HLSevenSegmentController----");
            return Json<List<HLSevenCompanyConfigEntity>>(segmentDetails);
        }

        [Route("InsertHLSevenCompanyConfig")]
        [HttpPost]
        public int InsertHLSevenCompanyConfig(HLSevenCompanyConfigEntity hlSevenconfig)
        {
            this._log.Debug("---Executing InsertHLSevenCompanyConfig() in HLSevenSegmentController----");
            var result = this._hLSevenSegmentService.InsertHLSevenCompanyConfig(hlSevenconfig).Result;
            this._log.Debug("---Executed Successfully InsertHLSevenCompanyConfig() in HLSevenSegmentController----");
            return result;
        }
        [Route("GetHLSevenConfigs/{ResidentId}/{SegmentDesc}")]
        [HttpGet]
        public JsonResult<List<HlFieldsEntity>> GetHLSevenConfigs(int residentId, string segmentDesc)
        {
            this._log.Debug("---Executing GetHLSevenConfigs() in HLSevenSegmentController----");
            var configs = this._hLSevenSegmentService.GetHLSevenConfigs(residentId, segmentDesc).Result;
            this._log.Debug("---Executed Successfully GetHLSevenConfigs() in HLSevenSegmentController----");
            return Json<List<HlFieldsEntity>>(configs);

        }
        [Route("CloneHlSevenCompanyConfig")]
        [HttpPost]
        public int CloneHlSevenCompanyConfig(HLSevenCloneEntity hlCloneConfig)
        {
            this._log.Debug("---Executing CloneHlSevenCompanyConfig() in HLSevenSegmentController----");
            var record = this._hLSevenSegmentService.CloneHlSevenCompanyConfig(hlCloneConfig).Result;
            this._log.Debug("---Executed Successfully CloneHlSevenCompanyConfig() in HLSevenSegmentController----");
            return record;

        }
        [Route("GetHLSevenConfigsBysegments")]
        [HttpPost]
        public JsonResult<List<HlFieldsEntity>> GetHLSevenConfigsBysegments(HLSevenConfigsEntity hlConfigs)
        {
            this._log.Debug("---Executing GetHLSevenConfigsByArray() in HLSevenSegmentController----");
            var configs = this._hLSevenSegmentService.GetHLSevenConfigsBysegments(hlConfigs).Result;
            this._log.Debug("---Executed Successfully GetHLSevenConfigsByArray() in HLSevenSegmentController----");
            return Json<List<HlFieldsEntity>>(configs);

        }
        [Route("GetHLSevenOutboundDisplaySegmentsData")]
        [HttpGet]
        public JsonResult<List<HLSevenOutboundDisplaySegmentEntity>> GetHLSevenOutboundDisplaySegmentsData()
       {
            this._log.Debug("---Executing GetHLSevenOutboundDisplaySegmentsData() in HLSevenSegmentController----");
            var configs = this._hLSevenSegmentService.GetHLSevenOutboundDisplaySegmentsData().Result;
            this._log.Debug("---Executed Successfully GetHLSevenOutboundDisplaySegmentsData() in HLSevenSegmentController----");
            return Json<List<HLSevenOutboundDisplaySegmentEntity>>(configs);

        }
        [Route("GetHlsevenEnabledCompanyDetails")]
        [HttpGet]
        public JsonResult<List<CompanyDropEntity>> GetHlsevenEnabledCompanyDetails()
        {
            this._log.Debug("---Executing GetHlsevenEnabledCompanyDetails() in HLSevenSegmentController----");
            var companies = this._hLSevenSegmentService.GetHlsevenEnabledCompanyDetails().Result;
            this._log.Debug("---Executed Successfully GetHlsevenEnabledCompanyDetails() in HLSevenSegmentController----");
            return Json<List<CompanyDropEntity>>(companies);

        }
        [Route("GetHLSevenOutboundDisplaySegmentsGridData/{segmentId}/{companyId}")]
        [HttpGet]
        public JsonResult<List<CustomHLSevenOutboundDisplaySegmentGridDataEntity>> GetHLSevenOutboundDisplaySegmentsGridData(int segmentId, Nullable<int> companyId)
        {
            this._log.Debug("---Executing GetHLSevenOutboundDisplaySegmentsGridData() in HLSevenSegmentController----");
            var data = this._hLSevenSegmentService.GetHLSevenOutboundDisplaySegmentsGridData(segmentId,companyId).Result;
            this._log.Debug("---Executed Successfully GetHLSevenOutboundDisplaySegmentsGridData() in HLSevenSegmentController----");
            return Json<List<CustomHLSevenOutboundDisplaySegmentGridDataEntity>>(data);

        }
        [Route("InsertUpdateHlsevenOutboundDisplayConfigsData")]
        [HttpPost]
        public int InsertUpdateHlsevenOutboundDisplayConfigsData(CustomHLSevenOutboundDisplayCompanyConfigEntity displayConfigs)
        {
            this._log.Debug("---Executing InsertUpdateHlsevenOutboundDisplayConfigsData() in HLSevenSegmentController----");
            var configs = this._hLSevenSegmentService.InsertUpdateHlsevenOutboundDisplayConfigsData(displayConfigs).Result;
            this._log.Debug("---Executed Successfully InsertUpdateHlsevenOutboundDisplayConfigsData() in HLSevenSegmentController----");
            return configs;

        }
        [Route("GetOutboundHLSevenConfigs/{residentId}/{segmentId}/{facilityID?}")]
        [HttpGet]
        public JsonResult<List<HLSevenOutboundDisplayCompanyConfigEntity>> GetOutboundHLSevenConfigs(int residentId, int segmentId, int? facilityID = null)
        {
            this._log.Debug("---Executing GetOutboundHLSevenConfigs() in HLSevenSegmentController----");
            var configs = this._hLSevenSegmentService.GetOutboundHLSevenConfigs(residentId, segmentId,facilityID).Result;
            this._log.Debug("---Executed Successfully GetOutboundHLSevenConfigs() in HLSevenSegmentController----");
            return Json<List<HLSevenOutboundDisplayCompanyConfigEntity>>(configs);

        }
    }
}