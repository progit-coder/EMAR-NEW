using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class FacCarePlanEntity
    {
        public int FacCarePlan_Id { get; set; }
        public int Facility_Id { get; set; }
        public Nullable<int> CP1ProbPerPageCPRpt { get; set; }
        public Nullable<int> CPEffDtCNMoFlowS { get; set; }
        public Nullable<int> CPPELFlowS { get; set; }
        public Nullable<int> CPPL { get; set; }
        public Nullable<int> EBLines { get; set; }
        public Nullable<int> RTLines { get; set; }
        public Nullable<int> GLines { get; set; }
        public Nullable<int> ILines { get; set; }
        public Nullable<int> FSLines { get; set; }
        public int FacCarePlan_CreatedBy { get; set; }
        public System.DateTime FacCarePlan_CreatedDate { get; set; }

    }
}
