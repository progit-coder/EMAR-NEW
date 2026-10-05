using System;
using System.Collections;
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
    [RoutePrefix("Emar")]
    [CustomAuthorizationFilter]
    public class EmarController : ApiController
    {
        private readonly IEmarService _emarService;
        private readonly ILogger _log;
        public EmarController(IEmarService emarService, ILogger log)
        {
            this._emarService = emarService;
            this._log = log;
        }
        [Route("GetEmarResidentGridData/{nurseStationId}/{Time}/{dateValue}/{NurseStationShiftId}/{ShowTwoHours}")]
        [HttpGet]
        public JsonResult<EmarData> GetEmarResidentGridData(int nurseStationId, string time, string dateValue, int NurseStationShiftId, int showTwoHours)
        {
            try
            {
                this._log.Debug("---Executing GetEmarResidentGridData() in EmarController----");
                var emarGridData = this._emarService.GetEmarResidentGridData(time, dateValue, nurseStationId, NurseStationShiftId, showTwoHours).Result;
                this._log.Debug("---Executed Successfully GetEmarResidentGridData() in EmarController----");
                return Json<EmarData>(emarGridData);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("GetEmarResidentBarcodes/{dateValue}/{patientId}")]
        [HttpGet]
        public IHttpActionResult GetEmarResidentBarcodes(string dateValue, int patientId)
        {
            try
            {
                this._log.Debug("---Executing GetEmarResidentBarcodes() in EmarController----");
                var BarcodeListData = this._emarService.GetEmarResidentBarcodes(dateValue, patientId).Result;
                this._log.Debug("---Executed Successfully GetEmarResidentBarcodes() in EmarController----");
                return Json<IList>(BarcodeListData);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("GetNursingScheduleData/{NurseStationID}/{ScheduleDate}")]
        [HttpGet]
        public JsonResult<List<NursingScheduleDropEntity>> GetNursingScheduleData(int nurseStationId, string scheduleDate)
        {
            this._log.Debug("---Executing GetNursingScheduleData() in EmarController----");
            //var nursingData = this._emarService.GetNursingScheduleData(nurseStationId, scheduleDate).Result;
            var nursingData = this._emarService.GetNursingScheduleData(nurseStationId, scheduleDate).GetAwaiter().GetResult();
            this._log.Debug("---Executed Successfully GetNursingScheduleData() in EmarController----");
            return Json<List<NursingScheduleDropEntity>>(nursingData);
        }
        [Route("GetNursingScheduleDataByPatientID/{PatientID}/{nursingStationId}/{ScheduleDate}")]
        [HttpGet]
        public JsonResult<List<NursingScheduleDropEntity>> GetNursingScheduleDataByPatientID(int patientId, int nursingStationId, string scheduleDate)
        {
            this._log.Debug("---Executing GetNursingScheduleData() in EmarController----");
            var nursingData = this._emarService.GetNursingScheduleDataByPatientID(patientId, scheduleDate, nursingStationId).Result;
            this._log.Debug("---Executed Successfully GetNursingScheduleData() in EmarController----");
            return Json<List<NursingScheduleDropEntity>>(nursingData);
        }
        [Route("GetResidentBiometricInfo/{PatientID}")]
        [HttpGet]
        public string GetResidentBiometricInfo(int PatientID)
        {
            this._log.Debug("---Executing GetResidentBiometricInfo() in OrdersController----");
            var biometricData = this._emarService.GetResidentBiometricInfo(PatientID).Result;
            this._log.Debug("---Executed Successfully GetResidentBiometricInfo() in OrdersController----");
            return biometricData;
        }
        [Route("GetEmarOrdersList/{PatientID}/{Time}/{dateValue}/{ShiftId}/{Window}/{userId}")]
        [HttpGet]
        public JsonResult<List<EmarOrdersListEntity>> GetEmarOrdersList(int patientId, string time, string dateValue, int shiftId, int window, int userId)
        {
            this._log.Debug("---Executing GetEmarOrdersList() in EmarController----");
            this._log.Debug("---Executing GetEmarOrdersList() in Slow Loading Test Start----"+userId.ToString()+"_" + DateTime.Now.ToString());
            var OrdersData = this._emarService.GetEmarOrdersList(patientId, time, dateValue, shiftId, window, userId).Result;
            //var OrdersData = this._emarService.GetEmarOrdersList(patientId, time, dateValue, shiftId, window, userId).GetAwaiter().GetResult();
            this._log.Debug("---Executed Successfully GetEmarOrdersList() in EmarController----");
            this._log.Debug("---Executing GetEmarOrdersList() in Slow Loading Test end----" + userId.ToString() + "_" + DateTime.Now.ToString());
            return Json<List<EmarOrdersListEntity>>(OrdersData);
        }
         [Route("GetEmarRefillNotes/{orderId}")]
        [HttpGet]
        public JsonResult<RefillDataEntity> GetEmarRefillNotes(int orderId)
        {
            try
            {
                this._log.Debug("---Executing GetEmarRefillNotes() in EmarController----");
                var result = this._emarService.GetEmarRefillNotes(orderId).Result;
                this._log.Debug("---Executed Successfully GetEmarRefillNotes() in EmarController----");
                return Json<RefillDataEntity>(result);
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        [Route("InsertDrugAdminister")]
        [HttpPost]
        public int InsertDrugAdminister(DrugAdministerEntity drugAdministar)
        {
            try
            {
                this._log.Debug("---Executing InsertDrugAdminister() in EmarController----");
                var result = this._emarService.InsertDrugAdminister(drugAdministar).Result;
                this._log.Debug("---Executed Successfully InsertDrugAdminister() in EmarController----");
                return result;
            }
            catch (Exception ex)
            {
                this._log.Debug("Exception: " + ex.Message + " \n Inner Exception" + ex.InnerException);
                this._log.Debug("Parameters of method: " + drugAdministar);
                return 0;
            }
        }
        [Route("InsertNursingFrequencyConfig")]
        [HttpPost]
        public int InsertNursingFrequencyConfig(NursingFrequencyConfigCustomEntity frequencyconfig)
        {
            this._log.Debug("---Executing InsertNursingFrequencyConfig() in EmarController----");
            var result = this._emarService.InsertNursingFrequencyConfig(frequencyconfig).Result;
            this._log.Debug("---Executed Successfully InsertNursingFrequencyConfig() in EmarController----");
            return result;

        }
        [Route("GetNursingFrequencyConfigsData/{UserId}")]
        [HttpGet]
        public JsonResult<List<PrcGetFreqMappingData_ResultEntity>> GetNursingFrequencyConfigsData(int userId)
        {
            this._log.Debug("---Executing GetNursingFrequencyConfigsData() in EmarController----");
            var result = this._emarService.GetNursingFrequencyConfigsData(userId).Result;
            this._log.Debug("---Executed Successfully GetNursingFrequencyConfigsData() in EmarController----");
            return Json<List<PrcGetFreqMappingData_ResultEntity>>(result);
        }
        [Route("GetMedicationReason")]
        [HttpGet]


        public JsonResult<List<MedicationReasonEntity>> GetMedicationReason()
        {
            this._log.Debug("---Executing GetMedicationReason() in EmarController----");
            var result = this._emarService.GetMedicationReason().Result;
            this._log.Debug("---Executed Successfully GetMedicationReason() in EmarController----");
            return Json<List<MedicationReasonEntity>>(result);
        }
        [Route("NursingFreqencyConfig/{NursingFreqId}")]
        [HttpGet]
        public JsonResult<NursingFrequencyConfigCustomEntity> GetNursingFrequencyDetailsByID(int nursingFreqId)
        {
            this._log.Debug("---Executing GetNursingFrequencyDetailsByID() in EmarController----");
            var nurseFreq = this._emarService.GetNursingFrequencyDetailsByID(nursingFreqId).Result;
            this._log.Debug("---Executed Successfully GetNursingFrequencyDetailsByID() in EmarController----");
            return Json<NursingFrequencyConfigCustomEntity>(nurseFreq);
        }
        [Route("GetPRNData")]
        [HttpPost]
        public JsonResult<List<PRNDetailsCustomEntity>> GetPRNData(PRNFilterCustomEntity configs)
        {
            this._log.Debug("---Executing GetPRNData() in EmarController----");
            var result = this._emarService.GetPRNData(configs).Result;
            this._log.Debug("---Executed Successfully GetPRNData() in EmarController----");
            return Json<List<PRNDetailsCustomEntity>>(result);
        }
        [Route("GetSeventyTwoHourCheckDetails")]
        [HttpPost]
        public JsonResult<List<SeventyTwoHourCheckEntity>> GetSeventyTwoHourCheckDetails(SeventyTwoHourCustomEntity configs)
        {
            this._log.Debug("---Executing GetSeventyTwoHourCheckDetails() in EMARController----");
            var data = this._emarService.GetSeventyTwoHourCheckDetails(configs).Result;
            this._log.Debug("---Executed Successfully GetSeventyTwoHourCheckDetails() in EMARController----");
            return Json<List<SeventyTwoHourCheckEntity>>(data);
        }
        [Route("InsertPRNData")]
        [HttpPost]
        public int InsertPRNData(List<PRNInsertCustomEntity> prnData)
        {
            this._log.Debug("---Executing InsertPRNData() in EmarController----");
            var result = this._emarService.InsertPRNData(prnData).Result;
            this._log.Debug("---Executed Successfully InsertPRNData() in EmarController----");
            return result;
        }
        [Route("InsertUpdateSeventyTwoHourChecks")]
        [HttpPost]
        public int InsertUpdateSeventyTwoHourChecks(List<SeventyTwoHourInsertEntity> entity)
        {
            this._log.Debug("---Executing InsertUpdateSeventyTwoHourChecks() in EMARController----");
            int result = this._emarService.InsertUpdateSeventyTwoHourChecks(entity).Result;
            this._log.Debug("---Executed Successfully InsertUpdateSeventyTwoHourChecks() in EMARController----");
            return result;

        }

        [Route("GetSeventyTwoHourDetailsByfilter")]
        [HttpPost]
        public JsonResult<List<SeventyTwoHourCheckEntity>> GetSeventyTwoHourDetailsByfilter(SeventyTwoHourCustomEntity configs)
        {
            this._log.Debug("---Executing GetSeventyTwoHourDetailsByfilter() in EMARController----");
            var filterData = this._emarService.GetSeventyTwoHourDetailsByfilter(configs).Result;
            this._log.Debug("---Executed Successfully GetSeventyTwoHourDetailsByfilter() in EMARController----");
            return Json<List<SeventyTwoHourCheckEntity>>(filterData);
        }
        [Route("GetPRNDetailsByfilter")]
        [HttpPost]
        public JsonResult<List<PRNDetailsCustomEntity>> GetPRNDetailsByfilter(PRNFilterCustomEntity configs)
        {
            this._log.Debug("---Executing GetPRNDetailsByfilter() in EMARController----");
            var filterData = this._emarService.GetPRNDetailsByfilter(configs).Result;
            this._log.Debug("---Executed Successfully GetPRNDetailsByfilter() in EMARController----");
            return Json<List<PRNDetailsCustomEntity>>(filterData);
        }
        [Route("ByPassBiometric")]
        [HttpPost]
        public int ByPassBiometric(BypassBiometricEntity byPass)
        {
            this._log.Debug("---Executing ByPassBiometric() in EMARController----");
            var result = this._emarService.ByPassBiometric(byPass).Result;
            this._log.Debug("---Executed Successfully ByPassBiometric() in EMARController----");
            return result;
        }
        [Route("InsertAdministredwithoutscanning")]
        [HttpPost]
        public int InsertAdministredwithoutscanning(NurseCommentsEntity nurseobj)
        {
            this._log.Debug("---Executing InsertAdministredwithoutscanning() in EMARController----");
            var result = this._emarService.InsertAdministredwithoutscanning(nurseobj).Result;
            this._log.Debug("---Executed Successfully InsertAdministredwithoutscanning() in EMARController----");
            return result;
        }
        [Route("GetVitalsCheckList/{PQuantityId}")]
        [HttpGet]
        public List<VitalsCheckEntity> GetVitalsCheckList(int PQuantityId)
        {
            this._log.Debug("---Executing GetVitalsCheckList() in EMARController----");
            var result = this._emarService.GetVitalsCheckList(PQuantityId).Result;
            this._log.Debug("---Executed Successfully GetVitalsCheckList() in EMARController----");
            return result;
        }
        [Route("GetVitalsChecksListbyQuantityIds/{PQuantityIds}")]
        [HttpGet]
        public List<VitalsCheckEntity> GetVitalsChecksListbyQuantityIds(string PQuantityIds)
        {
            this._log.Debug("---Executing GetVitalsChecksListbyQuantityIds() in EMARController----");
            var result = this._emarService.GetVitalsChecksListbyQuantityIds(PQuantityIds).Result;
            this._log.Debug("---Executed Successfully GetVitalsChecksListbyQuantityIds() in EMARController----");
            return result;
        }
        [Route("InsertOrderFavouritesData")]
        [HttpPost]
        public int InsertOrderFavouritesData(List<OrderFavouriteDataEntity> entity)
        {
            this._log.Debug("---Executing InsertOrderFavouritesData() in EmarController----");
            var result = this._emarService.InsertOrderFavouritesData(entity).Result;
            this._log.Debug("---Executed Successfully InsertOrderFavouritesData() in EmarController----");
            return result;
        }
        [Route("GetDueMARAlertData/{userId}")]
        [HttpGet]
        public int GetDueMARAlertData(int userId)
        {
            this._log.Debug("---Executing GetDueMARAlertData() in EmarController----");
            //var result = this._emarService.GetDueMARAlertData(userId).Result;
            var result = this._emarService.GetDueMARAlertData(userId).GetAwaiter().GetResult();
            this._log.Debug("---Executed Successfully GetDueMARAlertData() in EmarController----");
            return result;
        }
        [Route("GetEkitDropData/{nursestaionId}/{orderId}/{userId}")]
        [HttpGet]
        public JsonResult<List<EkitDropEntity>> GetEkitDropData(int nursestaionId, int orderId, int userId)
        {
            this._log.Debug("---Executing GetEkitDropData() in CommonController----");
            var ekit = this._emarService.GetEkitDropData(nursestaionId, orderId, userId).Result;
            this._log.Debug("---Executed Successfully GetEkitDropData() in CommonController----");
            return Json<List<EkitDropEntity>>(ekit);
        }
        [Route("GetNurseShiftDrop/{nursestationId}/{scheduleDate}")]
        [HttpGet]
        public JsonResult<List<NurseShiftsEntity>> GetNurseShiftDrop(int nursestationId, string scheduleDate)
        {
            this._log.Debug("---Executing GetNurseShiftDrop() in CommonController----");
            var result = this._emarService.GetNurseShiftDrop(nursestationId, scheduleDate).Result;
            this._log.Debug("---Executed Successfully GetNurseShiftDrop() in CommonController----");
            return Json<List<NurseShiftsEntity>>(result);
        }
        [Route("GetControlSubstanceTrans/{quantityId}")]
        [HttpGet]
        public JsonResult<List<ControlSubstanceTransEntity>> GetControlSubstanceTrans(int quantityId)
        {
            this._log.Debug("---Executing GetControlSubstanceTrans() in CommonController----");
            var result = this._emarService.GetControlSubstanceTrans(quantityId).Result;
            this._log.Debug("---Executed Successfully GetNurseShiftDrop() in CommonController----");
            return Json<List<ControlSubstanceTransEntity>>(result);
        }
        [Route("GetPRNAdministerCountByDate/{orderId}/{quantityd}/{dateValue}")]
        [HttpGet]
        public JsonResult<int> GetPRNAdministerCountByDate(int orderId, int quantityd, string dateValue)
        {
            this._log.Debug("---Executing GetPRNAdministerCountByDate() in CommonController----");
            var result = this._emarService.GetPRNAdministerCountByDate(orderId, quantityd, dateValue).Result;
            this._log.Debug("---Executed Successfully GetPRNAdministerCountByDate() in CommonController----");
            return Json<int>(result);
        }
        [Route("GetInsulinCommentsByQuantityId/{quantityId}")]
        [HttpGet]
        public JsonResult<string> GetInsulinCommentsByQuantityId(int quantityId)
        {
            this._log.Debug("---Executing GetInsulinCommentsByQuantityId() in CommonController----");
            var result = this._emarService.GetInsulinCommentsByQuantityId(quantityId).Result;
            this._log.Debug("---Executed Successfully GetInsulinCommentsByQuantityId() in CommonController----");
            return Json<string>(result);
        }
        [Route("GetNurseComments/{patientId}")]
        [HttpGet]
        public JsonResult<List<NurseCommentsEntity>> GetNurseComments(int patientId)
        {
            this._log.Debug("---Executing GetNurseNotes() in OrdersController----");
            var result = this._emarService.GetNurseComments(patientId).Result;
            this._log.Debug("---Executed Successfully GetNurseNotes() in OrdersController----");
            return Json<List<NurseCommentsEntity>>(result);
        }
        [Route("InsertUpdateNurseComments")]
        [HttpPost]
        public JsonResult<int> InsertUpdateNurseComments(NurseCommentsEntity notes)
        {
            this._log.Debug("---Executing InsertUpdateNurseNotes() in OrdersController----");
            var result = this._emarService.InsertUpdateNurseComments(notes).Result;
            this._log.Debug("---Executed Successfully InsertUpdateNurseNotes() in OrdersController----");
            return Json<int>(result);
        }
        [Route("GetSideEffectsByGPICode/{GPICode}")]
        [HttpGet]
        public JsonResult<string> GetSideEffectsByGPICode(string GPICode)
        {
            this._log.Debug("---Executing GetSideEffectsByGPICode() in EmarController----");
            var result = this._emarService.GetSideEffectsByGPICode(GPICode).Result;
            this._log.Debug("---Executed Successfully GetSideEffectsByGPICode() in EmarController----");
            return Json<string>(result);
        }
        [Route("AdministerOdersVitalsInfo")]
        [HttpPost]
        public string AdministerOdersVitalsInfo(VitalsEntity obj)
        {
            this._log.Debug("---Executing AdministerOdersVitalsInfo() in EMARController----");
            var result = this._emarService.AdministerOdersVitalsInfo(obj).Result;
            this._log.Debug("---Executed Successfully AdministerOdersVitalsInfo() in EMARController----");
            return result;
        }
        [Route("GetEkitControlSubstanceTrans/{ekitId}")]
        [HttpGet]
        public JsonResult<List<ControlSubstanceTransEntity>> GetEkitControlSubstanceTrans(int ekitId)
        {
            this._log.Debug("---Executing GetEkitControlSubstanceTrans() in EMARController----");
            var result = this._emarService.GetEkitControlSubstanceTrans(ekitId).Result;
            this._log.Debug("---Executed Successfully GetEkitControlSubstanceTrans() in EMARController----");
            return Json<List<ControlSubstanceTransEntity>>(result);
        }
        [Route("GetDate2hrsDiff")]
        [HttpPost]
        public JsonResult<int> GetDate2hrsDiff(TimezoneEntity obj)
        {
            this._log.Debug("---Executing GetDate2hrsDiff() in CommonController----");
            var result = this._emarService.GetDate2hrsDiff(obj).Result;
            this._log.Debug("---Executed Successfully GetDate2hrsDiff() in CommonController----");
            return Json<int>(result);

        }
    }
}