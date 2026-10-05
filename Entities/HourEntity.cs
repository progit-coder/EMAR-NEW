using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class HourEntity
    {        
        public HourEntity()
        {
            this.DrugAdministrationTimes = new HashSet<DrugAdministrationTimeEntity>();
        }

        public int Hour_Id { get; set; }
        public string Hour_Desc { get; set; }
        public int Hour_Status { get; set; }
        public Nullable<int> Hour_CreatedBy { get; set; }
        public System.DateTime Hour_Createddate { get; set; }
        public string RegularTime { get; set; }
        public string RegularTimeFormat { get; set; }
        [JsonIgnore]
        public virtual ICollection<DrugAdministrationTimeEntity> DrugAdministrationTimes { get; set; }
    }
    public class CustomPassShiftTimeEntity
    {
        public string Hour_Id { get; set; }
        public string Hour_Desc { get; set; }
    }
}
