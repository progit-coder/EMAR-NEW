using LTCPro.DAL;
using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IAdminApprovalService
    {
        Task<int> InsertResidentDemographicData(ApprovalDemographicEntity demographics);
        Task<int> InsertResidentAllergy(ApprovalAllergyInfoEntity entity);
        Task<int> InsertResidentDianosisData(ApprovalDiagnosisInfoEntity entity);
        Task<int> InsertResidentVisitInfo(ApprovalVisitInfoEntity entity);
        //Task<int> InsertResidentInfo(ApprovalVisitInfoEntity entity);
        Task<List<ApprovalPendingCustomEntity>> GetAllApprovalPendingList();
        Task<int> ApprovePendingData(List<ApprovalPendingCustomEntity> record);
        Task<int> RejectPendingData(List<ApprovalPendingCustomEntity> record);
        Task<List<DemographicResidentDropEnity>> GetAdminApprovalResidentsData();
        Task<List<ApprovalPendingCustomEntity>> GetApprovalPendingList(int patientId);
        Task<List<ApprovalChangesCustomEntity>> GetModifiedData(int approvalId, int recordId, string category);
        Task<int> InsertApprovalOrderData(ApprovalOrderEntity entity);
        Task<int> InsertUpdateApprovalRefill(ApprovalRefillEntity refillObj);
        Task<int> DiscontinueOrder(OrdersCommonStatusEntity entity);
        Task<int> InsertNewResidentDemographicData(NewResidentInfoEntity residentInfo);
        Task<int> PatientReAdmission(int patientId, DateTime AdmitDate);
        Task<int> InsertApprovalCPOEOrderData(ApprovalCPOEOrderEntity entity);
    }
}
