using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingPatientEntity
    {
        public long StagPatient_Id { get; set; }
        public Nullable<int> File_Id { get; set; }
        public Nullable<int> SetID { get; set; }
        public Nullable<int> ExternalPat_Id { get; set; }
        public string PatientIdentfier { get; set; }
        public Nullable<int> AlternatePat_Id { get; set; }
        public string PatientName { get; set; }
        public string MotherMaidenName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string PatientAlias { get; set; }
        public string Race { get; set; }
        public string PatientAddress { get; set; }
        public string CountyCode { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneBusiness { get; set; }
        public string Language { get; set; }
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
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<System.DateTime> LastFacilityUpdate { get; set; }
        public string SpeciesCode { get; set; }
        public string BreedCode { get; set; }
        public string Strain { get; set; }
        public string ProductionClassCode { get; set; }
        public string TribalCitizenship { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
