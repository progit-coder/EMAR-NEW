using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CityEntity
    {
        public int City_Id { get; set; }
        public string City_Name { get; set; }
        public string City_Code { get; set; }
        public int City_StateId { get; set; }
    }
}
