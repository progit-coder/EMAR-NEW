using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MaritalStatusEntity
    {
        public MaritalStatusEntity()
        {
            this.Users = new HashSet<UserEntity>();
        }

        public int Marital_id { get; set; }
        public string Marital_Desc { get; set; }
        public string Marital_Code { get; set; }
        public int Marital_Status { get; set; }
        public Nullable<int> Marital_CreatedBy { get; set; }
        public System.DateTime Marital_CreatedDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
