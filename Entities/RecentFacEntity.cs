using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public  class RecentFacEntity
    {
        public int RecFac_Id { get; set; }
        public int User_Id { get; set; }
        public int Facility_Id { get; set; }
        public string NurseStation_Id { get; set; }
        public int companyId { get; set; }
    }
}
