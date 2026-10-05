using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingPatientInEntity
    {
        public int PatIns_Id { get; set; }
        public int StagPatVisitId { get; set; }
        public string InsPlanId { get; set; }
        public string InsCompanyID { get; set; }
        public string InsCompanyName { get; set; }
        public string InsCompanyAddress { get; set; }
        public string InsContactPerson { get; set; }
        public string InsPhoneNumber { get; set; }
        public string InsGroupNumber { get; set; }
        public string InsGroupName { get; set; }
        public string InsGroupEmpId { get; set; }
        public string InsGroupEmpName { get; set; }
        public Nullable<System.DateTime> PlanEffDate { get; set; }
        public Nullable<System.DateTime> PlanExpDate { get; set; }
        public string AuthorizationInfo { get; set; }
        public string PlanType { get; set; }
        public string InsuredName { get; set; }
        public string InsRelationship { get; set; }
        public Nullable<System.DateTime> InsuredDob { get; set; }
        public string InsuredAddress { get; set; }
        public string BenefitAssignment { get; set; }
        public string BenefitCoordination { get; set; }
        public string BenefitCoordinationPriority { get; set; }
        public string AdmissionFlag { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public string EligibilityFlag { get; set; }
        public Nullable<System.DateTime> EligibilityDate { get; set; }
        public string ReleaseInfoCode { get; set; }
        public string PAC { get; set; }
        public Nullable<System.DateTime> VerificationDate { get; set; }
        public string VerificationBy { get; set; }
        public string AgreementCode { get; set; }
        public string BillingStatus { get; set; }
        public Nullable<int> ReserveDays { get; set; }
        public Nullable<int> DelayReserveDays { get; set; }
        public string CompanyPlanCode { get; set; }
        public string PolicyNumber { get; set; }
        public Nullable<decimal> PolicyDeductibles { get; set; }
        public Nullable<decimal> PolicyLimitAmount { get; set; }
        public Nullable<int> PolicyLimitDays { get; set; }
        public Nullable<decimal> RoomRateSemiPrivate { get; set; }
        public Nullable<decimal> RoomRatePrivate { get; set; }
        public string InsuredEmpStatus { get; set; }
        public string InsuredSex { get; set; }
        public string InsuredEmpAddress { get; set; }
        public string VerificationStatus { get; set; }
        public string PriorInsPlanId { get; set; }
        public string CoverageType { get; set; }
        public string Handicap { get; set; }
        public string InsuredIdNumber { get; set; }
        public string SignatureCode { get; set; }
        public Nullable<System.DateTime> SignatureCodeDate { get; set; }
        public string InsuredBirthPlace { get; set; }
        public string VipIndicator { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
