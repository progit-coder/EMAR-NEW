using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FTECategoryEntity
    {
        public FTECategoryEntity()
        {
            //this.FTEConfigurations = new HashSet<FTEConfigurationEntity>();
        }

        public int FteCategory_Id { get; set; }
        public string FteCategory_Desc { get; set; }
        public int FteCategory_Status { get; set; }
        public Nullable<int> FteCategory_CreatedBy { get; set; }
        public System.DateTime FteCategory_CreatedDate { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<FTEConfigurationEntity> FTEConfigurations { get; set; }

    }
}
