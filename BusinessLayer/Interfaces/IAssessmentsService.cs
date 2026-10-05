using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IAssessmentsService
    {
Task<int> InsertUpdateWeight(WeightLogEntity weightLog);
        Task<List<WeightLogEntity>> GetWeightList(int Patient_Id);
        Task<WeightLogEntity> GetWeightDetailsByID(int WeightLog_ID);
        Task<List<AdmitDateEntity>> GetAdmitDateByID(int Patient_Id);
        Task<int> insertUpdateVisitBehaviour(VisitBehaviourEntity visitBehaviour);
        Task<List<VisitBehaviourEntity>> GetVisitBehaviourList(int Patient_Id);
        Task<List<BehavioralSymptomsMasterEntity>> GetBehaviourDropData();
        Task<VisitBehaviourEntity> GetBehaviourDetailsByID(int VisitBehaviourId);
        Task<int> insertUpdateVisitFoodIn(VisitFoodintakeEntity visitiFoodIn);
        Task<List<VisitFoodintakeEntity>> GetVisitFoodIntakeList(int Patient_Id);
        Task<VisitFoodintakeEntity> GetFoodIntakeDetailsByID(int VisitFoodIntakeId);
        Task<int> InsertUpdateVisitVitals(VisitVitalEntity visitvitals);
        Task<List<VisitVitalEntity>> GetAllVisitVitalList(int Patient_Id);
        Task<VisitVitalEntity> GetVisitVitalDetailsByID(int Vitals_ID);
        Task<int> InsertUpdateVisitNurseNote(VisitNursingNoteEntity visitnursenote);
        Task<List<VisitNursingNoteEntity>> GetVisitNurseNote(int Patient_Id);
        Task<VisitNursingNoteEntity> GetVisitNurseNoteDetailsByID(int VisitNursingNotes_ID);
        Task<List<ResidentDropEntity>> GetResidentsListByNSId(int nurseStationId);
    }
}

