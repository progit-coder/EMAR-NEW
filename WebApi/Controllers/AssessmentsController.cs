using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Assessments")]
    [CustomAuthorizationFilter]
    public class AssessmentsController : ApiController
    {
        private readonly IAssessmentsService _assessmentsService;
        private readonly ILogger _log;
        public AssessmentsController(IAssessmentsService assessmentsService, ILogger log)
        {
            this._assessmentsService = assessmentsService;
            this._log = log;
        }
        [Route("InsertUpdateWeight")]
        [HttpPost]
        public int InsertUpdateWeight(WeightLogEntity weightLog)
        {
            this._log.Debug("---Executing Insert/Update() in AssessmentController----");
            int result = this._assessmentsService.InsertUpdateWeight(weightLog).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in AssessmentController----");
            return result;
        }
        [Route("GetWeightList/{Patient_Id}")]
        [HttpGet]
        public JsonResult<List<WeightLogEntity>> GetWeightList(int Patient_Id)
        {

            this._log.Debug("---Executing GetWeightList(Patient_Id) in AssessmentController----");
            var weightlogs = this._assessmentsService.GetWeightList(Patient_Id).Result;
            this._log.Debug("---Executed Successfully GetWeightList(Patient_Id) in AssessmentController----");
            return Json<List<WeightLogEntity>>(weightlogs);
        }
        [Route("GetWeightDetailsByID/{WeightLog_ID}")]
        [HttpGet]
        public JsonResult<WeightLogEntity> GetWeightDetailsByID(int WeightLog_ID)
        {
            this._log.Debug("---Executing GetWeightDetailsByID() in AssessmentController----");
            var weight = this._assessmentsService.GetWeightDetailsByID(WeightLog_ID).Result;
            this._log.Debug("---Executed Successfully GetAssessmentDetailsByID() in AssessmentController----");
            return Json<WeightLogEntity>(weight);
        }
        [Route("GetAdmitDateByID/{Patient_Id}")]
        [HttpGet]
        public JsonResult<List<AdmitDateEntity>> GetAdmitDateByID(int Patient_Id)
        {
            this._log.Debug("---Executing GetAdmitDateByID() in AssessmentController----");
            var admit = this._assessmentsService.GetAdmitDateByID(Patient_Id).Result;
            this._log.Debug("---Executed Successfully GetAdmitDateByID() in AssessmentController----");
            return Json<List<AdmitDateEntity>>(admit);
        }
        [Route("insertUpdateVisitBehaviour")]
        [HttpPost]
        public int insertUpdateVisitBehaviour(VisitBehaviourEntity visitBehaviour)
        {
            this._log.Debug("---Executing insertUpdateVisitBehaviour() in AssessmentsController----");
            var result = this._assessmentsService.insertUpdateVisitBehaviour(visitBehaviour).Result;
            this._log.Debug("---Executed Successfully insertUpdateVisitBehaviour() in AssessmentsController----");
            return result;

        }
        [Route("GetVisitBehaviourList/{Patient_Id}")]
        [HttpGet]
        public JsonResult<List<VisitBehaviourEntity>> GetVisitBehaviourList(int Patient_Id)
        {
            this._log.Debug("---Executing GetVisitBehaviourList() in AssessmentsController----");
            var visitbehaviour = this._assessmentsService.GetVisitBehaviourList(Patient_Id).Result;
            this._log.Debug("---Executed Successfully GetVisitBehaviourList() in AssessmentsController----");
            return Json(visitbehaviour);
        }
        [Route("GetBehaviourDropData")]
        [HttpGet]
        public JsonResult<List<BehavioralSymptomsMasterEntity>> GetBehaviourDropData()
        {
            this._log.Debug("---Executing GetBehaviourDropData() in AssessmentsController----");
            var behaviourDrop = this._assessmentsService.GetBehaviourDropData().Result;
            this._log.Debug("---Executed Successfully GetBehaviourDropData() in AssessmentsController----");
            return Json(behaviourDrop);
        }
        [Route("GetBehaviourDetailsByID/{VisitBehaviourId}")]
        [HttpGet]
        public JsonResult<VisitBehaviourEntity> GetBehaviourDetailsByID(int VisitBehaviourId)
        {
            this._log.Debug("---Executing GetBehaviourDetailsByID() in AssessmentsController----");
            var visitbehaviour = this._assessmentsService.GetBehaviourDetailsByID(VisitBehaviourId).Result;
            this._log.Debug("---Executed Successfully GetBehaviourDetailsByID() in AssessmentsController----");
            return Json<VisitBehaviourEntity>(visitbehaviour);
        }
        [Route("insertUpdateVisitFoodIntake")]
        [HttpPost]
        public int insertUpdateVisitFoodIn(VisitFoodintakeEntity visitiFoodIn)
        {
            this._log.Debug("---Executing insertUpdateVisitFoodIn() in AssessmentsController----");
            var result = this._assessmentsService.insertUpdateVisitFoodIn(visitiFoodIn).Result;
            this._log.Debug("---Executed Successfully insertUpdateVisitFoodIn() in AssessmentsController----");
            return result;

        }
        [Route("GetVisitFoodIntakeList/{Patient_Id}")]
        [HttpGet]
        public JsonResult<List<VisitFoodintakeEntity>> GetVisitFoodIntakeList(int Patient_Id)
        {
            this._log.Debug("---Executing GetVisitFoodIntakeList() in AssessmentsController----");
            var visitfoodin = this._assessmentsService.GetVisitFoodIntakeList(Patient_Id).Result;
            this._log.Debug("---Executed Successfully GetVisitFoodIntakeList() in AssessmentsController----");
            return Json(visitfoodin);
        }
        [Route("GetFoodIntakeDetailsByID/{VisitFoodIntakeId}")]
        [HttpGet]
        public JsonResult<VisitFoodintakeEntity> GetFoodIntakeDetailsByID(int VisitFoodIntakeId)
        {
            this._log.Debug("---Executing GetFoodIntakeDetailsByID() in AssessmentsController----");
            var visitfoodin = this._assessmentsService.GetFoodIntakeDetailsByID(VisitFoodIntakeId).Result;
            this._log.Debug("---Executed Successfully GetFoodIntakeDetailsByID() in AssessmentsController----");
            return Json<VisitFoodintakeEntity>(visitfoodin);
        }
        [Route("InsertUpdateVisitVitals")]
        [HttpPost]
        public int InsertUpdateVisitVitals(VisitVitalEntity visitvitals)
        {
            try
            {
                this._log.Debug("---vital Values----" + visitvitals.Temperature + "," + visitvitals.RespiratoryRate + "," + visitvitals.BloodSugar + "," + visitvitals.PulseRate + "," + visitvitals.HeartRate + "," + visitvitals.CistolicBP + "/" + visitvitals.DiastolicBP + "," + visitvitals.Remark);

                this._log.Debug("---Executing InsertUpdateVisitVitals() in AssesmentController----");
                var result = this._assessmentsService.InsertUpdateVisitVitals(visitvitals).Result;
                this._log.Debug("---Executed Successfully InsertUpdateVisitVitals() in AssesmentController----");
                return result;
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing InsertUpdateVisitVitals() in AssesmentController Error----"+ex.Message.ToString()+"---"+ex.InnerException.Message.ToString());
                return 0;
            }
           
        }
        [Route("GetAllVisitVitalList/{Patient_Id}")]
        [HttpGet]
        public JsonResult<List<VisitVitalEntity>> GetAllVisitVitalList(int Patient_Id)
        {

            this._log.Debug("---Executing GetAllVisitVitalList() in AssesmentController----");
            var visitvital = this._assessmentsService.GetAllVisitVitalList(Patient_Id).Result;
            this._log.Debug("---Executed Successfully GetAllVisitVitalList() in AssesmentController----");
            return Json<List<VisitVitalEntity>>(visitvital);
        }
        [Route("GetVisitVitalDetailsByID/{Vitals_ID}")]
        [HttpGet]
        public JsonResult<VisitVitalEntity> GetVisitVitalDetailsByID(int Vitals_ID)
        {
            this._log.Debug("---Executing GetVisitVitalDetailsByID() in AssesmentController----");
            var vital = this._assessmentsService.GetVisitVitalDetailsByID(Vitals_ID).Result;
            this._log.Debug("---Executed Successfully GetVisitVitalDetailsByID() in AssesmentController----");
            return Json<VisitVitalEntity>(vital);
        }
        [Route("InsertUpdateVisitNurseNote")]
        [HttpPost]
        public int InsertUpdateVisitNurseNote(VisitNursingNoteEntity visitnursenote)
        {
            this._log.Debug("---Executing InsertUpdateVisitNurseNote() in AssesmentController----");
            var result = this._assessmentsService.InsertUpdateVisitNurseNote(visitnursenote).Result;
            this._log.Debug("---Executed Successfully InsertUpdateVisitNurseNote() in AssesmentController----");
            return result;
        }
        [Route("GetVisitNurseNote/{Patient_Id}")]
        [HttpGet]
        public JsonResult<List<VisitNursingNoteEntity>> GetVisitNurseNote(int Patient_Id)
        {

            this._log.Debug("---Executing GetVisitNurseNote() in AssesmentController----");
            var visitnursenotes = this._assessmentsService.GetVisitNurseNote(Patient_Id).Result;
            this._log.Debug("---Executed Successfully GetVisitNurseNote() in AssesmentController----");
            return Json<List<VisitNursingNoteEntity>>(visitnursenotes);
        }
        [Route("GetVisitNurseNoteDetailsByID/{VisitNursingNotes_ID}")]
        [HttpGet]
        public JsonResult<VisitNursingNoteEntity> GetVisitNurseNoteDetailsByID(int VisitNursingNotes_ID)
        {
            this._log.Debug("---Executing GetVisitNurseNoteDetailsByID() in AssesmentController----");
            var visitnurse = this._assessmentsService.GetVisitNurseNoteDetailsByID(VisitNursingNotes_ID).Result;
            this._log.Debug("---Executed Successfully GetVisitNurseNoteDetailsByID() in AssesmentController----");
            return Json<VisitNursingNoteEntity>(visitnurse);
        }
        [Route("GetResidentsListByNSId/{nurseStationId}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetResidentsListByNSId(int nurseStationId)
        {
            this._log.Debug("---Executing GetResidentsListByNSId() in AssesmentController----");
            var records = this._assessmentsService.GetResidentsListByNSId(nurseStationId).Result;
            this._log.Debug("---Executed Successfully GetResidentsListByNSId() in AssesmentController----");
            return Json<List<ResidentDropEntity>>(records);
        }
    }
}
