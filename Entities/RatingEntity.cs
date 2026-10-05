using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class RatingEntity
    {
        public int Rating_Id { get; set; }
        public string Rating_Code { get; set; }
        public string Rating_Desc { get; set; }
        public int Rating_Status { get; set; }
        public int Rating_CreatedBy { get; set; }
        public System.DateTime Rating_CreatedDate { get; set; }
    }
}
