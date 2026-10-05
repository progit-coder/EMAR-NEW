using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{

    [CustomApiExceptionFilter]
    [RoutePrefix("Common")]
    [CustomAuthorizationFilter]
    public class CommonController : ApiController
    {
        private readonly ICommonService _commonService;
        private readonly ILogger _log;
        public CommonController(ICommonService commonService, ILogger log)
        {
            this._commonService = commonService;
            this._log = log;
        }
        [Route("GetAllCountries")]
        [HttpGet]
        public JsonResult<List<CountryEntity>> GetAllCountries()
        {
            this._log.Debug("---Executing GetAllCountries() in CommonController----");
            var countries = this._commonService.GetAllCountries().Result;
            this._log.Debug("---Executed Successfully Get() in CommonController----");
            return Json<List<CountryEntity>>(countries);

        }
        [Route("GetOderFavInfo")]
        [HttpGet]
        public JsonResult<List<OrderFavouriteCustomEntity>> GetOderFavInfo()
        {
            this._log.Debug("---Executing GetOderFavInfo() in CommonController----");
            var result = this._commonService.GetOderFavInfo().Result;
            this._log.Debug("---Executed Successfully GetOderFavInfo() in CommonController----");
            return Json<List<OrderFavouriteCustomEntity>>(result);
        }
        [Route("GetOrderFavDetails/{FacilityId}")]
        [HttpGet]
        public JsonResult<List<PrcGetOrderFavConfigInfoEntity>> GetOrderFavDetails(int FacilityId)
        {
            this._log.Debug("---Executing GetOrderFavDetails() in CommonController----");
            var result = this._commonService.GetOrderFavDetails(FacilityId).Result;
            this._log.Debug("---Executed Successfully GetOrderFavDetails() in CommonController----");
            return Json<List<PrcGetOrderFavConfigInfoEntity>>(result);
        }
        [Route("GetOrderFavConfigDetailsById")]
        [HttpPost]
        public JsonResult<PrcGetOrderFavConfigByCodeEntity> GetOrderFavConfigDetailsById(CustomOrderFavInfoDataEntity entity)
        {
            this._log.Debug("---Executing GetOrderFavConfigDetailsById() in CommonController----");
            var result = this._commonService.GetOrderFavConfigDetailsById(entity).Result;
            this._log.Debug("---Executed Successfully GetOrderFavConfigDetailsById() in CommonController----");
            return Json<PrcGetOrderFavConfigByCodeEntity>(result);
        }

        [Route("InsertUpdateMeasurementsandUserInputs")]
        [HttpPost]
        public JsonResult<int> InsertUpdateMeasurementsandUserInputs(PrcInsertOrderFavFacilityConfigEntity obj)
        {
            this._log.Debug("---Executing InsertUpdateMeasurementsandUserInputs() in CommonController----");
            var result = this._commonService.InsertUpdateMeasurementsandUserInputs(obj).Result;
            this._log.Debug("---Executed Successfully InsertUpdateMeasurementsandUserInputs() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetGenders")]
        [HttpGet]
        public JsonResult<List<GenderEntity>> GetGenders()
        {
            this._log.Debug("---Executing GetGenders() in CommonController----");
            var genders = this._commonService.GetGenders().Result;
            this._log.Debug("---Executed Successfully GetGenders() in CommonController----");
            return Json<List<GenderEntity>>(genders);
        }
        [Route("GetSuffixes")]
        [HttpGet]
        public JsonResult<List<SuffixEntity>> GetSuffixes()
        {
            this._log.Debug("---Executing GetSuffix() in CommonController----");
            var suffixes = this._commonService.GetSuffixes().Result;
            this._log.Debug("---Executed Successfully GetSuffix() in CommonController----");
            return Json<List<SuffixEntity>>(suffixes);
        }
        [Route("GetFTEConnections")]
        [HttpGet]
        public JsonResult<List<FteConnectionEntity>> GetFTEConnections()
        {
            this._log.Debug("---Executing GetFTEConnections() in CommonController----");
            var fteConnections = this._commonService.GetFTEConnections().Result;
            this._log.Debug("---Executed Successfully GetFTEConnections() in CommonController----");
            return Json<List<FteConnectionEntity>>(fteConnections);
        }
        [Route("GetFTECategories")]
        [HttpGet]
        public JsonResult<List<FTECategoryEntity>> GetFTECategories()
        {
            this._log.Debug("---Executing GetFTECategories() in CommonController----");
            var fteCategories = this._commonService.GetFTECategories().Result;
            this._log.Debug("---Executed Successfully GetFTECategories() in CommonController----");
            return Json<List<FTECategoryEntity>>(fteCategories);
        }
        [Route("ListFiles")]
        [HttpGet]
        public JsonResult<List<string>> ListFiles()
        {
            try
            {
                this._log.Debug("---Executing ListFiles() in CommonController----");
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://50.87.144.11/ITTeam/InBound/");
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential("emar@promantra.net", "?e%5#23jvT{?");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                List<string> data = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                this._log.Debug("---Executed Successfully ListFiles() in CommonController----");
                return Json<List<string>>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Route("InsertUpdatePatientType")]
        [HttpPost]
        public JsonResult<int> InsertUpdatePatientType(PatientTypeEntity entity)
        {
            this._log.Debug("---Executing InsertUpdatePatientType() in CommonController----");
            var result = this._commonService.InsertUpdatePatientType(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdatePatientType() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetPatientTypeData")]
        [HttpGet]
        public JsonResult<List<PatientTypeCustomEntity>> GetPatientTypeData()
        {
            this._log.Debug("---Executing GetPatientTypeData() in CommonController----");
            var result = this._commonService.GetPatientTypeData().Result;
            this._log.Debug("---Executed Successfully GetPatientTypeData() in CommonController----");
            return Json<List<PatientTypeCustomEntity>>(result);
        }
        [Route("GetPatientTypeByID/{patientTypeId}")]
        [HttpGet]
        public JsonResult<PatientTypeCustomEntity> GetPatientTypeByID(int patientTypeId)
        {
            this._log.Debug("---Executing GetPatientTypeByID() in CommonController----");
            var result = this._commonService.GetPatientTypeByID(patientTypeId).Result;
            this._log.Debug("---Executed Successfully GetPatientTypeByID() in CommonController----");
            return Json<PatientTypeCustomEntity>(result);
        }
        [Route("GetPatientTypeDrop/{patientId}")]
        [HttpGet]
        public JsonResult<List<PatientTypeCustomEntity>> GetPatientTypeDrop(int patientId)
        {
            this._log.Debug("---Executing GetPatientTypeByID() in CommonController----");
            var result = this._commonService.GetPatientTypeDrop(patientId).Result;
            this._log.Debug("---Executed Successfully GetPatientTypeDrop() in CommonController----");
            return Json<List<PatientTypeCustomEntity>>(result);
        }
        [Route("UpdatePatientTypeStatus")]
        [HttpPost]
        public int UpdatePatientTypeStatus(List<PatientTypeCustomEntity> data)
        {
            this._log.Debug("---Executing UpdatePatientTypeStatus() in CommonController----");
            var result = this._commonService.UpdatePatientTypeStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdatePatientTypeStatus() in CommonController----");
            return result;
        }
        [Route("GetUserRecentFacNs/{userId}")]
        [HttpGet]
        public JsonResult<RecentFacEntity> GetUserRecentFacNs(int userId)
        {
            this._log.Debug("---Executing GetUserRecentFacNs() in UserController----");
            var users = this._commonService.GetUserRecentFacNs(userId).Result;
            this._log.Debug("---Executed Successfully GetUserRecentFacNs() in UserController----");
            return Json<RecentFacEntity>(users);
        }

        //DocumentCheckOrder
        [Route("RemoveDocumentCheck")]
        [HttpPost]
        public JsonResult<int> RemoveDocumentCheck(List<DrugAdministerEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdatePatientType() in CommonController----");
            var result = this._commonService.RemoveDocumentCheck(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdatePatientType() in CommonController----");
            return Json<int>(result);
        }

        [Route("InsertUpdateDocumentCheck")]
        [HttpPost]
        public JsonResult<int> InsertUpdateDocumentCheck(List<DrugAdministerEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdatePatientType() in CommonController----");
            var result = this._commonService.InsertUpdateDocumentCheck(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdatePatientType() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetOrdersListData")]
        [HttpPost]
        public JsonResult<List<GetDocumentCheckDataEntity>> GetOrdersListData(OrdersListFilter filter)
        {
            this._log.Debug("---Executing GetOrdersListData() in CommonController----");
            var result = this._commonService.GetOrdersListData(filter).Result;
            this._log.Debug("---Executed Successfully GetOrdersListData() in CommonController----");
            return Json<List<GetDocumentCheckDataEntity>>(result);
        }
        [Route("GetDrugsListData/{residentId}/{startdate}/{enddate}")]
        [HttpGet]
        public JsonResult<List<GetDocumentCheckDataEntity>> GetDrugsListData(int residentId, string startdate, string enddate)
        {
            this._log.Debug("---Executing GetDrugsListData() in CommonController----");
            var result = this._commonService.GetDrugsListData(residentId, startdate, enddate).Result;
            this._log.Debug("---Executed Successfully GetDrugsListData() in CommonController----");
            return Json<List<GetDocumentCheckDataEntity>>(result);
        }
        [Route("GetPendingDosesList/{userId}/{nsIds}/{facId}")]
        [HttpGet]
        public JsonResult<List<DosesDetailsEntity>> GetPendingDosesList(int userId, string nsIds, int? facId)
        {
            this._log.Debug("---Executing GetPendingDosesList() in CommonController----");
            //var result = this._commonService.GetPendingDosesList(userId, nsIds, facId).Result;
            var result = this._commonService.GetPendingDosesList(userId, nsIds, facId).GetAwaiter().GetResult();
            this._log.Debug("---Executed Successfully GetPendingDosesList() in CommonController----");
            return Json<List<DosesDetailsEntity>>(result);
        }
        [Route("NoAdminitration")]
        [HttpPost]
        public JsonResult<int> NoAdminitration(DrugAdministerEntity drugAdministerEntity)
        {
            this._log.Debug("---Executing GetPendingDosesList() in CommonController----");
            var result = this._commonService.NoAdminitration(drugAdministerEntity).Result;
            this._log.Debug("---Executed Successfully GetPendingDosesList() in CommonController----");
            return Json<int>(result);
        }
        [Route("InsertIgnoreDosesDetails/{drugAdministerId}")]
        [HttpGet]
        public JsonResult<int> InsertIgnoreDosesDetails(long drugAdministerId)
        {
            this._log.Debug("---Executing InsertIgnoreDosesDetails() in CommonController----");
            var result = this._commonService.InsertIgnoreDosesDetails(drugAdministerId).Result;
            this._log.Debug("---Executed Successfully InsertIgnoreDosesDetails() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetDocAdminOrderAudit/{DrugAdminsterID}")]
        [HttpGet]
        public JsonResult<List<PrcGetDocAdminOrderAudit_ResultEntity>> GetDocAdminOrderAudit(Int64 DrugAdminsterID)
        {
            this._log.Debug("---Executing GetDocAdminOrderAudit() in CommonController----");
            var result = this._commonService.GetDocAdminOrderAudit(DrugAdminsterID).Result;
            this._log.Debug("---Executed Successfully GetDocAdminOrderAudit() in CommonController----");
            return Json<List<PrcGetDocAdminOrderAudit_ResultEntity>>(result);
        }
        [Route("GetOutboundErrorDetails/{userId}")]
        [HttpGet]
        public JsonResult<List<OutboundErrorDetailsEntity>> GetOutboundErrorDetails(int userId)
        {
            this._log.Debug("---Executing GetOutboundErrorDetails() in CommonController----");
            //var result = this._commonService.GetOutboundErrorDetails(userId).Result;
            var result = this._commonService.GetOutboundErrorDetails(userId).GetAwaiter().GetResult();
            this._log.Debug("---Executed Successfully GetOutboundErrorDetails() in CommonController----");
            return Json<List<OutboundErrorDetailsEntity>>(result);
        }
        [Route("InsertIgnoreOutboundErrorDetails/{fileId}")]
        [HttpGet]
        public JsonResult<int> InsertIgnoreOutboundErrorDetails(int fileId)
        {
            this._log.Debug("---Executing InsertIgnoreOutboundErrorDetails() in CommonController----");
            var result = this._commonService.InsertIgnoreOutboundErrorDetails(fileId).Result;
            this._log.Debug("---Executed Successfully InsertIgnoreDosesDetails() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetUserPassedDueFlag/{userId}")]
        [HttpGet]
        public int GetUserPassedDueFlag(int userId)
        {
            this._log.Debug("---Executing GetUserPassedDueFlag() in UserController----");
            var users = this._commonService.GetUserPassedDueFlag(userId).Result;
            this._log.Debug("---Executed Successfully GetUserPassedDueFlag() in UserController----");
            return users;
        }
        [Route("InsertUpdateRefillMailConfig")]
        [HttpPost]
        public JsonResult<int> InsertUpdateRefillMailConfig(RefillMailConfigCustomEntity entity)
        {
            this._log.Debug("---Executing InsertUpdateRefillMailConfig() in CommonController----");
            var result = this._commonService.InsertUpdateRefillMailConfig(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateRefillMailConfig() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetRefillConfigGridData/{userId}")]
        [HttpGet]
        public JsonResult<List<RefillMailConfigGridData>> GetRefillConfigGridData(int userId)
        {
            this._log.Debug("---Executing GetRefillConfigGridData() in CommonController----");
            var result = this._commonService.GetRefillConfigGridData(userId).Result;
            this._log.Debug("---Executed Successfully GetRefillConfigGridData() in CommonController----");
            return Json<List<RefillMailConfigGridData>>(result);
        }
        [Route("GetTimeZoneDate/{nursingStationId}")]
        [HttpGet]
        public JsonResult<string> GetTimeZoneDate(int nursingStationId)
        {
            this._log.Debug("---Executing GetTimeZoneDate() in CommonController----");
            var result = this._commonService.GetTimeZoneDate(nursingStationId).Result;
            this._log.Debug("---Executed Successfully GetTimeZoneDate() in CommonController----");
            return Json<string>(result);
        }
        [Route("inserUpdateMailconfigDetails")]
        [HttpPost]
        public JsonResult<int> inserUpdateMailconfigDetails(MailConfigEntity entity)
        {
            this._log.Debug("---Executing inserUpdateMailconfigDetails() in CommonController----");
            var result = this._commonService.inserUpdateMailconfigDetails(entity).Result;
            this._log.Debug("---Executed Successfully inserUpdateMailconfigDetails() in CommonController----");
            return Json<int>(result);
        }

        [Route("GetState")]
        [HttpGet]
        public JsonResult<List<StateEntity>> GetState()
        {
            this._log.Debug("---Executing GetState() in CommonController----");
            var states = this._commonService.GetState().Result;
            this._log.Debug("---Executed Successfully GetState() in CommonController----");
            return Json<List<StateEntity>>(states);
        }


        [Route("GetPrimarySpeciality")]
        [HttpGet]
        public JsonResult<List<PrimarySpeciality>> GetPrimarySpeciality()
        {
            this._log.Debug("---Executing GetPrimarySpeciality() in CommonController----");
            var result = this._commonService.GetPrimarySpeciality().Result;
            this._log.Debug("---Executed Successfully GetPrimarySpeciality() in CommonController----");
            return Json<List<PrimarySpeciality>>(result);
        }
        [Route("GetCredentialMaster")]
        [HttpGet]
        public JsonResult<List<CredentialMaster>> GetCredentialMaster()
        {
            this._log.Debug("---Executing GetCredentialMaster() in CommonController----");
            var result = this._commonService.GetCredentialMaster().Result;
            this._log.Debug("---Executed Successfully GetCredentialMaster() in CommonController----");
            return Json<List<CredentialMaster>>(result);
        }
    }
}
