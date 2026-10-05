using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AncillaryDetailEntity
    {
        public int PAnc_Id { get; set; }
        public int Patient_Id { get; set; }
        public string LiteralOrderCode { get; set; }
        public string LiteralOrderDesc { get; set; }
        public string MAROrderCatSeq { get; set; }
        public string POOrderCatSeq { get; set; }
        public string TAROrderCatSeq { get; set; }
        public string Filler { get; set; }
        public string IncludeOnMAR { get; set; }
        public string IncludeOnPO { get; set; }
        public string IncludeOnTAR { get; set; }
        public string RawAdministrationTimes { get; set; }
        public int PAnc_Status { get; set; }
        public Nullable<int> PAnc_CreatedBy { get; set; }
        public System.DateTime PAnc_CreatedDate { get; set; }

        public virtual DemographicEntity Demographic { get; set; }
    }
}
