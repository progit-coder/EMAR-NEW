using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FteConnectionEntity
    {
        public FteConnectionEntity()
        {
            //this.FTEConfigurations = new HashSet<FTEConfigurationEntity>();
        }

        public int FteConn_Id { get; set; }
        public string FteConn_Desc { get; set; }
        public int FteConn_Status { get; set; }
        public Nullable<int> FteConn_CreatedBy { get; set; }
        public System.DateTime FteConn_CreatedDate { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<FTEConfigurationEntity> FTEConfigurations { get; set; }

    }
}
