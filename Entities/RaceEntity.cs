using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class RaceEntity
    {
        public int Race_Id { get; set; }
        public string Race_Desc { get; set; }
        public int Race_Status { get; set; }
        public int Race_CreatedBy { get; set; }
        public System.DateTime Race_CreatedDate { get; set; }
    }
}
