using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class EventCategoryEntity
    {
        public int EventCat_Id { get; set; }
        public Nullable<int> Category_Id { get; set; }
        public string EventCat_Code { get; set; }
        public string EventCat_Desc { get; set; }
        public string EventCat_ShortCode { get; set; }
        public Nullable<int> EventCat_Type { get; set; }
        public int EventCat_Status { get; set; }
        public Nullable<int> EventCat_CreatedBy { get; set; }
        public System.DateTime EventCat_CreatedDate { get; set; }
    }
}
