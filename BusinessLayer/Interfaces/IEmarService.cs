using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;


namespace LTCPro.ServiceLayer
{
    public interface IEmarService
    {
        Task<EmarData> GetEmarResidentGridData(string time, string dateValue, int nurseStationId, int NurseStationShiftId, int showTwoHours);
        Task<IList> GetEmarResidentBarcodes(string dateValue, int nurseStationId);

        Task<List<NursingScheduleDropEntity>> GetNursingScheduleData(int nurseStationId, string scheduleDate);
        Task<string> GetResidentBiometricInfo(int patientId);
        Task<List<EmarOrdersListEntity>> GetEmarOrdersList(int patientId, string time, string dateValue, int shiftId, int window,int userId);
        Task<RefillDataEntity> GetEmarRefillNotes(int orderId);
        Task<int> InsertDrugAdminister(DrugAdministerEntity drugAdministar);
        Task<int> InsertNursingFrequencyConfig(NursingFrequencyConfigCustomEntity frequencyconfig);
        Task<List<PrcGetFreqMappingData_ResultEntity>> GetNursingFrequencyConfigsData(int userId);
        Task<List<MedicationReasonEntity>> GetMedicationReason();
        Task<NursingFrequencyConfigCustomEntity> GetNursingFrequencyDetailsByID(int nursingFreqId);
        Task<List<PRNDetailsCustomEntity>> GetPRNData(PRNFilterCustomEntity configs);
        Task<List<SeventyTwoHourCheckEntity>> GetSeventyTwoHourCheckDetails(SeventyTwoHourCustomEntity configs);
        Task<int> InsertUpdateSeventyTwoHourChecks(List<SeventyTwoHourInsertEntity> s);
        Task<int> InsertPRNData(List<PRNInsertCustomEntity> prnData);
        Task<List<SeventyTwoHourCheckEntity>> GetSeventyTwoHourDetailsByfilter(SeventyTwoHourCustomEntity configs);
        Task<List<PRNDetailsCustomEntity>> GetPRNDetailsByfilter(PRNFilterCustomEntity configs);
        Task<int> ByPassBiometric(BypassBiometricEntity byPass);
        Task<int> InsertAdministredwithoutscanning(NurseCommentsEntity nurseobj);
        Task<List<VitalsCheckEntity>> GetVitalsCheckList(int PQuantityId);
        Task<List<VitalsCheckEntity>> GetVitalsChecksListbyQuantityIds(string PQuantityIds);
        Task<int> InsertOrderFavouritesData(List<OrderFavouriteDataEntity> entity);
        Task<int> GetDueMARAlertData(int userId);
        Task<List<EkitDropEntity>> GetEkitDropData(int nursestaionId, int orderId, int userId);
        Task<List<NurseShiftsEntity>> GetNurseShiftDrop(int nursestationId, string scheduleDate);
        Task<List<NursingScheduleDropEntity>> GetNursingScheduleDataByPatientID(int patientId, string scheduleDate, int nursingStationId);
        Task<List<ControlSubstanceTransEntity>> GetControlSubstanceTrans(int quantityId);
        Task<int> GetPRNAdministerCountByDate(int orderId, int quantityd, string dateValue);
        Task<string> GetInsulinCommentsByQuantityId(int quantityId);
        Task<int> InsertUpdateNurseComments(NurseCommentsEntity notes);
        Task<List<NurseCommentsEntity>> GetNurseComments(int patientId);
        Task<string> GetSideEffectsByGPICode(string GPICode);
        Task<string> AdministerOdersVitalsInfo(VitalsEntity obj);
        Task<List<ControlSubstanceTransEntity>> GetEkitControlSubstanceTrans(int ekitId);
        Task<int> GetDate2hrsDiff(TimezoneEntity obj);
    }
}
