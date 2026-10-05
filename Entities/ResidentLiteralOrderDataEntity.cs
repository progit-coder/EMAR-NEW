using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResidentLiteralOrderDataEntity
    {
        public string FillerType { get; set; }
        public string OrderType { get; set; }
        public string DrugName { get; set; }
        public string DosageForm { get; set; }
        public string Quantity { get; set; }
        public string PhysicianName { get; set; }
        public string Directions { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public System.DateTime POrder_CreatedDate { get; set; }
        public int POrder_Id { get; set; }
        public Nullable<int> PQuantity_Id { get; set; }
        public Nullable<int> OrderTypeID { get; set; }
    }
}
