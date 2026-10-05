using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class HLSevenSegmentEntity
    {
        public int Segment_Id { get; set; }
        public string Segment_Desc { get; set; }
        public string Segment_Code { get; set; }
        public int Segment_Status { get; set; }
        public Nullable<int> Segment_CreatedBy { get; set; }
        public System.DateTime Segment_CreatedDate { get; set; }        
    }
    public class HLSevenOutboundDisplaySegmentEntity
    {
        public int OSegment_Id { get; set; }
        public string OSegment_Desc { get; set; }
        public int OSegment_Status { get; set; }
        public Nullable<int> OSegment_CreatedBy { get; set; }
        public System.DateTime OSegment_CreatedDate { get; set; }

    }
    public class CustomHLSevenOutboundDisplaySegmentGridDataEntity
    {
        public int OHLConfig_Id { get; set; }
        public int OSegDetail_Id { get; set; }
        public Nullable<int> DisplayConfigId { get; set; }
        public string OSegDetail_Desc { get; set; }
        public int OutboundHlsevenSettingsResult { get; set; }
        public Nullable<int> HlSequence { get; set; }
    }
    public class CustomHLSevenOutboundDisplayCompanyConfigEntity
    {
        public int OHLConfig_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public int OSegDetail_Id { get; set; }
        public Nullable<int> DisplayConfigId { get; set; }
        public Nullable<int> OHLConfig_Status { get; set; }
        public Nullable<int> OHLConfig_CreatedBy { get; set; }
        public Nullable<System.DateTime> OHLConfig_CreatedDate { get; set; }
        public int OSegment_Id { get; set; }
        public Nullable<int> HlSequence { get; set; }
        //  public virtual Company Company { get; set; }
        //  public virtual HLSevenOutboundDisplaySegmentDetail HLSevenOutboundDisplaySegmentDetail { get; set; }
        //  public virtual User User { get; set; }
    }
}
