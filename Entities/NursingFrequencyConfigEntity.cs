using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class NursingFrequencyConfigEntity
    {
        public int NursingFreq_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> NursingStation_Id { get; set; }
        public Nullable<int> Floor_Id { get; set; }
        public Nullable<int> Wing_Id { get; set; }
        public Nullable<int> Frequency_Id { get; set; }
        public Nullable<int> StartTime { get; set; }
        public Nullable<int> TimeFormat_ID { get; set; }
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
        public Nullable<int> NursingFreq_CreatedBy { get; set; }
        public System.DateTime NursingFreq_CreatedDate { get; set; }
        public List<HourEntity> HoursList { get; set; }

        // public virtual Floor Floor { get; set; }
        //public virtual FrequencyMaster FrequencyMaster { get; set; }
        //public virtual Hour Hour { get; set; }
        //public virtual Month Month { get; set; }
        //public virtual NursingStation NursingStation { get; set; }
        //public virtual TimeFormat TimeFormat { get; set; }
        //public virtual User User { get; set; }
        //public virtual Week Week { get; set; }
        //public virtual Wing Wing { get; set; }
        //public virtual FrequencyMaster FrequencyMaster1 { get; set; }
    }
}
