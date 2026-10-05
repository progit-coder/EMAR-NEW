using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class WeekEntity
    {
        public WeekEntity()
        {
            this.DrugAdministrationTimes = new HashSet<DrugAdministrationTimeEntity>();
        }

        public int Week_Id { get; set; }
        public string Week_Desc { get; set; }
        public int Week_Status { get; set; }
        public Nullable<int> Week_CreatedBy { get; set; }
        public System.DateTime Week_CreatedDate { get; set; }
        
        public virtual ICollection<DrugAdministrationTimeEntity> DrugAdministrationTimes { get; set; }
    }
}
