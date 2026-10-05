using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    
    public partial class AllergyInfoEntity
    {
        public int PAllergy_Id { get; set; }
        public int Patient_Id { get; set; }
        public Nullable<int> AllergyType_Id { get; set; }
        public Nullable<int> ClassDrug_Id { get; set; }
        public string ClassDrug_Name { get; set; }
        public string NameOfCoding { get; set; }
        public string AllergySeverityCode { get; set; }
        public string AllergyReactionCode { get; set; }
        public Nullable<System.DateTime> AllergyIdentificationDate { get; set; }
        public int PAllergy_Status { get; set; }
        public Nullable<int> PAllergy_CreatedBy { get; set; }
        public System.DateTime PAllergy_CreatedDate { get; set; }
        public Nullable<int> ClassDrugType { get; set; }
        public Nullable<int> PAOutBoundFileStatus { get; set; }
        public Nullable<int> PAOutBoundApproval { get; set; }
        public Nullable<int> PAOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PAOutBoundApprovalOn { get; set; }
     
    }
}
