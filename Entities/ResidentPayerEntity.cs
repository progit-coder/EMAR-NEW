using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class ResidentPayerEntity
    {
        public string ResidentID { get; set; }
        public Nullable<long> ContactID { get; set; }
        public Nullable<long> StayID { get; set; }
        public string PayerType { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<long> Priority { get; set; }
        public string PolicyNumber { get; set; }
        public string GroupNumber { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
