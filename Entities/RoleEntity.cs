using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class RoleEntity
    {
        public int Role_Id { get; set; }
        public string Role_Desc { get; set; }
        public int Role_Status { get; set; }
        public Nullable<int> Role_CreatedBy { get; set; }
        public System.DateTime Role_CreatedDate { get; set; }
        public Nullable<int> DefaultScreen_Id { get; set; }
        public string Screen_Desc { get; set; }
        public string UserName { get; set; }
        public virtual UserEntity User { get; set; }
        public Nullable<int> IsAdmin { get; set; }
        public Nullable<int> Parent_Id { get; set; }
        public string SuperiorRoleName { get; set; }
        //   [JsonIgnore]
        //  public virtual ICollection<RoleConfigEntity> RoleConfigs { get; set; }
    }
    public class RoleDropdownEntity
    {
        public int Role_Id { get; set; }
        public string Role_Desc { get; set; }
    }
    public class RoleGridData
    {
        public int Role_id { get; set; }
        public string Role_Description { get; set; }
        public string Superior_Role { get; set; }
        public int Role_Status { get; set; }
        public string IsAdmin { get; set; }
        public string Screen_Desc { get; set; }
    }
}
