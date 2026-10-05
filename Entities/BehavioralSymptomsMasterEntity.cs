using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class BehavioralSymptomsMasterEntity
    {
        public int BehavioralSym_ID { get; set; }
        public string BehavioralSym_Desc { get; set; }
        public int BehavioralSym_Status { get; set; }
        public Nullable<int> BehavioralSym_CreatedBy { get; set; }
        public System.DateTime BehavioralSym_CreatedOn { get; set; }
    }
}
