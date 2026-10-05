using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NursingFrequencyConfig1Entity
    {
        public int AuditNursingFreq_Id { get; set; }
        public Nullable<int> NursingFreq_Id { get; set; }
        public int NursingStation_Id { get; set; }
        public int Floor_Id { get; set; }
        public int Wing_Id { get; set; }
        public int Frequency_Id { get; set; }
        public Nullable<int> StartTime { get; set; }
        public int TimeFormat_ID { get; set; }
        public Nullable<int> Hours { get; set; }
        public Nullable<bool> Monday { get; set; }
        public Nullable<bool> Tuesday { get; set; }
        public Nullable<bool> Wednesday { get; set; }
        public Nullable<bool> Thursday { get; set; }
        public Nullable<bool> Friday { get; set; }
        public Nullable<bool> Saturday { get; set; }
        public Nullable<bool> Sunday { get; set; }
        public Nullable<int> Week_Id { get; set; }
        public Nullable<int> Month_Id { get; set; }
        public Nullable<int> OnlyOnDay { get; set; }
        public Nullable<int> ThroughDay { get; set; }
        public Nullable<int> ActiveDays { get; set; }
        public Nullable<int> HoldDays { get; set; }
        public int NursingFreq_Status { get; set; }
        // public Nullable<int> NursingFreq_UpdatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> NursingFreq_UpdatedDate { get; set; }
    }
}
