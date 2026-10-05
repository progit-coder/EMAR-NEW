using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ControlSubstanceFilter
    {
        public int Company_Id { get; set; }
        public List<int> Facilities { get; set; }
        public List<int> NurseStations { get; set; }
        public List<int> Floors { get; set; }
        public List<int> Wings { get; set; }
        public List<int> Rooms { get; set; }
        public List<int> Beds { get; set; }
        public int History { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int User_Id { get; set; }
        public int CompanyToBedFlag { get; set; }
        public int ResidentId { get; set; }
    }
}
