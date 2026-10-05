using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ResponsiblePartyEntity
    {
        public int Resparty_Id { get; set; }
        public string Resparty_Desc { get; set; }
        public int Resparty_Status { get; set; }
        public int Resparty_CreatedBy { get; set; }
        public System.DateTime Resparty_CreatedDate { get; set; }

    }
}
