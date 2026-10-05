namespace LTCPro.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class VisitVitalEntity
    {
        public long Vitals_ID { get; set; }
        public long PVisit_Id { get; set; }
        public Nullable<System.DateTime> VitalDate { get; set; }
        public string VitalTime { get; set; }
        public string CistolicBP { get; set; }
        public Nullable<int> DiastolicBP { get; set; }
        public Nullable<int> HeartRate { get; set; }
        public Nullable<int> RespiratoryRate { get; set; }
        public Nullable<int> OxygenRate { get; set; }
        public string Temperature { get; set; }
        public string Pain { get; set; }
        public string Remark { get; set; }
        public Nullable<int> PulseRate { get; set; }
        public string Initials { get; set; }
        public string PDAID { get; set; }
        public int VitalSigns_status { get; set; }
        public Nullable<int> VitalSigns_CreatedBy { get; set; }
        public Nullable<System.DateTime> VitalSigns_CreatedOn { get; set; }
        public string BloodSugar { get; set; }
        public string UserName { get; set; }
       
    }
}
