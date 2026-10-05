using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NDCEntity
    {
        public int NDC_Id { get; set; }
        public Nullable<long> NDC_Code { get; set; }
        public string NDC_Desc { get; set; }
    }
}
