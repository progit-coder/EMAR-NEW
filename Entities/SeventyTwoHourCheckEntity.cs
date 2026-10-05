using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public partial class SeventyTwoHourCheckEntity
    {
        public Nullable<System.DateTime> AdministerOn{ get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string GiveCodeText { get; set; }
        public string ResidentName { get; set; }
        public int Porder_Id { get; set; }
        public int Patient_Id { get; set; }
        public Nullable<System.DateTime> AdminsterSchedule { get; set; }
        public Nullable<System.DateTime> AdminsterOn { get; set; }
        public int DrugAdminister_Id { get; set; }
        //public string SeventyTwoComment { get; set; }
        //public int SeventyTwoCommentBy { get; set; }
        //public Nullable<System.DateTime> SeventyTwoCommentOn { get; set; }
        //public string UserName { get; set; }
    }
    public partial class SeventyTwoHourInsertEntity
    {
        public Int64 DrugAdminister_Id { get; set; }
        public string SeventyTwoComment { get; set; }
        public int SeventyTwoCommentBy { get; set; }
        public DateTime SeventyTwoCommentOn { get; set; }
    }
    public partial class SeventyTwoHourCustomEntity
    {
        public int User_Id { get; set; }
        public int Company_Id { get; set; }
        public List<int> Facilities { get; set; }
        public List<int> NurseStations { get; set; }
        public List<int> Floors { get; set; }
        public List<int> Wings { get; set; }
        public List<int> Rooms { get; set; }
        public List<int> Beds { get; set; }

    }
}
