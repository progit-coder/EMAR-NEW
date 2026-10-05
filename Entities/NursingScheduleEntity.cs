using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NursingScheduleEntity
    {
        public int NursingSchedule_Id { get; set; }
        public int NursingStation_Id { get; set; }
        public string ScheduleTime { get; set; }
        public int NursingSchedule_Status { get; set; }
        public Nullable<int> NursingSchedule_CreatedBy { get; set; }
        public Nullable<System.DateTime> NursingSchedule_CreatedDate { get; set; }

        //public virtual NursingStationEntity NursingStation { get; set; }
        //public virtual UserEntity User { get; set; }
    }
    public class NursingScheduleDropEntity
    {
        public int NursingSchedule_Id { get; set; }
        public string ScheduleTime { get; set; }
        public string NurseShiftTime { get; set; }
    }
}
