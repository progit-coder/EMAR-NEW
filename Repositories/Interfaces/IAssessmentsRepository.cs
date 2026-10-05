using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IAssessmentsRepository
    {
int InsertUpdateWeight(WeightLogEntity weight);
        List<WeightLogEntity> GetWeightList(int Patient_Id);
        WeightLogEntity GetWeightDetailsByID(int WeightLog_ID);
        List<AdmitDateEntity> GetAdmitDateByID(int Patient_Id);
        int insertUpdateVisitBehaviour(VisitBehaviourEntity visitBehaviour);
        List<VisitBehaviourEntity> GetVisitBehaviourList(int Patient_Id);
        List<BehavioralSymptomsMasterEntity> GetBehaviourDropData();
        VisitBehaviourEntity GetBehaviourDetailsByID(int VisitBehaviourId);
        int insertUpdateVisitFoodIn(VisitFoodintakeEntity visitiFoodIn);
        List<VisitFoodintakeEntity> GetVisitFoodIntakeList(int Patient_Id);
        VisitFoodintakeEntity GetFoodIntakeDetailsByID(int VisitFoodIntakeId);
        int InsertUpdateVisitVitals(VisitVitalEntity visitvitals);
        List<VisitVitalEntity> GetAllVisitVitalList(int Patient_Id);
        VisitVitalEntity GetVisitVitalDetailsByID(int Vitals_ID);
        int InsertUpdateVisitNurseNote(VisitNursingNoteEntity visitnursenote);
        List<VisitNursingNoteEntity> GetVisitNurseNote(int Patient_Id);
        VisitNursingNoteEntity GetVisitNurseNoteDetailsByID(int VisitNursingNotes_ID);
        List<ResidentDropEntity> GetResidentsListByNSId(int nurseStationId);


   }
}
