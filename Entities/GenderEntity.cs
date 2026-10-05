using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class GenderEntity
    {
        public GenderEntity()
        {
            this.Users = new HashSet<UserEntity>();
        }

        public int Gender_Id { get; set; }
        public string Gender_Desc { get; set; }
        public string Gender_Code { get; set; }
        public int Gender_Status { get; set; }
        public Nullable<int> Gender_CreatedBy { get; set; }
        public System.DateTime Gender_CreatedDate { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
