using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class GetDocumentCheckDataEntity
    {
        public long DrugAdminister_Id { get; set; }
        public Nullable<int> Porder_Id { get; set; }
        public Nullable<int> PQuantity_Id { get; set; }
        public string Order { get; set; }
        public int Type { get; set; }
        public int ControlSubstanceBit { get; set; }
        public Nullable<int> ControlSubCreatedBy { get; set; }
        public string Pass_Time { get; set; }
        public Nullable<System.DateTime> AdminsterOn { get; set; }
        public Nullable<bool> PRNFlag { get; set; }
        public string Quantity { get; set; }
        public string Route { get; set; }
        public string GPI { get; set; }
        public Nullable<int> DiscardDays { get; set; }
        public Nullable<System.DateTime> DiscardDate { get; set; }
        public Nullable<int> MaxPerdays { get; set; }
        public string AdministerSites { get; set; }
        public string ControlSubstanceCertifiedBy { get; set; }
        public int PRNAdministered { get; set; }
        public Nullable<System.DateTime> AdminsterSchedule { get; set; }


        public string Reason { get; set; }






    }
}
