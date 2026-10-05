using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class VisitInfoEntity
    {
        public long PVisit_Id { get; set; }
        public int Patient_Id { get; set; }
        public string PatientClass { get; set; }
        public Nullable<int> NursingStationId { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string Floor { get; set; }
        public string AdmissionType { get; set; }
        public string PreAdmitNumber { get; set; }
        public Nullable<int> PriorNursingStationId { get; set; }
        public string PriorRoom { get; set; }
        public string PriorBed { get; set; }
        public Nullable<int> PriorFacilityId { get; set; }
        public string PriorFloor { get; set; }
        public string PrimaryPhysicianNPI { get; set; }
        public string PrimaryPhysicianLName { get; set; }
        public string PrimaryPhysicianFName { get; set; }
        public string ReferringDoctor { get; set; }
        public string ConsultingDoctor { get; set; }
        public string HospitalService { get; set; }
        public string TemporaryLocation { get; set; }
        public string PreAdmitTestIndicator { get; set; }
        public string ReAdmissionIndicator { get; set; }
        public string AdmitSource { get; set; }
        public string AmbulatoryStatus { get; set; }
        public string VIPIndicator { get; set; }
        public string AdmittingDoctor { get; set; }
        public string PatientType { get; set; }
        public string VisitNumber { get; set; }
        public string FinancialClass { get; set; }
        public string ChargePriceIndicator { get; set; }
        public string CourtesyCode { get; set; }
        public string CreditRating { get; set; }
        public string ContractCode { get; set; }
        public Nullable<System.DateTime> ContractEffDate { get; set; }
        public string ContractAmount { get; set; }
        public string ContractPeriod { get; set; }
        public string InterestCode { get; set; }
        public string BadDebtCode { get; set; }
        public Nullable<System.DateTime> BadDebtDate { get; set; }
        public string BadDebtAgencyCode { get; set; }
        public string BadDebtTransferAmt { get; set; }
        public string BadDebtRecoveryAmt { get; set; }
        public string DeleteAccIndicator { get; set; }
        public Nullable<System.DateTime> DeleteAccDate { get; set; }
        public string DischargeDisposition { get; set; }
        public string DischargedLocation { get; set; }
        public string DietType { get; set; }
        public string ServicingFacility { get; set; }
        public string BedStatus { get; set; }
        public string AccStatus { get; set; }
        public string PendingLocation { get; set; }
        public string PriorTemporaryLocation { get; set; }
        public Nullable<System.DateTime> AdmitDate { get; set; }
        public Nullable<System.DateTime> DischargeDate { get; set; }
        public string CurrentPatientBalance { get; set; }
        public string TotalCharges { get; set; }
        public string TotalAdjustments { get; set; }
        public string TotalPayments { get; set; }
        public string AlternateVisitId { get; set; }
        public string VisitIndicator { get; set; }
        public string OtherHealthProvider { get; set; }
        public int PVisit_Status { get; set; }
        public Nullable<int> PVisit_CreatedBy { get; set; }
        public System.DateTime PVisit_CreatedDate { get; set; }
        public Nullable<int> PVOutBoundFileStatus { get; set; }
        public Nullable<int> PVOutBoundApproval { get; set; }
        public Nullable<int> PVOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PVOutBoundApprovalOn { get; set; }
        public Nullable<int> Wing { get; set; }
        public Nullable<int> Physician_Id { get; set; }
        public int DefaultPhysicianFlag { get; set; }

        public virtual DemographicEntity Demographic { get; set; }
     
    }

    public class VisitInfoCustomEntity
    {
        public long PVisit_Id { get; set; }
        public int Patient_Id { get; set; }
        public string PatientClass { get; set; }
        public Nullable<int> NursingStationId { get; set; }
        public string Room { get; set; }
        public string Bed { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string Floor { get; set; }
        public string AdmissionType { get; set; }
        public string PreAdmitNumber { get; set; }
        public Nullable<int> PriorNursingStationId { get; set; }
        public string PriorRoom { get; set; }
        public string PriorBed { get; set; }
        public Nullable<int> PriorFacilityId { get; set; }
        public string PriorFloor { get; set; }
        public string PrimaryPhysicianNPI { get; set; }
        public string PrimaryPhysicianLName { get; set; }
        public string PrimaryPhysicianFName { get; set; }
        public string ReferringDoctor { get; set; }
        public string ConsultingDoctor { get; set; }
        public string HospitalService { get; set; }
        public string TemporaryLocation { get; set; }
        public string PreAdmitTestIndicator { get; set; }
        public string ReAdmissionIndicator { get; set; }
        public string AdmitSource { get; set; }
        public string AmbulatoryStatus { get; set; }
        public string VIPIndicator { get; set; }
        public string AdmittingDoctor { get; set; }
        public string PatientType { get; set; }
        public string VisitNumber { get; set; }
        public string FinancialClass { get; set; }
        public string ChargePriceIndicator { get; set; }
        public string CourtesyCode { get; set; }
        public string CreditRating { get; set; }
        public string ContractCode { get; set; }
        public Nullable<System.DateTime> ContractEffDate { get; set; }
        public string ContractAmount { get; set; }
        public string ContractPeriod { get; set; }
        public string InterestCode { get; set; }
        public string BadDebtCode { get; set; }
        public Nullable<System.DateTime> BadDebtDate { get; set; }
        public string BadDebtAgencyCode { get; set; }
        public string BadDebtTransferAmt { get; set; }
        public string BadDebtRecoveryAmt { get; set; }
        public string DeleteAccIndicator { get; set; }
        public Nullable<System.DateTime> DeleteAccDate { get; set; }
        public string DischargeDisposition { get; set; }
        public string DischargedLocation { get; set; }
        public string DietType { get; set; }
        public string ServicingFacility { get; set; }
        public string BedStatus { get; set; }
        public string AccStatus { get; set; }
        public string PendingLocation { get; set; }
        public string PriorTemporaryLocation { get; set; }
        public Nullable<System.DateTime> AdmitDate { get; set; }
        public Nullable<System.DateTime> DischargeDate { get; set; }
        public string CurrentPatientBalance { get; set; }
        public string TotalCharges { get; set; }
        public string TotalAdjustments { get; set; }
        public string TotalPayments { get; set; }
        public string AlternateVisitId { get; set; }
        public string VisitIndicator { get; set; }
        public string OtherHealthProvider { get; set; }
        public int PVisit_Status { get; set; }
        public Nullable<int> PVisit_CreatedBy { get; set; }
        public System.DateTime PVisit_CreatedDate { get; set; }
        public Nullable<int> PVOutBoundFileStatus { get; set; }
        public Nullable<int> PVOutBoundApproval { get; set; }
        public Nullable<int> PVOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PVOutBoundApprovalOn { get; set; }
        public Nullable<int> Wing { get; set; }
        public Nullable<int> Physician_Id { get; set; }
        public int ApprovalPVisit_Id { get; set; }
    }
}
