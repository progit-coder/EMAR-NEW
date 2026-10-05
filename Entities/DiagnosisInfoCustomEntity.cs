using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DiagnosisInfoCustomEntity
    {
        public int PDiagnosis_Id { get; set; }
        public int Patient_Id { get; set; }

        public int ICD10_Id { get; set; }
        public string CodingMethod { get; set; }
        public int DCodingType_Id { get; set; }

        public string DiagnosisDescription { get; set; }
        public string AltCodingId { get; set; }

        public string AltCodingText { get; set; }

        public string AltCodingMethod { get; set; }

        public DateTime DiagnosisDate { get; set; }

        public int PDiagnosis_Status { get; set; }
        public Nullable<int> PDiagnosis_CreatedBy { get; set; }
        public DateTime PDiagnosis_CreatedDate { get; set; }
        public Nullable<int> PDGOutBoundFileStatus { get; set; }
        public Nullable<int> PDGOutBoundApproval { get; set; }
        public Nullable<int> PDGOutBoundApprovalBy { get; set; }
        public DateTime PDGOutBoundApprovalOn { get; set; }
        public string CreatedBy { get; set; }

    }
}
