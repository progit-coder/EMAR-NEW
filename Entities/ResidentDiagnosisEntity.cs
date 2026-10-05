using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentDiagnosisEntity
    {       
        public Nullable<long> StayID { get; set; }
        public string ResidentID { get; set; }
        public string DiagCode { get; set; }
        public string DiagDesc { get; set; }
        public string DiagType { get; set; }
        public Nullable<bool> Resolved { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DiagnosisDate { get; set; }
        public Nullable<System.DateTime> ResolvedDate { get; set; }
        public string FCSectPrtFlg { get; set; }
        public Nullable<int> FCSeqFlg { get; set; }
        public Nullable<bool> PrimaryDiag { get; set; }
        public string PrintOnPORpt { get; set; }
        public string PrintOnPOFlowsheet { get; set; }
        public string PrintOnFaceSheet { get; set; }
        public string PrintOnCPRpt { get; set; }
        public string PrintOnCPFlowsheet { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<int> Version { get; set; }
        public Nullable<long> ConvertedFrom { get; set; }
    }
}
