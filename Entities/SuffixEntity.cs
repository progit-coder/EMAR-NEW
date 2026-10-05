using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
 public class SuffixEntity
    {
        public int Suffix_Id { get; set; }
        public string Suffix_Desc { get; set; }
        public int Suffix_Status { get; set; }
        public Nullable<int> Suffix_CreatedBy { get; set; }
        public System.DateTime Suffix_CreatedDate { get; set; }
    }
}
