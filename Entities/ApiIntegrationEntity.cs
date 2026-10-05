using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ApiIntegrationEntity
    {
        public int Integration_Id { get; set; }
        public Nullable<int> Integration_TypeId { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public string ApiPath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Integration_Status { get; set; }
        public Nullable<int> Integration_CreatedBy { get; set; }
        public System.DateTime Integration_CreatedDate { get; set; }
        public string CratedUserName { get; set; }
    }
}
