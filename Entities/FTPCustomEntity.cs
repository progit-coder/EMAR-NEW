using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class FTPCustomEntity
    {
        public int Company_Id { get; set; }
        public string ServerIp { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
