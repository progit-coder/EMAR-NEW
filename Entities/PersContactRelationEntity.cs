using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
   public class PersContactRelationEntity
    {
        public int PersContactRelation_Id { get; set; }
        public string PersContactRelation_Desc { get; set; }
        public int PersContactRelation_Status { get; set; }
        public int PersContactRelation_CreatedBy { get; set; }
        public System.DateTime PersContactRelation_CreatedDate { get; set; }
    }
}
