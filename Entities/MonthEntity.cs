using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MonthEntity
    {        
        public MonthEntity()
        {
            this.DrugAdministrationTimes = new HashSet<DrugAdministrationTimeEntity>();
        }

        public int Month_Id { get; set; }
        public string Month_Name { get; set; }
        public int Month_Status { get; set; }
        public Nullable<int> Month_CreatedBy { get; set; }
        public System.DateTime Month_CreatedDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<DrugAdministrationTimeEntity> DrugAdministrationTimes { get; set; }
    }
}
