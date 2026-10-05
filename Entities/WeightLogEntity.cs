namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class WeightLogEntity
    {
        public int WeightLog_ID { get; set; }
        public int Patient_Id { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Type { get; set; }
        public string HeightFeet { get; set; }
        public string HeightInc { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public Nullable<int> PreDialysis { get; set; }
        public Nullable<int> PostDialysis { get; set; }
        public string Remarks { get; set; }
        public string IBW { get; set; }
        public string initials { get; set; }
        public int WeightLog_Status { get; set; }
        public Nullable<int> WeightLog_CreatedBy { get; set; }
        public Nullable<System.DateTime> WeightLog_CreatedOn { get; set; }
        public string UserName { get; set; }
        
    }
}
