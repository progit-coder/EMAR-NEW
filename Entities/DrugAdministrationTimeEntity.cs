using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DrugAdministrationTimeEntity
    {
        public int DAdminId { get; set; }
        public int POrderId { get; set; }
        public Nullable<int> AdministrationType { get; set; }
        public Nullable<int> NursingFreqId { get; set; }
        public string HourId { get; set; }
        public Nullable<int> TimeFormatId { get; set; }
        public Nullable<int> Hours { get; set; }
        public Nullable<bool> Monday { get; set; }
        public Nullable<bool> Tuesday { get; set; }
        public Nullable<bool> Wednesday { get; set; }
        public Nullable<bool> Thursday { get; set; }
        public Nullable<bool> Friday { get; set; }
        public Nullable<bool> Saturday { get; set; }
        public Nullable<bool> Sunday { get; set; }
        public string WeekId { get; set; }
        public string MonthId { get; set; }
        public Nullable<int> OnlyOnDay { get; set; }
        public Nullable<int> ThroughDay { get; set; }
        public Nullable<int> ActiveDays { get; set; }
        public Nullable<int> HoldDays { get; set; }
        public int DAdminStatus { get; set; }
        public Nullable<int> DAdminCreatedBy { get; set; }
        public Nullable<System.DateTime> DAdminCreatedDate { get; set; }
        public Nullable<int> PQuantityId { get; set; }
        public Nullable<int> ActiveDay { get; set; }
        public string Days { get; set; }
        public List<HourEntity> HoursList { get; set; }
        public string NurseShiftsId { get; set; }
        public string Freq_Group { get; set; }
    }
}
