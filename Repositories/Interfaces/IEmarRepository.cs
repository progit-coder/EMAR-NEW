using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.Repositories
{
    public interface IEmarRepository
    {
        EmarData GetEmarResidentGridData(string time, string dateValue, int nurseStationId,int NurseStationShiftId, int showTwoHours);
        IList GetEmarResidentBarcodes(string dateValue, int nurseStationId);

        List<NursingScheduleDropEntity> GetNursingScheduleData(int nurseStationId ,string scheduleDate);
        string GetResidentBiometricInfo(int PatientId);
        List<EmarOrdersListEntity> GetEmarOrdersList(int patientId, string time, string dateValue, int shiftId, int window,int userId);
        RefillDataEntity GetEmarRefillNotes(int orderId);
        int InsertDrugAdminister(DrugAdministerEntity drugAdministar);
        int InsertNursingFrequencyConfig(NursingFrequencyConfigCustomEntity frequencyconfig);
        List<PrcGetFreqMappingData_ResultEntity> GetNursingFrequencyConfigsData(int userId);
        List<MedicationReasonEntity> GetMedicationReason();
        NursingFrequencyConfigCustomEntity GetNursingFrequencyDetailsByID(int nursingFreqId);
        List<PRNDetailsCustomEntity> GetPRNData(PRNFilterCustomEntity configs);
        List<SeventyTwoHourCheckEntity> GetSeventyTwoHourCheckDetails(SeventyTwoHourCustomEntity configs);
        int InsertUpdateSeventyTwoHourChecks(List<SeventyTwoHourInsertEntity> s);
        int InsertPRNData(List<PRNInsertCustomEntity> prnData);
        List<SeventyTwoHourCheckEntity> GetSeventyTwoHourDetailsByfilter(SeventyTwoHourCustomEntity configs);
        List<PRNDetailsCustomEntity> GetPRNDetailsByfilter(PRNFilterCustomEntity configs);
        int ByPassBiometric(BypassBiometricEntity byPass);
        int InsertAdministredwithoutscanning(NurseCommentsEntity nurseobj);
        List<VitalsCheckEntity> GetVitalsCheckList(int PQuantityId);
        List<VitalsCheckEntity> GetVitalsChecksListbyQuantityIds(string PQuantityIds);
        int InsertOrderFavouritesData(List<OrderFavouriteDataEntity> entity);
        int GetDueMARAlertData(int userId);
        List<EkitDropEntity> GetEkitDropData(int nursestaionId,int orderId, int userId);
        List<NurseShiftsEntity> GetNurseShiftDrop(int nursestationId, string scheduleDate);
        List<NursingScheduleDropEntity> GetNursingScheduleDataByPatientID(int patientId, string scheduleDate, int nursingStationId);
        List<ControlSubstanceTransEntity> GetControlSubstanceTrans(int quantityId);
        int GetPRNAdministerCountByDate(int orderId, int quantityd, string dateValue);
        string GetInsulinCommentsByQuantityId(int quantityId);
        int InsertUpdateNurseComments(NurseCommentsEntity notes);
        List<NurseCommentsEntity> GetNurseComments(int patientId);
        string GetSideEffectsByGPICode(string GPICode);
        string AdministerOdersVitalsInfo(VitalsEntity obj);
        List<ControlSubstanceTransEntity> GetEkitControlSubstanceTrans(int ekitId);
        int GetDate2hrsDiff(TimezoneEntity obj);
    }
}
