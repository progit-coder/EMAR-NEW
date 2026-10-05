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
    [RoutePrefix("AdminApproval")]
    public class AdminApprovalController : ApiController
    {
        private readonly IAdminApprovalService _adminApprovalService;
        private readonly ILogger _log;
        public AdminApprovalController(IAdminApprovalService adminApprovalService, ILogger log)
        {
            this._adminApprovalService = adminApprovalService;
            this._log = log;
        }
        [Route("InsertResidentDemographicData")]
        [HttpPost]
        public JsonResult<int> InsertResidentDemographicData(ApprovalDemographicEntity demographics)
        {
            this._log.Debug("---Executing InsertResidentDemographicData() in AdminApprovalController----");
            var records = this._adminApprovalService.InsertResidentDemographicData(demographics).Result;
            this._log.Debug("---Executed Successfully InsertResidentDemographicData() in AdminApprovalController----");
            return Json<int>(records);

        }
        [Route("InsertNewResidentDemographicData")]
        [HttpPost]
        public JsonResult<int> InsertNewResidentDemographicData(NewResidentInfoEntity residentInfo)
        {
            this._log.Debug("---Executing InsertNewResidentDemographicData() in AdminApprovalController----");
            var records = this._adminApprovalService.InsertNewResidentDemographicData(residentInfo).Result;
            this._log.Debug("---Executed Successfully InsertNewResidentDemographicData() in AdminApprovalController----");
            return Json<int>(records);

        }
        [Route("PatientReAdmission")]
        [HttpPost]
        public JsonResult<int> PatientReAdmission(PatientReAdmissionEntity residentInfo)
        {
            this._log.Debug("---Executing PatientReAdmission() in AdminApprovalController----");
            var records = this._adminApprovalService.PatientReAdmission(residentInfo.PatientId, residentInfo.AdmitDate).Result;
            this._log.Debug("---Executed Successfully PatientReAdmission() in AdminApprovalController----");
            return Json<int>(records);

        }
        [Route("InsertUpdateAllergyInfo")]
        [HttpPost]
        public JsonResult<int> InsertResidentAllergy(ApprovalAllergyInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentAllergy() in AdminApprovalController----");
            var records = this._adminApprovalService.InsertResidentAllergy(entity).Result;
            this._log.Debug("---Executed Successfully InsertResidentAllergy() in AdminApprovalController----");
            return Json<int>(records);
        }
        [Route("InsertUpdateDiagnosisInfo")]
        [HttpPost]
        public JsonResult<int> InsertResidentDianosisData(ApprovalDiagnosisInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentDianosisData() in AdminApprovalController----");
            var records = this._adminApprovalService.InsertResidentDianosisData(entity).Result;
            this._log.Debug("---Executed Successfully InsertResidentDianosisData() in AdminApprovalController----");
            return Json<int>(records);
        }
        [Route("InsertResidentAdmitVistiInfoData")]
        [HttpPost]
        public JsonResult<int> InsertResidentVisitInfo(ApprovalVisitInfoEntity entity)
        {
            this._log.Debug("---Executing InsertResidentVisitInfo() in AdminApprovalController----");
            var records = this._adminApprovalService.InsertResidentVisitInfo(entity).Result;
            this._log.Debug("---Executed Successfully InsertResidentVisitInfo() in AdminApprovalController----");
            return Json<int>(records);
        }
        //[Route("InsertResidentInfo")]
        //[HttpPost]
        //public JsonResult<int> InsertResidentInfo(ApprovalVisitInfoEntity entity)
        //{
        //    this._log.Debug("---Executing InsertResidentInfo() in AdminApprovalController----");
        //    var records = this._adminApprovalService.InsertResidentInfo(entity).Result;
        //    this._log.Debug("---Executed Successfully InsertResidentInfo() in AdminApprovalController----");
        //    return Json<int>(records);
        //}
        [Route("GetAllApprovalPendingList")]
        [HttpGet]
        public JsonResult<List<ApprovalPendingCustomEntity>> GetAllApprovalPendingList()
        {
            try
            {
                this._log.Debug("---Executing GetApprovalPendingList() in AdminApprovalController----");
                var records = this._adminApprovalService.GetAllApprovalPendingList().Result;
                this._log.Debug("---Executed Successfully GetApprovalPendingList() in AdminApprovalController----");
                return Json<List<ApprovalPendingCustomEntity>>(records);
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }
        [Route("GetApprovalPendingList/{patientID}")]
        [HttpGet]
        public JsonResult<List<ApprovalPendingCustomEntity>> GetApprovalPendingList(int patientID)
        {
            this._log.Debug("---Executing GetApprovalPendingList() in ResidentDemographicController----");
            var records = this._adminApprovalService.GetApprovalPendingList(patientID).Result;
            this._log.Debug("---Executed Successfully GetApprovalPendingList() in ResidentDemographicController----");
            return Json<List<ApprovalPendingCustomEntity>>(records);
        }

        [Route("ApprovePendingData")]
        [HttpPost]
        public int ApprovePendingData(List<ApprovalPendingCustomEntity> record)
        {
            this._log.Debug("---Executing ApprovePendingData() in AdminApprovalController----");
            var result = this._adminApprovalService.ApprovePendingData(record).Result;
            this._log.Debug("---Executed Successfully ApprovePendingData() in AdminApprovalController----");
            return result;
        }
        [Route("RejectPendingData")]
        [HttpPost]
        public int RejectPendingData(List<ApprovalPendingCustomEntity> record)
        {
            this._log.Debug("---Executing RejectPendingData() in AdminApprovalController----");
            var result = this._adminApprovalService.RejectPendingData(record).Result;
            this._log.Debug("---Executed Successfully RejectPendingData() in AdminApprovalController----");
            return result;
        }
        [Route("GetAdminApprovalResidentsData")]
        [HttpGet]
        public JsonResult<List<DemographicResidentDropEnity>> GetAdminApprovalResidentsData()
        {
            this._log.Debug("---Executing GetAdminApprovalResidentsData() in AdminApprovalController----");
            var record = this._adminApprovalService.GetAdminApprovalResidentsData().Result;
            this._log.Debug("---Executed Successfully GetAdminApprovalResidentsData() in AdminApprovalController----");
            return Json<List<DemographicResidentDropEnity>>(record);
        }
        [Route("ViewModifiedData/{ApprovalId}/{RecordId}/{Category}")]
        [HttpGet]
        public JsonResult<List<ApprovalChangesCustomEntity>> GetModifiedData(int approvalId, int recordId, string category)
        {
            this._log.Debug("---Executing GetModifiedData() in AdminApprovalController----");
            var record = this._adminApprovalService.GetModifiedData(approvalId, recordId, category).Result;
            this._log.Debug("---Executed Successfully GetModifiedData() in AdminApprovalController----");
            return Json<List<ApprovalChangesCustomEntity>>(record);
        }

        [Route("InsertApprovalOrderData")]
        [HttpPost]
        public JsonResult<int> InsertApprovalOrderData(ApprovalOrderEntity entity)
        {
            this._log.Debug("---Executing GetModifiedData() in AdminApprovalController----");
            int result = this._adminApprovalService.InsertApprovalOrderData(entity).Result;
            this._log.Debug("---Executed Successfully GetModifiedData() in AdminApprovalController----");
            return Json<int>(result);
        }
        [Route("InsertRefill")]
        [HttpPost]
        public int InsertUpdateApprovalRefill(ApprovalRefillEntity refillObj)
        {
            this._log.Debug("---Executing InsertUpdateApprovalRefill() in AdminApprovalController----");
            var result = this._adminApprovalService.InsertUpdateApprovalRefill(refillObj).Result;
            this._log.Debug("---Executed Successfully InsertUpdateApprovalRefill() in AdminApprovalController----");
            return result;
        }
        [Route("DiscontinueOrder")]
        [HttpPost]
        public int DiscontinueOrder(OrdersCommonStatusEntity record)
        {
          

            this._log.Debug("---Executing DiscontinueOrder() in AdminApprovalController PatientId : "+ record.PatientId+ "OrderId :" + record.OrderId );
            this._log.Debug("---Executing DiscontinueOrder() in AdminApprovalController----");
            var result = this._adminApprovalService.DiscontinueOrder(record).Result;
            this._log.Debug("---Executed Successfully DiscontinueOrder() in AdminApprovalController----");
            return result;
        }
        [Route("InsertApprovalCPOEOrderData")]
        [HttpPost]
        public JsonResult<int> InsertApprovalCPOEOrderData(ApprovalCPOEOrderEntity entity)
        {
            try
            {
                this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController----");
                this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController---- " + entity.Patient_Id);
                this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController OrderingPhysicianID---- " + entity.OrderingPhysicianID);
                this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController POrder_CreatedBy---- " + entity.POrder_CreatedBy);
                //this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController POrder_CreatedBy---- " + entity.orde);
                int result = this._adminApprovalService.InsertApprovalCPOEOrderData(entity).Result;
                this._log.Debug("---Executed Successfully InsertApprovalCPOEOrderData() in AdminApprovalController----");
                return Json<int>(result);
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController Exception----"+ex.Message.ToString());
                this._log.Debug("---Executing InsertApprovalCPOEOrderData() in AdminApprovalController Exception----" + ex.InnerException.Message.ToString());
                return null;
            }
           
           // return null;
        }
    }
}
