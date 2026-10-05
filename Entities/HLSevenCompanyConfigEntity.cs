using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class HLSevenCompanyConfigEntity
    {
        public int HLConfig_Id { get; set; }
        public int Company_Id { get; set; }
        public int SegDetail_Id { get; set; }
        public int SegDetailConfig_Id { get; set; }
        public Nullable<int> SegDisplay_Id { get; set; }
        public int HLConfig_Status { get; set; }
        public Nullable<int> HLConfig_CreatedBy { get; set; }
        public System.DateTime HLConfig_CreatedDate { get; set; }

        public string SegDetail_Desc { get; set; }

    }

    public partial class HLSevenCloneEntity
    {
        public int InputCompanyId { get; set; }
        public int OutputCompanyId { get; set; }
        public int SegmentId { get; set; }
        public int CreatedBy { get; set; }
    }
}
