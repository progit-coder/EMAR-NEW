namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ApprovalDemographicEntity
    {
        public int ApprovalPatient_Id { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public string ExternalPatientId { get; set; }
        public string ExternalFacShortName { get; set; }
        public string ExternalFacPatientId { get; set; }
        public Nullable<int> AlternatePatientId { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMiddleInitial { get; set; }
        public string NameTypeCode { get; set; }
        public string MotherMaidenName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string AdministrativeSex { get; set; }
        public string PatientAlias { get; set; }
        public string Race { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientAddress2 { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        public string CountyCode { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneBusiness { get; set; }
        public string PrimaryLanguage { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string PatientMRNumber { get; set; }
        public string SSN { get; set; }
        public string DriverLicense { get; set; }
        public string MotherIdentifier { get; set; }
        public string EthnicGroup { get; set; }
        public string BirthPlace { get; set; }
        public string MultipleBirthIndicator { get; set; }
        public string BirthOrder { get; set; }
        public string Citizenship { get; set; }
        public string MilitaryStatus { get; set; }
        public string Nationality { get; set; }
        public Nullable<System.DateTime> DeathDateTime { get; set; }
        public string DeathIndicator { get; set; }
        public string IdentityIndicator { get; set; }
        public string IdentityReliability { get; set; }
        public string LastUpdate { get; set; }
        public string LastFacilityUpdate { get; set; }
        public string SpeciesCode { get; set; }
        public string BreedCode { get; set; }
        public string Strain { get; set; }
        public string ProductionClassCode { get; set; }
        public string TribalCitizenship { get; set; }
        public string ImageLocation { get; set; }
        public int Patient_Status { get; set; }
        public Nullable<int> Patient_CreatedBy { get; set; }
        public System.DateTime Patient_CreatedDate { get; set; }
        public Nullable<int> PDOutBoundFileStatus { get; set; }
        public Nullable<int> PDOutBoundApproval { get; set; }
        public Nullable<int> PDOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PDOutBoundApprovalOn { get; set; }
        public string Alert { get; set; }
        public string Diet { get; set; }
        public string BiometricInfo { get; set; }
    }
    public class NewResidentInfoEntity
    {
        public DemographicCustomEntity DemographicDetails { get; set; }
        public VisitInfoCustomEntity VisitInfoDetails { get; set; }
        public string ResidentUniqueId { get; set; }
    }
    public class PatientReAdmissionEntity
    {
        public int PatientId { get; set; }
        public DateTime AdmitDate { get; set; }
    }
}
