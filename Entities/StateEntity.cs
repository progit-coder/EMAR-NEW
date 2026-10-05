using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class StateEntity
    {
        public int State_Id { get; set; }
        public string State_Name { get; set; }
        public string State_Code { get; set; }
        public int State_CountryId { get; set; }
    }
}
