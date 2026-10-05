using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FTEConfigurationEntity
    {
        public int FteConfig_Id { get; set; }
        public int Company_Id { get; set; }
        public string ServerIp { get; set; }
        public int Category { get; set; }
        public Nullable<int> ConnectionType { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int FteConfig_Status { get; set; }
        public Nullable<int> FteConfig_CreatedBy { get; set; }
        public System.DateTime FteConfig_CreatedDate { get; set; }
        public string ConnectionStatus { get; set; }

        //public virtual CompanyEntity Company { get; set; }
        //public virtual FTECategoryEntity FTECategory { get; set; }
        //public virtual FteConnectionEntity FteConnection { get; set; }
        //public virtual UserEntity User { get; set; }

    }
    public class FTConfigurationGridEntity
    {
        public int FteConfig_Id { get; set; }
        public string ServerIp { get; set; }
        public string Category { get; set; }
        public string ConnectionType { get; set; }
        public string Port { get; set; }
        public int FteConfig_Status { get; set; }
        public string ConnectionStatus { get; set; }

        //public virtual CompanyEntity Company { get; set; }
        //public virtual FTECategoryEntity FTECategory { get; set; }
        //public virtual FteConnectionEntity FteConnection { get; set; }
        //public virtual UserEntity User { get; set; }

    }
}
