using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using System.Web.Http.Results;
using LTCPro.Entities;
using Newtonsoft.Json;
using System.Web;
using LTCPro.Repositories;
using WebApi.Filters;
using System.Collections;

namespace LTCPro.WebApi
{

    [CustomAuthorizationFilter]
    [CustomApiExceptionFilter]
    [RoutePrefix("ResidentDemographic")]
    public class ResidentDemographicController : ApiController
    {
        private readonly IResidentDemographicService _residentDemographicService;
        private readonly IAllergyInfoService _allergyInfoService;
        private readonly IDiagnosisInfoService _diagnosisInfoService;
        private readonly ILogger _log;

        public ResidentDemographicController(IResidentDemographicService residentDemographicService,
            IAllergyInfoService allergyInfoService, IDiagnosisInfoService diagnosisInfoService, ILogger log)
        {
            this._residentDemographicService = residentDemographicService;
            this._allergyInfoService = allergyInfoService;
            this._diagnosisInfoService = diagnosisInfoService;
            this._log = log;
        }
        //[Route("GetResidentGridData/{searchPattern}/{UserId}")]
        //[HttpGet]
        //public JsonResult<List<ResidentGridEntity>> GetResidentGridData(string searchPattern, int userId)
        //{
        //    this._log.Debug("---Executing GetResidentGridData() in ResidentDemographicController----");
        //    var residentGrid = this._residentDemographicService.GetResidentGridData(searchPattern, userId).Result;
        //    this._log.Debug("---Executed Successfully GetResidentGridData() in ResidentDemographicController----");
        //    return Json<List<ResidentGridEntity>>(residentGrid);
        //}
        [Route("GetResidentDropData/{UserId}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetResidentDropData(int userId)
        {
            this._log.Debug("---Executing GetResidentDropData() in ResidentDemographicController----");
            var residentDrop = this._residentDemographicService.GetResidentDropData(userId).Result;
            this._log.Debug("---Executed Successfully GetResidentDropData() in ResidentDemographicController----");
            return Json<List<ResidentDropEntity>>(residentDrop);
        }
        [Route("GetResidentAllergies/{patientID}")]
        [HttpGet]
        public JsonResult<List<AllergyInfoCustomEntity>> GetResidentAllergies(int patientId)
        {
            this._log.Debug("---Executing GetResidentAllergies() in ResidentDemographicController----");
            var residentAllergies = this._allergyInfoService.GetResidentAllergies(patientId).Result;
            this._log.Debug("---Executed Successfully GetResidentAllergies() in ResidentDemographicController----");
            return Json<List<AllergyInfoCustomEntity>>(residentAllergies);
        }
        [Route("GetResidentDiagnosisInfo/{patientID}")]
        [HttpGet]
        public JsonResult<List<DiagnosisInfoCustomEntity>> GetResidentDiagnosisInfo(int patientId)
        {
            this._log.Debug("---Executing GetResidentDiagnosisInfo() in ResidentDemographicController----");
            var residentDiagnosis = this._diagnosisInfoService.GetResidentDiagnosisInfo(patientId).Result;
            this._log.Debug("---Executed Successfully GetResidentDiagnosisInfo() in ResidentDemographicController----");
            return Json<List<DiagnosisInfoCustomEntity>>(residentDiagnosis);
        }
        [Route("GetResidentDemographicData/{patientId}")]
        [HttpGet]
        public JsonResult<DemographicEntity> GetResidentDemographicData(int patientId)
        {
            this._log.Debug("---Executing GetResidentDemographicData() in ResidentDemographicController----");
            var demographicdata = this._residentDemographicService.GetResidentDemographicData(patientId).Result;
            this._log.Debug("---Executed Successfully GetResidentDemographicData() in ResidentDemographicController----");
            return Json<DemographicEntity>(demographicdata);

        }

        [Route("GetResidentAdmitDischargeData/{patientID}")]
        [HttpGet]
        public JsonResult<List<ResidentAdmitDischargeEntity>> GetResidentAdmitDischargeData(int patientID)
        {
            this._log.Debug("---Executing GetResidentAdmitDischargeData() in ResidentDemographicController----");
            var residentGrid = this._residentDemographicService.GetResidentAdmitDischargeData(patientID).Result;
            this._log.Debug("---Executed Successfully GetResidentAdmitDischargeData() in ResidentDemographicController----");
            return Json<List<ResidentAdmitDischargeEntity>>(residentGrid);
        }
        [Route("GetResidentInformation/{patientID}")]
        [HttpGet]
        public JsonResult<DemographicInfoEntity> GetResidentInformation(int patientID)
        {
            try
            {
                this._log.Debug("---Executing GetResidentInformation() in ResidentDemographicController----");
                var residentGrid = this._residentDemographicService.GetResidentInformation(patientID).Result;
                this._log.Debug("---Executed Successfully GetResidentInformation() in ResidentDemographicController----");
                return Json<DemographicInfoEntity>(residentGrid);

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("GetLiteralOrdersData/{patientID}")]
        [HttpGet]
        public JsonResult<List<LiteralOrdersEntity>> GetLiteralOrdersData(int patientID)
        {
            this._log.Debug("---Executing GetLiteralOrdersData() in ResidentDemographicController----");
            var literalOrders = this._residentDemographicService.GetLiteralOrdersData(patientID).Result;
            this._log.Debug("---Executed Successfully GetLiteralOrdersData() in ResidentDemographicController----");
            return Json<List<LiteralOrdersEntity>>(literalOrders);
        }
        [Route("GetResidentOrderData/{patientID}")]
        [HttpGet]
        public JsonResult<List<ResidentLiteralOrderDataEntity>> GetResidentOrderData(int patientID)
        {
            this._log.Debug("---Executing GetResidentOrderData() in ResidentDemographicController----");
            var literalOrders = this._residentDemographicService.GetResidentOrderData(patientID).Result;
            this._log.Debug("---Executed Successfully GetResidentOrderData() in ResidentDemographicController----");
            return Json<List<ResidentLiteralOrderDataEntity>>(literalOrders);
        }

        [Route("GetMedications/{patientID}")]
        [HttpGet]
        public JsonResult<List<TreatmentInfoEntity>> GetMedications(int patientID)
        {
            this._log.Debug("---Executing GetMedications() in ResidentDemographicController----");
            var medications = this._residentDemographicService.GetMedications(patientID).Result;
            this._log.Debug("---Executed Successfully GetMedications() in ResidentDemographicController----");
            return Json<List<TreatmentInfoEntity>>(medications);
        }

        [Route("GetResidentsByCompanyBed")]
        [HttpPost]
        public JsonResult<ResidentGridEntity> GetResidentsByCompanyBed(string body)
        {
            this._log.Debug("---Executing GetResidentsByCompanyBed() in ResidentDemographicController----");
            CompanyBedConfigCustomEntity filterConfigs = JsonConvert.DeserializeObject<CompanyBedConfigCustomEntity>(body);
            var residents = this._residentDemographicService.GetResidentsByCompanyBed(filterConfigs).Result;
            this._log.Debug("---Executed Successfully GetResidentsByCompanyBed() in ResidentDemographicController----");
            return Json<ResidentGridEntity>(residents);
        }
        [Route("UploadResidentImage")]
        [HttpPost]
        public int UploadResidentImage(string body)
        {
            int residentID = Convert.ToInt16(body);
            this._log.Debug("---Executing UploadResidentImage() in ResidentDemographicController----");
            return this._residentDemographicService.UploadResidentImage(residentID).Result;

        }
        //[Route("InsertUpdateAllergyInfo")]
        //[HttpPost]
        //public int InsertUpdateAllergyInfo(AllergyInfoEntity entity)
        //{
        //    this._log.Debug("---Executing InsertUpdateAllergyInfo() in ResidentDemographicController----");
        //    return this._allergyInfoService.InsertUpdateAllergyInfo(entity).Result;

        //}
        //[Route("InsertUpdateDiagnosisInfo")]
        //[HttpPost]
        //public int InsertUpdateDiagnosisInfo(DiagnosisInfoCustomEntity entity)
        //{
        //    this._log.Debug("---Executing InsertUpdateDiagnosisInfo() in ResidentDemographicController----");
        //    return this._diagnosisInfoService.InsertUpdateDiagnosisInfo(entity).Result;

        //}



        [Route("GetResidentAdmitvisitInfo/{visitId}")]
        [HttpGet]
        public JsonResult<VisitInfoEntity> GetResidentAdmitvisitInfoData(int visitId)
        {
            this._log.Debug("---Executing GetResidentAdmitvisitInfoData() in ResidentDemographicController----");
            var visitinfo = this._residentDemographicService.GetResidentAdmitvisitInfoData(visitId).Result;
            this._log.Debug("---Executed Successfully GetResidentAdmitvisitInfoData() in ResidentDemographicController----");
            return Json<VisitInfoEntity>(visitinfo);
        }
        [Route("GetResidentLiteralordersData/{OrderId}")]
        [HttpGet]
        public JsonResult<CommonOrderInfoEntity> GetResidentLiteralordersData(int OrderId)
        {
            this._log.Debug("---Executing GetResidentLiteralordersData() in ResidentDemographicController----");
            var literalorders = this._residentDemographicService.GetResidentLiteralordersData(OrderId).Result;
            this._log.Debug("---Executed Successfully GetResidentLiteralordersData() in ResidentDemographicController----");
            return Json<CommonOrderInfoEntity>(literalorders);
        }
        //[Route("InsertResidentMedications")]
        //[HttpPost]
        //public int InsertResidentMedications(TreatmentInfoEntity medications)
        //{
        //    this._log.Debug("---Executing InsertResidentMedications() in ResidentDemographicController----");
        //    var result = this._residentDemographicService.InsertResidentMedications(medications).Result;
        //    this._log.Debug("---Executed Successfully InsertResidentMedications() in ResidentDemographicController----");
        //    return result;
        //}

        //[Route("InsertResidentAdmitVistiInfoData")]
        //[HttpPost]
        //public int InsertResidentAdmitVistiInfoData(VisitInfoEntity admitvisitinfo)
        //{
        //    this._log.Debug("---Executing InsertResidentAdmitVistiInfoData() in ResidentDemographicController----");
        //    var result = this._residentDemographicService.InsertResidentAdmitVistiInfoData(admitvisitinfo).Result;
        //    this._log.Debug("---Executed Successfully InsertResidentAdmitVistiInfoData() in ResidentDemographicController----");
        //    return result;
        //}

        [Route("InsertResidentDemographicData")]
        [HttpPost]
        public int InsertResidentDemographicData(DemographicEntity demographics)
        {
            this._log.Debug("---Executing InsertResidentDemographicData() in ResidentDemographicController----");
            var result = this._residentDemographicService.InsertResidentDemographicData(demographics).Result;
            this._log.Debug("---Executed Successfully InsertResidentDemographicData() in ResidentDemographicController----");
            return result;

        }
        //[Route("InsertResidentLiteralOrdersData")]
        //[HttpPost]
        //public int InsertResidentLiteralOrdersData(CommonOrderInfoEntity literalorders)
        //{
        //    this._log.Debug("---Executing InsertResidentLiteralOrdersData() in ResidentDemographicController----");
        //    var result = this._residentDemographicService.InsertResidentLiteralOrdersData(literalorders).Result;
        //    this._log.Debug("---Executed Successfully InsertResidentLiteralOrdersData() in ResidentDemographicController----");
        //    return result;


        //}
        [Route("GetResidentsCount/{UserId}")]
        [HttpGet]
        public JsonResult<ResidentCountCustomEntity> GetResidentsCount(int userId)
        {
            this._log.Debug("---Executing GetResidentsCount() in ResidentDemographicController----");
            var residentsCount = this._residentDemographicService.GetResidentsCount(userId).Result;
            this._log.Debug("---Executed Successfully GetResidentsCount() in ResidentDemographicController----");
            return Json<ResidentCountCustomEntity>(residentsCount);
        }

        [Route("GetResidentMedications/{TreatmentId}")]
        [HttpGet]
        public JsonResult<TreatmentInfoEntity> GetResidentMedications(int TreatmentId)
        {
            this._log.Debug("---Executing GetResidentMedications() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetResidentMedications(TreatmentId).Result;
            this._log.Debug("---Executed Successfully GetResidentMedications() in ResidentDemographicController----");
            return Json<TreatmentInfoEntity>(result);
        }
        [Route("GetFacilityNSResidentsDataByPId/{PatientId}")]
        [HttpGet]
        public JsonResult<ResidentDataEntity> GetFacilityNSResidentsDataByPId(int patientId)
        {
            this._log.Debug("---Executing GetFacilityNSResidentsDataByPId() in ResidentDemographicController----");
            var data = this._residentDemographicService.GetFacilityNSResidentsDataByPId(patientId).Result;
            this._log.Debug("---Executed Successfully GetFacilityNSResidentsDataByPId() in ResidentDemographicController----");
            return Json<ResidentDataEntity>(data);
        }
        [Route("GetResidentsByNSId/{NurseStationId}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetResidentsByNSId(int nurseStationId)
        {
            this._log.Debug("---Executing GetResidentsByNSId() in ResidentDemographicController----");
            var residentGrid = this._residentDemographicService.GetResidentsByNSId(nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetResidentsByNSId() in ResidentDemographicController----");
            return Json<List<ResidentDropEntity>>(residentGrid);
        }
        [Route("GetResidentsByNSIds/{NurseStationIds}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetResidentsByNSIds(string nurseStationIds)
        {
            this._log.Debug("---Executing GetResidentsByNSIds() in ResidentDemographicController----");
            var residentGrid = this._residentDemographicService.GetResidentsByNSIds(nurseStationIds).Result;
            this._log.Debug("---Executed Successfully GetResidentsByNSIds() in ResidentDemographicController----");
            return Json<List<ResidentDropEntity>>(residentGrid);
        }
        [Route("GetAllegyDetails/{AllergyId}")]
        [HttpGet]
        public JsonResult<AllergyInfoEntity> GetAllegyDetails(int AllergyId)
        {
            this._log.Debug("---Executing GetAllegyDetails in ResidentDemographicController----");
            var result = this._allergyInfoService.GetAllegyDetails(AllergyId).Result;
            this._log.Debug("---Executed Successfully GetAllegyDetails() in ResidentDemographicController----");
            return Json<AllergyInfoEntity>(result);
        }
        [Route("GetDiagnosisDetails/{DiagnosisId}")]
        [HttpGet]
        public JsonResult<DiagnosisInfoEntity> GetDiagnosisDetails(int DiagnosisId)
        {
            try
            {
                this._log.Debug("---Executing GetDiagnosisDetails in ResidentDemographicController----");
                var result = this._diagnosisInfoService.GetDiagnosisDetails(DiagnosisId).Result;
                this._log.Debug("---Executed Successfully GetDiagnosisDetails() in ResidentDemographicController----");
                return Json<DiagnosisInfoEntity>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("UpdateResidentInfo")]
        [HttpPost]
        public int UpdateResidentInfo(VisitUpdateEntity residentinfo)
        {
            this._log.Debug("---Executing UpdateResidentInfo() in ResidentDemographicController----");
            var result = this._residentDemographicService.UpdateResidentInfo(residentinfo).Result;
            this._log.Debug("---Executed Successfully UpdateResidentInfo() in ResidentDemographicController----");
            return result;
        }
        [Route("GetNurseStationName/{patientId}")]
        [HttpGet]
        public string GetNurseStationName(int patientId)
        {
            this._log.Debug("---Executing GetNurseStationName() in ResidentDemographicController----");
            string result = this._residentDemographicService.GetNurseStationName(patientId).Result;
            this._log.Debug("---Executed Successfully GetNurseStationName() in ResidentDemographicController----");
            return result;
        }
        [Route("GetResidentOrdersView/{orderType}/{orderId}/{quantityId}")]
        [HttpGet]
        public JsonResult<OrdersViewEntity> GetResidentOrdersView(int orderType, Int64 orderId, Int64 quantityId)
        {
            this._log.Debug("---Executing GetResidentOrdersView() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetResidentOrdersView(orderType, orderId, quantityId).Result;
            this._log.Debug("---Executed Successfully GetResidentOrdersView() in ResidentDemographicController----");
            return Json<OrdersViewEntity>(result);
        }
        [Route("InsertPatientType")]
        [HttpPost]
        public JsonResult<int> InsertPatientType(ColourTypeEntity entity)
        {
            this._log.Debug("---Executing InsertPatientType() in ResidentDemographicController----");
            var result = this._residentDemographicService.InsertPatientType(entity).Result;
            this._log.Debug("---Executed Successfully InsertPatientType() in ResidentDemographicController----");
            return Json<int>(result);
        }
        [Route("GetPatientType/{patientId}")]
        [HttpGet]
        public JsonResult<List<PatientColorTypeEntity>> GetPatientType(int patientId)
        {
            this._log.Debug("---Executing GetPatientType() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetPatientType(patientId).Result;
            this._log.Debug("---Executed Successfully GetPatientType() in ResidentDemographicController----");
            return Json<List<PatientColorTypeEntity>>(result);
        }
        [Route("GetNurseStationByPId/{patientId}")]
        [HttpGet]
        public JsonResult<int> GetNurseStationByPId(int patientId)
        {
            this._log.Debug("---Executing GetNurseStationByPId() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetNurseStationByPId(patientId).Result;
            this._log.Debug("---Executed Successfully GetNurseStationByPId() in ResidentDemographicController----");
            return Json<int>(result);
        }
        [Route("GetAllPatientTypesByPId/{patientId}")]
        [HttpGet]
        public JsonResult<List<PatientTypeEntity>> GetAllPatientTypesByPId(int patientId)
        {
            this._log.Debug("---Executing GetAllPatientTypesByPId() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetAllPatientTypesByPId(patientId).Result;
            this._log.Debug("---Executed Successfully GetAllPatientTypesByPId() in ResidentDemographicController----");
            return Json<List<PatientTypeEntity>>(result);
        }
        [Route("InsertUpdatePhyscianDetails")]
        [HttpPost]
        public int InsertUpdatePhyscianDetails(PhysicianDetailsEntity physcianDetails)
        {
            this._log.Debug("---Exceuting successfully InsertUpdatePhyscianDetails()---");
            var result = this._residentDemographicService.InsertUpdatePhyscianDetails(physcianDetails).Result;
            this._log.Debug("---Executed successfully InsertUpdatePhyscianDetails() in ResidentDemographicController---");
            return result;
        }
        [Route("GetPhyscianDetails/{userId}")]
        [HttpGet]
        public JsonResult<List<PhysicianGridEntity>> GetPhyscianDetails(int userId)
        {
            this._log.Debug("---Executing successfully GetPhyscianDetails() in ResidentDemographicController---");
            var result = this._residentDemographicService.GetPhyscianDetails(userId).Result;
            this._log.Debug("---Executed successfully GetPhyscianDetails() in ResidentDemographicController--- ");
            return Json<List<PhysicianGridEntity>>(result);
        }
        [Route("GetPhyscianDetailsGridDataNew/{userId}")]
        [HttpGet]
        public JsonResult<List<PhysicianGridEntityNew>> GetPhyscianDetailsGridDataNew(int userId)
        {
            this._log.Debug("---Executing successfully GetPhyscianDetailsGridDataNew() in ResidentDemographicController---");
            var result = this._residentDemographicService.GetPhyscianDetailsGridDataNew(userId).Result;
            this._log.Debug("---Executed successfully GetPhyscianDetailsGridDataNew() in ResidentDemographicController--- ");
            return Json<List<PhysicianGridEntityNew>>(result);
        }
        [Route("GetPhyscianDetailsById/{PhyscianNPI}/{FacilityId}")]
        [HttpGet]
        public JsonResult<PhysicianDetailsEntity> GetPhyscianDetailsById(string PhyscianNPI,string FacilityId)
        {
            this._log.Debug("---Executing Successfully GetPhyscianDetailsById in ResidentDemographic Controller--- ");
            var result = this._residentDemographicService.GetPhyscianDetailsById(PhyscianNPI, FacilityId).Result;
            this._log.Debug("---Executed  Successfully GetPhyscianDetailsById in ResidentDemographic Controller--- ");
            return Json<PhysicianDetailsEntity>(result);
        }
        [Route("CheckResidentUniqueId/{patientId}")]
        [HttpGet]
        public string CheckResidentUniqueId(int patientId)
        {
            this._log.Debug("---Executing CheckResidentUniqueId() in ResidentDemographicController----");
            string result = this._residentDemographicService.CheckResidentUniqueId(patientId).Result;
            this._log.Debug("---Executed Successfully CheckResidentUniqueId() in ResidentDemographicController----");
            return result;
        }
        [Route("InsertUpdatePatientOnLeave")]
        [HttpPost]
        public int InsertUpdatePatientOnLeave(OnLeaveEntity entity)
        {
            this._log.Debug("---Executing  InsertUpdatePatientOnLeave()  in ResidentDemographicController----");
            int result = this._residentDemographicService.InsertUpdatePatientOnLeave(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdatePatientOnLeave() in ResidentDemographicController----");
            return result;
        }
        [Route("UpdatePhysiciansStatus")]
        [HttpPost]
        public int UpdatePhysiciansStatus(List<PhysicianDetailsEntity> data)
        {
            this._log.Debug("---Executing UpdatePhysiciansStatus() in ResidentDemographicController----");
            var result = this._residentDemographicService.UpdatePhysiciansStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdatePhysiciansStatus() in ResidentDemographicController----");
            return result;
        }
        [Route("RemoveResidentAllergiesInfo/{pAllergyId}")]
        [HttpGet]
        public int RemoveResidentAllergiesInfo(int pAllergyId)
        {
            this._log.Debug("---Executing RemoveResidentAllergiesInfo() in ResidentDemographicController----");
            var result = this._allergyInfoService.RemoveResidentAllergiesInfo(pAllergyId).Result;
            this._log.Debug("---Executed Successfully RemoveResidentAllergiesInfo() in ResidentDemographicController----");
            return result;
        }
        [Route("GetResidentDetails/{status}/{nursestationId}/{residentcount?}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetResidentDetails(Nullable<int> status, string nursestationId, int residentcount = 0)
        {
            this._log.Debug("---Executing successfully GetResidentDetails() in ResidentDemographicController---");
            var result = this._residentDemographicService.GetResidentDetails(status, nursestationId, residentcount).Result;
            this._log.Debug("---Executed successfully GetResidentDetails() in ResidentDemographicController--- ");
            return Json<List<ResidentDropEntity>>(result);
        }
        [Route("RemoveResidentDiagnosis/{pDiagnosisId}")]
        [HttpGet]
        public int RemoveResidentDiagnosis(int pDiagnosisId)
        {
            this._log.Debug("---Executing RemoveResidentDiagnosis() in ResidentDemographicController----");
            var result = this._diagnosisInfoService.RemoveResidentDiagnosis(pDiagnosisId).Result;
            this._log.Debug("---Executed Successfully RemoveResidentDiagnosis() in ResidentDemographicController----");
            return result;
        }
        [Route("CheckResidentInternalId/{InternalId}")]
        [HttpGet]
        public int CheckResidentInternalId(string InternalId)
        {
            this._log.Debug("---Executing CheckResidentInternalId() in ResidentDemographicController----");
            var result = this._residentDemographicService.CheckResidentInternalId(InternalId).Result;
            this._log.Debug("---Executed Successfully CheckResidentInternalId() in ResidentDemographicController----");
            return result;
        }
        [Route("InsertMergeStatus")]
        [HttpPost]
        public JsonResult<int> InsertMergeStatus(PostMergeDetails objPost)
        {
            this._log.Debug("---Executing InsertMergeStatus() in ResidentDemographicController----");
            var result = this._residentDemographicService.InsertMergeStatus(objPost).Result;
            this._log.Debug("---Executed Successfully InsertMergeStatus() in ResidentDemographicController----");
            return Json<int>(result);
        }
        [Route("GetMergeDetails/{PatientID}/{MergePatientID}")]
        [HttpGet]
        public JsonResult<ResidentMergeEntity> GetMergeDetails(int PatientID, int MergePatientID)
        {
            this._log.Debug("---Executing GetMergeDetails() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetMergeDetails(PatientID, MergePatientID).Result;
            this._log.Debug("---Executed Successfully GetMergeDetails() in ResidentDemographicController----");
            return Json<ResidentMergeEntity>(result);
        }
        [Route("GetResidentMergeDetailsByPatientID/{PatientID}")]
        [HttpGet]
        public JsonResult<ResidentMergeDetails> GetResidentMergeDetailsByPatientID(int PatientID)
        {
            this._log.Debug("---Executing GetResidentMergeDetailsByPatientID() in ResidentDemographicController----");
            var result = this._residentDemographicService.GetResidentMergeDetailsByPatientID(PatientID).Result;
            this._log.Debug("---Executed Successfully GetResidentMergeDetailsByPatientID() in ResidentDemographicController----");
            return Json<ResidentMergeDetails>(result);
        }
        [Route("GetHl7PendingCount")]
        [HttpGet]
        public int GetHl7PendingCount()
        {
            this._log.Debug("---Executing GetHl7PendingCount() in ResidentDemographicController----");
            return this._residentDemographicService.GetHl7PendingCount().Result;
        }
        [Route("GetComputerNameDropData")]
        [HttpGet]
        public JsonResult<IList> GetComputerNameDropData()
        {
            this._log.Debug("---Executing GetComputerNameDropData() in UserController----");
            var Computernamemaster = this._residentDemographicService.GetComputerNameDropData().Result;
            this._log.Debug("---Executed Successfully GetComputerNameDropData() in UserController----");
            return Json<IList>(Computernamemaster);
        }
        [Route("GetResidentActiveStatus/{mrNumber}")]
        [HttpGet]
        public JsonResult<string> GetResidentActiveStatus(string mrNumber)
        {
            this._log.Debug("---Executing GetResidentActiveStatus() in UserController----");
            var Computernamemaster = this._residentDemographicService.GetResidentActiveStatus(mrNumber).Result;
            this._log.Debug("---Executed Successfully GetResidentActiveStatus() in UserController----");
            return Json<string>(Computernamemaster);
        }
        [Route("GetUserProcessKeyByID/{userId}")]
        [HttpGet]
        public JsonResult<string> GetUserProcessKeyByID(int userId)
        {
            this._log.Debug("---Executing GetUserProcessKeyByID() in UserController----");
            var Computernamemaster = this._residentDemographicService.GetUserProcessKeyByID(userId).Result;
            this._log.Debug("---Executed Successfully GetUserProcessKeyByID() in UserController----");
            return Json<string>(Computernamemaster);
        }
        [Route("GetCertifyOrderResidentGridData/{nursingStations}/{userId}/{phyNpi}")]
        [HttpGet]
        public JsonResult<List<ResidentsEntity>> GetCertifyOrderResidentGridData(string nursingStations, int userId, string phyNpi)

        {
            this._log.Debug("---Executing GetCertifyOrderResidentGridData() in ResidentDemographicController----");
            var list = this._residentDemographicService.GetCertifyOrderResidentGridData(nursingStations, userId, phyNpi).Result;
            this._log.Debug("---Executed Successfully GetCertifyOrderResidentGridData() in ResidentDemographicController----");
            return Json<List<ResidentsEntity>>(list);
        }
        [Route("GetCertifyOrderGridData/{patientId}/{phyNpi}")]
        [HttpGet]
        public JsonResult<IList> GetCertifyOrderGridData(int patientId, string phyNpi)
        {
            this._log.Debug("---Executing GetCertifyOrderGridData() in ResidentDemographicController----");
            var list = this._residentDemographicService.GetCertifyOrderGridData(patientId, phyNpi).Result;
            this._log.Debug("---Executed Successfully GetCertifyOrderGridData() in ResidentDemographicController----");
            return Json<IList>(list);
        }
        [Route("GetResidentsListByNSId/{nurseStationId}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetResidentsListByNSId(int nurseStationId)
        {
            this._log.Debug("---Executing GetResidentsListByNSId() in AssesmentController----");
            var records = this._residentDemographicService.GetResidentsListByNSId(nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetResidentsListByNSId() in AssesmentController----");
            return Json<List<ResidentDropEntity>>(records);
        }
        [Route("GetProfileCertifyOrderResidentGridData/{nursingStations}")]
        [HttpGet]
        public JsonResult<List<ResidentsEntity>> GetProfileCertifyOrderResidentGridData(string nursingStations)

        {
            this._log.Debug("---Executing GetProfileCertifyOrderResidentGridData() in ResidentDemographicController----");
            var list = this._residentDemographicService.GetProfileCertifyOrderResidentGridData(nursingStations).Result;
            this._log.Debug("---Executed Successfully GetProfileCertifyOrderResidentGridData() in ResidentDemographicController----");
            return Json<List<ResidentsEntity>>(list);
        }
        [Route("GetProfileCertifyOrderGridData/{patientId}")]
        [HttpGet]
        public JsonResult<IList> GetProfileCertifyOrderGridData(int patientId)
        {
            this._log.Debug("---Executing GetProfileCertifyOrderGridData() in ResidentDemographicController----");
            var list = this._residentDemographicService.GetProfileCertifyOrderGridData(patientId).Result;
            this._log.Debug("---Executed Successfully GetProfileCertifyOrderGridData() in ResidentDemographicController----");
            return Json<IList>(list);
        }
        [Route("GetDefaultPhysicianNursestation/{PhysicianNPI}")]
        [HttpGet]
        public JsonResult<string> GetDefaultPhysicianNursestation(string PhysicianNPI)
        {
            this._log.Debug("---Executing GetDefaultPhysicianNursestation() in ResidentDemographicController----");
            var list = this._residentDemographicService.GetDefaultPhysicianNursestation(PhysicianNPI).Result;
            this._log.Debug("---Executed Successfully GetDefaultPhysicianNursestation() in ResidentDemographicController----");
            return Json<string>(list);
        }


        [Route("UpdatePregnecyFeeding")]
        [HttpPost]
        public int UpdatePregnecyFeeding(UpdatePregnecyFeedingEntity Role)
        {
            this._log.Debug("---Executing UpdatePregnecyFeeding in ResidentDemographicController----");
            int result = this._residentDemographicService.UpdatePregnecyFeeding(Role).Result;
            this._log.Debug("---Executed Successfully UpdatePregnecyFeeding in ResidentDemographicController----");
            return 0;
        }

    }
}