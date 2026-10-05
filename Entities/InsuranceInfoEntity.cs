using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class InsuranceInfoEntity
    {
        public int PatIns_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string InsIdentifier { get; set; }
        public string InsPlanName { get; set; }
        public string CodingSystemName { get; set; }
        public string InsCompanyID { get; set; }
        public string InsCompanyName { get; set; }
        public string InsCompanyAddress1 { get; set; }
        public string InsCompanyAddress2 { get; set; }
        public string InsCompanyCity { get; set; }
        public string InsCompanyState { get; set; }
        public string InsCompanyZipCode { get; set; }
        public string InsContactPerson { get; set; }
        public string InsPhoneNumber { get; set; }
        public string InsGroupNumber { get; set; }
        public string InsGroupName { get; set; }
        public string InsGroupEmpId { get; set; }
        public string InsGroupEmpName { get; set; }
        public string PlanEffDate { get; set; }
        public string PlanExpDate { get; set; }
        public string AuthorizationInfo { get; set; }
        public string PlanType { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string InsRelationIdentifier { get; set; }
        public string InsRelationText { get; set; }
        public string InsuredDob { get; set; }
        public string InsuredAddress { get; set; }
        public string BenefitAssignment { get; set; }
        public string BenefitCoordination { get; set; }
        public string BenefitCoordinationPriority { get; set; }
        public string AdmissionFlag { get; set; }
        public string AdmissionDate { get; set; }
        public string EligibilityFlag { get; set; }
        public string EligibilityDate { get; set; }
        public string ReleaseInfoCode { get; set; }
        public string PAC { get; set; }
        public string VerificationDate { get; set; }
        public string VerificationBy { get; set; }
        public string AgreementCode { get; set; }
        public string BillingStatus { get; set; }
        public Nullable<int> ReserveDays { get; set; }
        public Nullable<int> DelayReserveDays { get; set; }
        public string CompanyPlanCode { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyDeductibles { get; set; }
        public string PolicyLimitAmount { get; set; }
        public Nullable<int> PolicyLimitDays { get; set; }
        public string RoomRateSemiPrivate { get; set; }
        public string RoomRatePrivate { get; set; }
        public string InsuredEmpStatus { get; set; }
        public string InsuredSex { get; set; }
        public string InsuredEmpAddress { get; set; }
        public string VerificationStatus { get; set; }
        public string PriorInsPlanId { get; set; }
        public string CoverageType { get; set; }
        public string Handicap { get; set; }
        public string InsuredIdNumber { get; set; }
        public string SignatureCode { get; set; }
        public string SignatureCodeDate { get; set; }
        public string InsuredBirthPlace { get; set; }
        public string VipIndicator { get; set; }
        public int PatIns_Status { get; set; }
        public Nullable<int> PatIns_CreatedBy { get; set; }
        public System.DateTime PatIns_CreatedDate { get; set; }

        public virtual DemographicEntity Demographic { get; set; }
    }
}
