using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class SkinEntity
    {
        public int Skin_Id { get; set; }
        public string Skin_Type { get; set; }
        public string Skin_Desc { get; set; }
        public int Skin_Status { get; set; }
        public int Skin_CreatedBy { get; set; }
        public System.DateTime Skin_CreatedDate { get; set; }
    }
}
