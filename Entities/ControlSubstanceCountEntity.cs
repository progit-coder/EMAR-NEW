using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ControlSubstanceCountEntity
    {
        public int ControlSubstance_Id { get; set; }
        public Nullable<int> Porder_Id { get; set; }
        public string Quantity { get; set; }
        public Nullable<int> CertifiedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> CertifiedDate { get; set; }
        public int ControlSubstance_Status { get; set; }
        public System.DateTime ControlSubstance_CreatedDate { get; set; }

        //public virtual User User { get; set; }
        //public virtual User User1 { get; set; }
    }
}
