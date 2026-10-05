using LTCPro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using log4net.Core;


namespace LTCPro.ServiceLayer
{
    public class AssessmentsService : IAssessmentsService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IAssessmentsRepository _assessmentsRepository;
        private readonly ILogger _log;

        public AssessmentsService(IAutoMapper autoMapper, IAssessmentsRepository assessmentsRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._assessmentsRepository = assessmentsRepository;
            this._log = log;
        }
        public async Task<int> InsertUpdateWeight(WeightLogEntity weightLog)
        {
            this._log.Debug("---Executing InsertUpdateWeight() in AssessmentService----");
            return await Task.FromResult<int>(this._assessmentsRepository.InsertUpdateWeight(weightLog));
        }
        public async Task<List<WeightLogEntity>> GetWeightList(int Patient_Id)
        {
            this._log.Debug("---Executing GetWeightList() in AssessmentService----");
            return await Task.FromResult<List<WeightLogEntity>>(this._assessmentsRepository.GetWeightList(Patient_Id));
        }
        public async Task<WeightLogEntity> GetWeightDetailsByID(int WeightLog_ID)
        {
            this._log.Debug("---Executing GetWeightDetailsByID() in AssessmentService----");
            return await Task.FromResult<WeightLogEntity>(this._assessmentsRepository.GetWeightDetailsByID(WeightLog_ID));
        }
        public async Task<List<AdmitDateEntity>> GetAdmitDateByID(int Patient_Id)
        {
            this._log.Debug("---Executing GetAdmitDateByID() in AssessmentService----");
            return await Task.FromResult<List<AdmitDateEntity>>(this._assessmentsRepository.GetAdmitDateByID(Patient_Id));
        }
        public async Task<int> insertUpdateVisitBehaviour(VisitBehaviourEntity visitBehaviour)
        {
            this._log.Debug("---Executing insertUpdateVisitBehaviour() in AssessmentsService----");
            return await Task.FromResult<int>(this._assessmentsRepository.insertUpdateVisitBehaviour(visitBehaviour));
        }
        public async Task<List<VisitBehaviourEntity>> GetVisitBehaviourList(int Patient_Id)
        {
            this._log.Debug("---Executing GetVisitBehaviourList() in AssessmentsService----");
            return await Task.FromResult<List<VisitBehaviourEntity>>(this._assessmentsRepository.GetVisitBehaviourList(Patient_Id));
        }
        public async Task<List<BehavioralSymptomsMasterEntity>> GetBehaviourDropData()
        {
            this._log.Debug("---Executing GetBehaviourDropData() in AssessmentsService----");
            return await Task.FromResult<List<BehavioralSymptomsMasterEntity>>(this._assessmentsRepository.GetBehaviourDropData());
        }
        public async Task<VisitBehaviourEntity> GetBehaviourDetailsByID(int VisitBehaviourId)
        {
            this._log.Debug("---Executing GetBehaviourDetailsByID() in AssessmentsService----");
            return await Task.FromResult<VisitBehaviourEntity>(this._assessmentsRepository.GetBehaviourDetailsByID(VisitBehaviourId));
        }
        public async Task<int> insertUpdateVisitFoodIn(VisitFoodintakeEntity visitiFoodIn)
        {
            this._log.Debug("---Executing insertUpdateVisitFoodIn() in AssessmentsService----");
            return await Task.FromResult<int>(this._assessmentsRepository.insertUpdateVisitFoodIn(visitiFoodIn));
        }
        public async Task<List<VisitFoodintakeEntity>> GetVisitFoodIntakeList(int Patient_Id)
        {
            this._log.Debug("---Executing GetVisitFoodIntakeList() in AssessmentsService----");
            return await Task.FromResult<List<VisitFoodintakeEntity>>(this._assessmentsRepository.GetVisitFoodIntakeList(Patient_Id));
        }
        public async Task<VisitFoodintakeEntity> GetFoodIntakeDetailsByID(int VisitFoodIntakeId)
        {
            this._log.Debug("---Executing GetFoodIntakeDetailsByID() in AssessmentsService----");
            return await Task.FromResult<VisitFoodintakeEntity>(this._assessmentsRepository.GetFoodIntakeDetailsByID(VisitFoodIntakeId));
        }
        public async Task<int> InsertUpdateVisitVitals(VisitVitalEntity visitvitals)
        {

            this._log.Debug("---Executing InsertUpdateVisitVitals() in AssesmentService----");
            return await Task.FromResult<int>(this._assessmentsRepository.InsertUpdateVisitVitals(visitvitals));
        }
        public async Task<List<VisitVitalEntity>> GetAllVisitVitalList(int Patient_Id)
        {
            this._log.Debug("---Executing GetAllVisitVitalList() in AssesmentService----");
            return await Task.FromResult<List<VisitVitalEntity>>(this._assessmentsRepository.GetAllVisitVitalList(Patient_Id));
        }
        public async Task<VisitVitalEntity> GetVisitVitalDetailsByID(int Vitals_ID)
        {
            this._log.Debug("---Executing GetVisitVitalDetailsByID() in AssesmentService----");
            return await Task.FromResult<VisitVitalEntity>(this._assessmentsRepository.GetVisitVitalDetailsByID(Vitals_ID));
        }
        public async Task<int> InsertUpdateVisitNurseNote(VisitNursingNoteEntity visitnursenote)
        {

            this._log.Debug("---Executing InsertUpdateVisitNurseNote() in AssesmentService----");
            return await Task.FromResult<int>(this._assessmentsRepository.InsertUpdateVisitNurseNote(visitnursenote));
        }
        public async Task<List<VisitNursingNoteEntity>> GetVisitNurseNote(int Patient_Id)
        {
            this._log.Debug("---Executing GetVisitNurseNote() in AssesmentService----");
            return await Task.FromResult<List<VisitNursingNoteEntity>>(this._assessmentsRepository.GetVisitNurseNote(Patient_Id));
        }
        public async Task<VisitNursingNoteEntity> GetVisitNurseNoteDetailsByID(int VisitNursingNotes_ID)
        {
            this._log.Debug("---Executing GetVisitNurseNoteDetailsByID() in AssesmentService----");
            return await Task.FromResult<VisitNursingNoteEntity>(this._assessmentsRepository.GetVisitNurseNoteDetailsByID(VisitNursingNotes_ID));
        }
        public async Task<List<ResidentDropEntity>> GetResidentsListByNSId(int nurseStationId)
        {
            this._log.Debug("---Executing GetResidentsListByNSId() in AssesmentService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._assessmentsRepository.GetResidentsListByNSId(nurseStationId));
        }

    }
}


