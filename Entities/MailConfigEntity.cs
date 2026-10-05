using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MailConfigEntity
    {
        public int ID { get; set; }
        public Nullable<int> Port { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Host { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime Createddate { get; set; }

        //public virtual User User { get; set; }
    }
}
