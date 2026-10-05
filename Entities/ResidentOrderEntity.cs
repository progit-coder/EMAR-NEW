using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentOrderEntity
    {
        public int ResOrder_Id { get; set; }
        public int Patient_ID { get; set; }
        public string ResOrderType { get; set; }
        public Nullable<System.DateTime> ResOrderDate { get; set; }
        public string ResPhysician { get; set; }
        public string ResOrderText { get; set; }
        public int ResOrder_Status { get; set; }
        public Nullable<int> ResOrder_CreatedBy { get; set; }
        public System.DateTime ResOrder_CreatedDate { get; set; }
        public string PhysicianName { get; set; }

        public virtual DemographicEntity Demographic { get; set; }
    }
}
