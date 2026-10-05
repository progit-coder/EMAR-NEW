using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [CustomApiExceptionFilter]
   [CustomAuthorizationFilter]
    [RoutePrefix("Company")]
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger _log;
        public CompanyController(ICompanyService companyService, ILogger log)
        {
            this._companyService = companyService;
            this._log = log;
        }

        [Route("UserCompanyMaster")]
        [HttpGet]
        public JsonResult<List<CompanyCustomEntity>> GetUserActiveCompanyMasterList()
        {
            this._log.Debug("---Executing GetUserActiveCompanyMasterList() in CompanyController----");
            var companies = this._companyService.GetUserActiveCompanyMasterList().Result;
            this._log.Debug("---Executed Successfully GetUserActiveCompanyMasterList() in CompanyController----");
            return Json<List<CompanyCustomEntity>>(companies);
        }

        [Route("AllCompanyMaster")]
        [HttpGet]
        public JsonResult<List<CompanyCustomEntity>> GetAllCompanyMasterList()
        {
            this._log.Debug("---Executing GetAllCompanyMasterList() in CompanyController----");
            var companies = this._companyService.GetAllCompanyMasterList().Result;
            this._log.Debug("---Executed Successfully GetAllCompanyMasterList() in CompanyController----");
            return Json<List<CompanyCustomEntity>>(companies);
        }
        [Route("GetAllCompanyConfigList")]
        [HttpGet]
        public JsonResult<List<CompanyConfigEntity>> GetAllCompanyConfigList()
        {
            this._log.Debug("---Executing GetAllCompanyConfigList() in CompanyController----");
            var companies = this._companyService.GetAllCompanyConfigList().Result;
            this._log.Debug("---Executed Successfully GetAllCompanyConfigList() in CompanyController----");
            return Json<List<CompanyConfigEntity>>(companies);
        }

        [Route("UserCompanyDrop")]
        [HttpGet]
        public JsonResult<List<CompanyDropEntity>> GetUserActiveCompanyNames()
        {
            this._log.Debug("---Executing GetUserActiveCompanyNames() in CompanyController----");
            var companies = this._companyService.GetUserActiveCompanyNames().Result;
            this._log.Debug("---Executed Successfully GetUserActiveCompanyNames() in CompanyController----");
            return Json<List<CompanyDropEntity>>(companies);
        }
        [Route("GetHLDirectionList")]
        [HttpGet]
        public JsonResult<List<HLDirectionEntity>> GetHLDirectionList()
        {
            this._log.Debug("---Executing GetHLDirectionList() in CompanyController----");
            var hldesc = this._companyService.GetHLDirectionList().Result;
            this._log.Debug("---Executed Successfully GetHLDirectionList() in CompanyController----");
            return Json<List<HLDirectionEntity>>(hldesc);
        }
        [Route("GetAllFTECategory")]
        [HttpGet]
        public JsonResult<List<FTECategoryEntity>> GetAllFTECategory()
        {
            this._log.Debug("---Executing GetAllFTECategory() in CompanyController----");
            var category = this._companyService.GetAllFTECategory().Result;
            this._log.Debug("---Executed Successfully GetAllFTECategory() in CompanyController----");
            return Json<List<FTECategoryEntity>>(category);
        }
        [Route("GetAllEventCategroy/{directionalId}/{categoryId}")]
        [HttpGet]
        public JsonResult<List<EventCategoryEntity>> GetAllEventCategroy(int directionalId, int categoryId)
        {
            this._log.Debug("---Executing GetAllEventCategroy() in CompanyController----");
            var category = this._companyService.GetAllEventCategroy(directionalId, categoryId).Result;
            this._log.Debug("---Executed Successfully GetAllEventCategroy() in CompanyController----");
            return Json<List<EventCategoryEntity>>(category);
        }
        [Route("AllActiveCompanyDrop")]
        [HttpGet]
        public JsonResult<List<CompanyDropEntity>> GetAllActiveCompanyNames()
        {
            this._log.Debug("---Executing GetAllActiveCompanyNames() in CompanyController----");
            var companies = this._companyService.GetAllActiveCompanyNames().Result;
            this._log.Debug("---Executed Successfully GetAllActiveCompanyNames() in CompanyController----");
            return Json<List<CompanyDropEntity>>(companies);
        }
        [Route("Company/{CompanyId}")]
        [HttpGet]
        public JsonResult<CompanyCustomEntity> GetCompanyDetailsByID(int companyId)
        {
            this._log.Debug("---Executing GetCompanyDetailsByID() in CompanyController----");
            var company = this._companyService.GetCompanyDetailsByID(companyId).Result;
            this._log.Debug("---Executed Successfully GetCompanyDetailsByID() in CompanyController----");
            return Json<CompanyCustomEntity>(company);
        }
        [Route("InsertComapnyConfigDetails")]
        [HttpPost]
        public int InsertComapnyConfigDetails(CompanyConfigEntity config)
        {
            this._log.Debug("---Executing InsertComapnyConfigDetails() in CompanyController----");
            int result = this._companyService.InsertComapnyConfigDetails(config).Result;
            this._log.Debug("---Executed Successfully InsertComapnyConfigDetails() in CompanyController----");
            return result;
        }
        [Route("InsertCompany")]
        [HttpPost]
        public int InsertUpdateCompanyMaster(string body)
        {
            CompanyEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyEntity>(body);
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                byte[] buffer = new byte[16 * 1024];
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    Stream input = httpRequest.Files[i].InputStream;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);        
                        }
                        if (httpRequest.Files.Keys[i] == "Image")
                        {
                            entity.Company_Logo = ms.ToArray();
                        }
                        else
                        {
                            entity.Company_FooterLogo = ms.ToArray();
                        }
                    }
                }
            }
            this._log.Debug("---Executing Insert/Update() in CompanyController----");
            int result = this._companyService.InsertUpdateCompanyMaster(entity).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in CompanyController----");
            return result;
        }
        [Route("CompanyUIDDrop")]
        [HttpGet]
        public JsonResult<List<CompanyUIDDropEntity>> GetCompanyUIDDropDownList()
        {
            this._log.Debug("---Executing GetCompanyUIDDropDownList() in CompanyController----");
            var companyUIDs = this._companyService.GetCompanyUIDDropDownList().Result;
            return Json<List<CompanyUIDDropEntity>>(companyUIDs);
        }
        [Route("GetApprovalFlag/{PatientId}")]
        [HttpGet]
        public JsonResult<int> GetApprovalFlag(int patientId)
        {
            this._log.Debug("---Executing GetApprovalFlag() in CompanyController----");
            var flag = this._companyService.GetApprovalFlag(patientId).Result;
            return Json<int>(flag);
        }
        [Route("GetFingersDescDrop")]
        [HttpGet]
        public JsonResult<List<FingersdescEntity>> GetFingersDescDrop()
        {
            this._log.Debug("---Executing GetFingersDescDrop() in CompanyController----");
            var fingerDescDrop = this._companyService.GetFingersDescDrop().Result;
            return Json<List<FingersdescEntity>>(fingerDescDrop);
        }
        [Route("GetCompanyConfigById/{companyConfigId}/{hl7Configured}/{FteCategoryId}")]
        [HttpGet]
        public JsonResult<CompanyConfigEntity> GetCompanyConfigById(int companyConfigId, int hl7Configured, int FteCategoryId)
        {
            this._log.Debug("---Executing GetCompanyConfigById() in CompanyController----");
            var result = this._companyService.GetCompanyConfigById(companyConfigId, hl7Configured, FteCategoryId).Result;
            return Json<CompanyConfigEntity>(result);
        }
        [Route("GetCompanyEventCategoriesByPid/{patientId}")]
        [HttpGet]
        public JsonResult<List<EventCategoryEntity>> GetCompanyEventCategoriesByPid(int patientId)
        {
            this._log.Debug("---Executing GetCompanyEventCategoriesByPid() in CompanyController----");
            var result = this._companyService.GetCompanyEventCategoriesByPid(patientId).Result;
            return Json<List<EventCategoryEntity>>(result);
        }
        [Route("GetStockReportForDropData")]
        [HttpGet]
        public JsonResult<List<StockReportEntity>> GetStockReportForDropData()
        {
            this._log.Debug("---Executing GetStockReportForDropData() in CompanyController----");
            var result = this._companyService.GetStockReportForDropData().Result;
            return Json<List<StockReportEntity>>(result);
        }
        [Route("UpdateCompaniesStatus")]
        [HttpPost]
        public int UpdateCompaniesStatus(List<CompanyCustomEntity> data)
        {
            this._log.Debug("---Executing UpdateCompaniesStatus() in CompanyController----");
            var result = this._companyService.UpdateCompaniesStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateCompaniesStatus() in CompanyController----");
            return result;
        }
        [Route("GetBiometricFingerConfigByNsId/{nurseStationId}")]
        [HttpGet]
        public int GetBiometricFingerConfigByNsId(int nurseStationId)
        {
            this._log.Debug("---Executing GetBiometricFingerConfigByNsId() in CompanyController----");
            var result = this._companyService.GetBiometricFingerConfigByNsId(nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetBiometricFingerConfigByNsId() in CompanyController----");
            return result==null ? 0: (int)result;
        }
        [Route("GetStockUserCompanyDrop/{userId}")]
        [HttpGet]
        public JsonResult<List<CompanyDropEntity>> GetStockUserCompanyDrop(int userId)
        {
            this._log.Debug("---Executing GetStockUserCompanyDrop() in CompanyController----");
            var companies = this._companyService.GetStockUserCompanyDrop(userId).Result;
            this._log.Debug("---Executed Successfully GetStockUserCompanyDrop() in CompanyController----");
            return Json<List<CompanyDropEntity>>(companies);
        }
        [Route("GetAllFlagsForCompanyByNSId/{nurseStationId}/{residentId?}")]
        [HttpGet]
        public CompanyFlagsEntity GetAllFlagsForCompanyByNSId(int nurseStationId,int? residentId=0)
        {
            this._log.Debug("---Executing GetDrFirstFlagForCompanyByNSId() in CompanyController----");
            var result = this._companyService.GetAllFlagsForCompanyByNSId(nurseStationId,residentId).Result;
            this._log.Debug("---Executed Successfully GetDrFirstFlagForCompanyByNSId() in CompanyController----");
            return result;
        }
        [Route("GetCompanyHlSevenFlag/{fcailityId}/{companyId}")]
        [HttpGet]
        public int GetCompanyHlSevenFlag(int fcailityId, int companyId)
        {
            this._log.Debug("---Executing GetCompanyHlSevenFlag() in CompanyController----");
            var result = this._companyService.GetCompanyHlSevenFlag(fcailityId, companyId).Result;
            this._log.Debug("---Executed Successfully GetCompanyHlSevenFlag() in CompanyController----");
            return result;
        }
        [Route("InsertUpdateNurseStationhierarchyMaster")]
        [HttpPost]
        public int InsertUpdateNurseStationhierarchyMaster(NurseStationHierarchyEntity hierarchyentity)
        {
            this._log.Debug("---Executing InsertUpdateNurseStationhierarchyMaster() in CompanyController----");
            int result = this._companyService.InsertUpdateNurseStationhierarchyMaster(hierarchyentity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateNurseStationhierarchyMaster() in CompanyController----");
            return result;
        }
        [Route("GetCompanyToBedMapGrid/{facilityId?}/{NsId?}")]
        [HttpGet]
        public JsonResult<List<NurseStationHierarchyEntity>> GetCompanyToBedMapGrid(int? facilityId = null, int? NsId = null)
        {
            this._log.Debug("---Executing GetCompanyToBedMapGrid() in CompanyController----");
            var result = this._companyService.GetCompanyToBedMapGrid(facilityId,NsId).Result;
            this._log.Debug("---Executed Successfully GetCompanyToBedMapGrid() in CompanyController----");
            return Json<List<NurseStationHierarchyEntity>>(result);
        }
        [Route("GetNurseStationHierarchyDetailsById/{hierarchyId}")]
        [HttpGet]
        public JsonResult<NurseStationHierarchyEntity> GetNurseStationHierarchyDetailsById(int hierarchyId)
        {
            this._log.Debug("---Executing GetNurseStationHierarchyDetailsById() in CompanyController----");
            var result = this._companyService.GetNurseStationHierarchyDetailsById(hierarchyId).Result;
            this._log.Debug("---Executed Successfully GetNurseStationHierarchyDetailsById() in CompanyController----");
            return Json<NurseStationHierarchyEntity>(result);
        }
        [Route("GetNurseStationHierarchyDetailsbyNsId/{NsId?}")]
        [HttpGet]
        public JsonResult<NurseStationHierarchyEntity> GetNurseStationHierarchyDetailsbyNsId(int NsId)
        {
            this._log.Debug("---Executing GetNurseStationHierarchyDetailsbyNsId() in CompanyController----");
            var result = this._companyService.GetNurseStationHierarchyDetailsbyNsId(NsId).Result;
            this._log.Debug("---Executed Successfully GetNurseStationHierarchyDetailsbyNsId() in CompanyController----");
            return Json<NurseStationHierarchyEntity>(result);
        }
     
    }
}
