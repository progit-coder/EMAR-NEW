using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class NotesInfoEntity
    {
        public int PNote_id { get; set; }
        public int Patient_Id { get; set; }
        public string SourceOfComment { get; set; }
        public string Comment { get; set; }
        public string CommentType { get; set; }
        public int PNote_Status { get; set; }
        public Nullable<int> PNote_CreatedBy { get; set; }
        public System.DateTime PNote_CreatedDate { get; set; }

        public virtual DemographicEntity Demographic { get; set; }
    }
}
