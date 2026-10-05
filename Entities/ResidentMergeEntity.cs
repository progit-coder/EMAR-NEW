using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentMergeEntity
    {
        public ResidentMergeDetails Merge1 { get; set; }
        public ResidentMergeDetails Merge2 { get; set; }
        public List<ResidentMergeVisitInfo> Visit1 { get; set; }
        public List<ResidentMergeVisitInfo> Visit2 { get; set; }
        public List<ResidentMergeAllergies> Allergy1 { get; set; }
        public List<ResidentMergeAllergies> Allergy2 { get; set; }
        public List<ResidentMergeDiagnosis> Diagnosis1 { get; set; }
        public List<ResidentMergeDiagnosis> Diagnosis2 { get; set; }
    }
    public class ResidentMergeVisitInfo
    {
        public DateTime? AdmitDate { get; set; }
        public DateTime? DischargeDate { get; set; }
    }
    public class ResidentMergeDetails
    {
        public int PatientId { get; set; }
        public string ResidentName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ExternalPatientId { get; set; }
        public string ExternalFacPatientId { get; set; }
        public DateTime? AdmitDate { get; set; }
        public string PatientMRNumber {get;set;}
        public string Facilityname { get; set; }
    }
    public class ResidentMergeAllergies
    {
        public string Allergy { get; set; }
        public string Reaction { get; set; }
    }
    public class ResidentMergeDiagnosis
    {
        public string DiagnosisDesc { get; set; }
    }

    public class PostMergeDetails
    {
        public int patientId { get; set; }
        public int mergepatientId { get; set; }
        public string aliasName { get; set; }
        public int DetailsUpdate { get; set; }
        public int AllergyMerge { get; set; }
        public int DiagnosisMerge { get; set; }
    }
}
