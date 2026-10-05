using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class SectionEntity
    {
        public int Section_Id { get; set; }
        public Nullable<int> Section_Type { get; set; }
        public string Section_Desc { get; set; }
    }
}
