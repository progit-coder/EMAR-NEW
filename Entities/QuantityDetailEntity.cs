using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class QuantityDetailEntity
    {
        public int PQuantity_Id { get; set; }
        public int POrder_Id { get; set; }
        public string Quantity { get; set; }
        public string RepeatPattern { get; set; }
        public string ExplicitTime { get; set; }
        public string RelativeTimeUnits { get; set; }
        public string ServiceDuration { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Priority { get; set; }
        public string ConditionText { get; set; }
        public string TextInstruction { get; set; }
        public string Conjunction { get; set; }
        public string OccuranceDuration { get; set; }
        public string TotalOccurances { get; set; }
        public int PQuantity_Status { get; set; }
        public Nullable<int> PQuantity_CreatedBy { get; set; }
        public System.DateTime PQuantity_CreatedDate { get; set; }
        public Nullable<int> EndDateStatus { get; set; }

    }
}
