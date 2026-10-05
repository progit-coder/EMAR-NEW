using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AwarenessRatingEntity
    {
        public int ARating_Id { get; set; }
        public string ARating_Code { get; set; }
        public string ARating_Desc { get; set; }
        public int ARating_Status { get; set; }
        public int ARating_CreatedBy { get; set; }
        public System.DateTime ARating_CreatedDate { get; set; }
    }
}
