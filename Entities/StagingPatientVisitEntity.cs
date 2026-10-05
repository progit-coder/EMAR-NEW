using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingPatientVisitEntity
    {
        public long StagPatVisitId { get; set; }
        public long StagPatient_Id { get; set; }
        public string PatientClass { get; set; }
        public string PatientLocation { get; set; }
        public string AdmissionType { get; set; }
        public string PreAdmitNumber { get; set; }
        public string PriorLocation { get; set; }
        public string AttendingDoctor { get; set; }
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
        public Nullable<decimal> ContractAmount { get; set; }
        public Nullable<decimal> ContractPeriod { get; set; }
        public string InterestCode { get; set; }
        public string BadDebtCode { get; set; }
        public Nullable<System.DateTime> BadDebtDate { get; set; }
        public string BadDebtAgencyCode { get; set; }
        public Nullable<decimal> BadDebtTransferAmt { get; set; }
        public Nullable<decimal> BadDebtRecoveryAmt { get; set; }
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
        public Nullable<decimal> CurrentPatientBalance { get; set; }
        public Nullable<decimal> TotalCharges { get; set; }
        public Nullable<decimal> TotalAdjustments { get; set; }
        public Nullable<decimal> TotalPayments { get; set; }
        public string AlternateVisitId { get; set; }
        public string VisitIndicator { get; set; }
        public string OtherHealthProvider { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
