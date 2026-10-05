using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NursingFrequencyConfigCustomEntity
    {
        public int NursingFreq_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public string NursingStations { get; set; }
        public string Times { get; set; }
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
        public string FacilityName { get; set; }
        public string NursingStation_Name { get; set; }
        public string Floor_Name { get; set; }
        public string Wing_Name { get; set; }
        public string Frequency_Name { get; set; }
        public string Hour_Desc { get; set; }
        public string TimeFormat_Desc { get; set; }
        public string StartTime_Desc { get; set; }
        public List<HourEntity> HoursList { get; set; }
        public int OldFrequencyId { get; set; }
        public int OldFacilityId { get; set; }
        public int OldNursingStationId { get; set; }

    }
}
