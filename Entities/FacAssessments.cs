using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FacAssessments
    {
        public int Facility_Id { get; set; }
        public Nullable<int> AssmtLevelBasis { get; set; }
        public int FacOther_CreatedBy { get; set; }
        public System.DateTime FacOther_CreatedDate { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
