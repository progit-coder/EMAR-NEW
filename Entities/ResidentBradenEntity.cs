using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentBradenEntity
    {
        public int ResidentId { get; set; }
        public long StayID { get; set; }
        public Nullable<System.DateTime> AssmtDate { get; set; }
        public Nullable<int> SensoryPerception { get; set; }
        public Nullable<int> Moisture { get; set; }
        public Nullable<int> Activity { get; set; }
        public Nullable<int> Mobility { get; set; }
        public Nullable<int> Nutrition { get; set; }
        public Nullable<int> FrictionAndShear { get; set; }
        public string Signature { get; set; }
        public Nullable<System.DateTime> DateSigned { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
