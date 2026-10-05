using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class FacADTDefaultEntity
    {
        public int FacAdt_Id { get; set; }
        public int Facility_Id { get; set; }
        public Nullable<bool> ADTAdvDirFaceS { get; set; }
        public Nullable<bool> ADTRespFaceS { get; set; }
        public Nullable<bool> ADTPADFaceS { get; set; }
        public Nullable<int> ADTPersContFaceSSect { get; set; }
        public Nullable<bool> ADTPersContPWFaceS { get; set; }
        public Nullable<bool> ADTPersContPHFaceS { get; set; }
        public Nullable<bool> ADTPersContPFFaceS { get; set; }
        public Nullable<bool> ADTPersContPPFaceS { get; set; }
        public Nullable<bool> ADTPersContPCFaceS { get; set; }
        public Nullable<bool> ADTPersContPMFaceS { get; set; }
        public Nullable<bool> ADTProfContFaceSSect { get; set; }
        public Nullable<bool> ADTProfContPWFaceS { get; set; }
        public Nullable<bool> ADTProfContPHFaceS { get; set; }
        public Nullable<bool> ADTProfContPFFaceS { get; set; }
        public Nullable<bool> ADTProfContPPFaceS { get; set; }
        public Nullable<bool> ADTProfContPCFaceS { get; set; }
        public Nullable<bool> ADTProfContPMFaceS { get; set; }
        public Nullable<bool> ADTADFaceSSect { get; set; }
        public Nullable<bool> ADTADCPFlowS { get; set; }
        public Nullable<bool> ADTADPORpt { get; set; }
        public Nullable<bool> ADTADPOFlowS { get; set; }
        public Nullable<bool> ADTIDFaceSSect { get; set; }
        public Nullable<bool> ADTIDCPFlowS { get; set; }
        public Nullable<bool> ADTIDPORpt { get; set; }
        public Nullable<bool> ADTIDPOFlowS { get; set; }
        public Nullable<bool> ADTDDFaceSSect { get; set; }
        public Nullable<bool> ADTDDCPFlowS { get; set; }
        public Nullable<bool> ADTDDPORpt { get; set; }
        public Nullable<bool> ADTDDPOFlowS { get; set; }
        public Nullable<int> ADTPL { get; set; }
        public Nullable<bool> ADTADMedCert { get; set; }
        public Nullable<bool> ADTIDMedCert { get; set; }
        public Nullable<bool> ADTDDMedCert { get; set; }
        public int FacAdt_CreatedBy { get; set; }
        public System.DateTime FacAdt_CreatedDate { get; set; }
    }
}
