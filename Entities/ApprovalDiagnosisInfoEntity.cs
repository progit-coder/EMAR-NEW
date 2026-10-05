namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ApprovalDiagnosisInfoEntity
    {
        public int ApprovalPDiagnosis_Id { get; set; }
        public Nullable<int> PDiagnosis_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string CodingMethod { get; set; }
        public Nullable<int> ICD10_Id { get; set; }
        public Nullable<int> DCodingType_Id { get; set; }
        public string AltCodingId { get; set; }
        public string AltCodingText { get; set; }
        public string AltCodingMethod { get; set; }
        public string DiagnosisDescription { get; set; }
        public Nullable<System.DateTime> DiagnosisDate { get; set; }
        public string DiagnosisType { get; set; }
        public string MajorDiagnosticId { get; set; }
        public string MajorDiagnosticText { get; set; }
        public string MDCodingSystem { get; set; }
        public string MDAlternateId { get; set; }
        public string MDAlternateText { get; set; }
        public string MDAltCodingSystem { get; set; }
        public string DiagnosticGroupId { get; set; }
        public string DiagnosticGroupText { get; set; }
        public string DGCodingSystem { get; set; }
        public string DGAlternateId { get; set; }
        public string DGAlternateText { get; set; }
        public string DGAltCodingSystem { get; set; }
        public string DRGApprovalIndicator { get; set; }
        public string DRGGrouperReviewCode { get; set; }
        public string OutlierId { get; set; }
        public string OutlierText { get; set; }
        public string OutlierCodingSystem { get; set; }
        public string OAlternateId { get; set; }
        public string OAlternateText { get; set; }
        public string OAltCodingSystem { get; set; }
        public string OutlierDays { get; set; }
        public string OutlierQuantity { get; set; }
        public string OutlierDenomination { get; set; }
        public string PriceType { get; set; }
        public string FromValue { get; set; }
        public string ToValue { get; set; }
        public string RangeId { get; set; }
        public string RangeText { get; set; }
        public string RangeCodingSystem { get; set; }
        public string RangeAltId { get; set; }
        public string RangeAltText { get; set; }
        public string RangeAltCodingSystem { get; set; }
        public string RangeType { get; set; }
        public string GrouperVersion { get; set; }
        public string DiagnosisPriority { get; set; }
        public string DiagnosisClassification { get; set; }
        public string ConfidentialIndicator { get; set; }
        public Nullable<System.DateTime> AttestationDate { get; set; }
        public string DiagnosisIdentifier { get; set; }
        public string DiagnosisActionCode { get; set; }
        public int PDiagnosis_Status { get; set; }
        public Nullable<int> PDiagnosis_CreatedBy { get; set; }
        public System.DateTime PDiagnosis_CreatedDate { get; set; }
        public Nullable<int> PDGOutBoundFileStatus { get; set; }
        public Nullable<int> PDGOutBoundApproval { get; set; }
        public Nullable<int> PDGOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PDGOutBoundApprovalOn { get; set; }
        public string PhysicianNPI { get; set; }
        public string PhysicianLName { get; set; }
        public string PhysicianFName { get; set; }
    }
}
