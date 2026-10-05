using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IAdminApprovalRepository
    {
        int InsertResidentDemographicData(ApprovalDemographicEntity demographics);
        int InsertResidentAllergy(ApprovalAllergyInfoEntity entity);
        int InsertResidentDianosisData(ApprovalDiagnosisInfoEntity entity);
        int InsertResidentVisitInfo(ApprovalVisitInfoEntity entity);
        int InsertResidentVisitInfoWithoutApproval(ApprovalVisitInfoEntity entity, EMAREntities context);
        List<ApprovalAllergyInfoEntity> GetApprovalPendingAllergies();
        List<ApprovalDiagnosisInfoEntity> GetApprovalPendingDiagnosis();
        List<ApprovalVisitInfoEntity> GetApprovalPendingTransfer();
        List<ApprovalVisitInfoEntity> GetApprovalPendingDischarge();
        List<ApprovalDemographicEntity> GetApprovalPendingDemographics();
        List<ApprovalOrderEntity> GetApprovalPendingOrders();
        List<ApprovalRefillEntity> GetApprovalPendingRefill();
        int ApproveAllergy(ApprovalPendingCustomEntity entity, EMAREntities context);
        int ApproveDiagnosis(ApprovalPendingCustomEntity entity, EMAREntities context);
        int ApproveDemographic(ApprovalPendingCustomEntity entity, EMAREntities context);
        int ApproveTransfer(ApprovalPendingCustomEntity entity, EMAREntities context);
        int ApproveDischarge(ApprovalPendingCustomEntity entity, EMAREntities context);
        int ApproveRefill(ApprovalPendingCustomEntity entity, EMAREntities context);
        List<DemographicResidentDropEnity> GetAdminApprovalResidentsData();
        int RejectAllergy(ApprovalPendingCustomEntity entity);
        int RejectDiagnosis(ApprovalPendingCustomEntity entity);
        int RejectDemographic(ApprovalPendingCustomEntity entity);
        int RejectTransferDischarge(ApprovalPendingCustomEntity entity);
        int RejectNewOrder(ApprovalPendingCustomEntity entity);
        int RejectRefill(ApprovalPendingCustomEntity entity);
        List<ApprovalDemographicEntity> GetApprovalPendingDemographicsByPatientId(int patientId);
        List<ApprovalAllergyInfoEntity> GetApprovalPendingAllergiesByPatientId(int patientId);
        List<ApprovalDiagnosisInfoEntity> GetApprovalPendingDiagnosisByPatientId(int patientId);
        List<ApprovalVisitInfoEntity> GetApprovalPendingTransferByPatientId(int patientId);
        List<ApprovalVisitInfoEntity> GetApprovalPendingDischargeByPatientId(int patientId);
        List<ApprovalOrderEntity> GetApprovalPendingOrdersByPatientId(int patientId);
        List<ApprovalRefillEntity> GetApprovalPendingRefillByPatientId(int patientId);
        List<ApprovalChangesCustomEntity> GetModifiedData(int approvalId, int recordId, string category);
        int InsertApprovalOrderData(ApprovalOrderEntity entity);
        int ApproveNewOrder(ApprovalPendingCustomEntity entity, EMAREntities context);
        string GetUserNameById(int userId);
        int UpdateDemographicWithoutApproval(ApprovalDemographicEntity entity, EMAREntities context);
        int InsertUpdateAllergyWithoutApproval(ApprovalAllergyInfoEntity allergyInfo, EMAREntities context);
        int InsertUpdateDiagnosisWithoutApproval(ApprovalDiagnosisInfoEntity diagnosisInfo, EMAREntities context);
        int InsertUpdateApprovalRefill(ApprovalRefillEntity refillObj);
        int DiscontinueOrder(OrdersCommonStatusEntity entity, EMAREntities context);
        int InsertApprovedRefillFileId(int patientId, int orderId, int fileId, EMAREntities context);
        int InsertNewDemographicDetails(NewResidentInfoEntity residentInfo,int ApprovalFlag);
        int DiscontinuedSplitOrder(OrdersCommonStatusEntity entity, EMAREntities context);
        int PatientReAdmission(int patientId, DateTime AdmitDate);
        int ApproveCPOENewOrder(ApprovalPendingCustomEntity entity, EMAREntities context);
        int InsertApprovalCPOEOrderData(ApprovalCPOEOrderEntity entity);
        int IsCPOEOrder(long ApprovalId, EMAREntities context);
    }
}
