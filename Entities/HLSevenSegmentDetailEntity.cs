using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class HLSevenSegmentDetailEntity
    {
        
        public HLSevenSegmentDetailEntity()
        {
            this.HLSevenCompanyConfigs = new HashSet<HLSevenCompanyConfigEntity>();
        }

        public int SegDetail_Id { get; set; }
        public int Segment_Id { get; set; }
        public string SegDetail_Desc { get; set; }
        public int SegDetail_Status { get; set; }
        public Nullable<int> SegDetail_CreatedBy { get; set; }
        public System.DateTime SegDetail_CreatedDate { get; set; }
        public Nullable<int> SegDetail_Length { get; set; }
        public Nullable<int> SegDetail_Sequence { get; set; }
        public string SegDetail_ColumnID { get; set; }
        public string File_Values { get; set; }

        public virtual HLSevenSegmentEntity HLSevenSegment { get; set; }
        [JsonIgnore]
        public virtual ICollection<HLSevenCompanyConfigEntity> HLSevenCompanyConfigs { get; set; }        
    }
}
