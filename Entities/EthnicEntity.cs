using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class EthnicEntity
    {
        public int Ethnic_Id { get; set; }
        public int Ethnic_OriginCode { get; set; }
        public string Ethnic_OriginShortCode { get; set; }
        public string Ethnic_Description { get; set; }
        public int Ethnic_Status { get; set; }
        public int Ethnic_CreatedBy { get; set; }
        public System.DateTime Ethnic_CreatedDate { get; set; }
    }
}
