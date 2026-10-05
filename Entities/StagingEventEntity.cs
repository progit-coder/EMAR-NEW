using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{    
    public class StagingEventEntity
    {
        public int StagEvent_Id { get; set; }
        public int StagMsh_Id { get; set; }
        public string EventTypeCode { get; set; }
        public Nullable<System.DateTime> RecorderDateTime { get; set; }
        public Nullable<System.DateTime> PlannedEventDateTime { get; set; }
        public string EventReasonCode { get; set; }
        public string OperatorId { get; set; }
        public Nullable<System.DateTime> EventOccured { get; set; }
        public string EventFacility { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
