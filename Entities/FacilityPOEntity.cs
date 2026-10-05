using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FacilityPOEntity
    {
        public int FacilityPO_Id { get; set; }
        public int Facility_Id { get; set; }
        public Nullable<int> POPECLFlowS { get; set; }
        public Nullable<int> POPELRpt { get; set; }
        public Nullable<int> POonRpt { get; set; }
        public Nullable<int> POonFlowS { get; set; }
        public Nullable<int> POonCPRpt { get; set; }
        public Nullable<int> POLibonRpt { get; set; }
        public Nullable<int> POLibonFlowS { get; set; }
        public Nullable<int> POLibonCPRpt { get; set; }
        public Nullable<int> POPL { get; set; }
        public Nullable<int> POSEonPO { get; set; }
        public Nullable<int> POSEonFlowS { get; set; }
        public Nullable<int> POExtraLines { get; set; }
        public Nullable<System.DateTime> POEffDtCNMoPO { get; set; }
        public Nullable<System.DateTime> POEffDtCNMoFlowS { get; set; }
        public int FacilityPO_CreatedBy { get; set; }
        public System.DateTime FacilityPO_CreatedDate { get; set; }
    }
}
