using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrdersCustomFilterEntity
    {
        public string ControlType { get; set; }
        public string Type { get; set; }
        public List<int> Facilities { get; set; }
        public List<int> NurseStations { get; set; }
        public List<int> Floors { get; set; }
        public List<int> Wings { get; set; }
        public List<int> Rooms { get; set; }
        public List<int> Beds { get; set; }
        public int CompanyID { get; set; }
        public int User_Id { get; set; }
        public int VisitStatus { get; set; }
        public int patientId { get; set; }
    }
}
