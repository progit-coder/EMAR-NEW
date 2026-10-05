using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
  public class PRNDetailsCustomEntity
    {
        public int POrder_Id { get; set; }
        public string PatientName { get; set; }
        public Nullable<System.DateTime> AdminsterOn { get; set; }
        public string AdminsterBy { get; set; }
        public string AdministerComment { get; set; }
        public string MedicationReason_Desc { get; set; }
        public int Patient_Id { get; set; }
        public string PRNComment { get; set; }
        public int PRNCommentBy { get; set; }
        public Nullable<System.DateTime> PRNCommentOn { get; set; }
        public int DrugAdminister_Id { get; set; }
        public string GiveCodeText { get; set;}
    }
}
