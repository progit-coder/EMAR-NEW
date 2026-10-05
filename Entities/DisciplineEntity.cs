using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DisciplineEntity
    {
        public int Discipline_Id { get; set; }
        public string Discipline_Code { get; set; }
        public string Discipline_Desc { get; set; }
        public Nullable<int> Discipline_CreatedBy { get; set; }
        public System.DateTime Discipline_CreatedDate { get; set; }
        public Nullable<int> Discipline_Status { get; set; }
    }
}
