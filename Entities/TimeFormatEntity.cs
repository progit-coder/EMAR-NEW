using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class TimeFormatEntity
    {        
        public TimeFormatEntity()
        {
            this.DrugAdministrationTimes = new HashSet<DrugAdministrationTimeEntity>();
        }

        public int TimeFormat_Id { get; set; }
        public string TimeFormat_Desc { get; set; }
        public int TimeFormat_Status { get; set; }
        public Nullable<int> TimeFormat_CreatedBy { get; set; }
        public System.DateTime TimeFormat_CreatedDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<DrugAdministrationTimeEntity> DrugAdministrationTimes { get; set; }
    }
}
