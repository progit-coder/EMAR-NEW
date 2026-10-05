using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DrFirstOrderCheck_ResultEntity
    {
        public string Drug { get; set; }
        public string AdditionalInst { get; set; }
        public string NumberofRefill { get; set; }
        public string PatientMRNumber { get; set; }
        public string OtherNotes { get; set; }
        public string PatientNotes { get; set; }
        public string Physician_Ordered { get; set; }
        public string DrFirstOrder_CreatedDate { get; set; }
        public Nullable<int> DrFirstOrderXMLTrans_Approval { get; set; }
        public int DrFirstOrder_Id { get; set; }
        public string Quantity { get; set; }
        public string No__of_Days { get; set; }
        public string Comments { get; set; }
        public int status { get; set; }
        public string Cancel_Date { get; set; }

    }
    public partial class PrcDrFirstOrderHL7_ResultEntity
    {
        public int Patient_Id { get; set; }
        public string Drug { get; set; }
        public string RouteText { get; set; }
        public string AdditionalInst { get; set; }
        public string NumberofRefill { get; set; }
        public int Porder_Id { get; set; }
        public Nullable<int> DAdmin_Id { get; set; }
        public Nullable<int> POOutBoundApproval { get; set; }
    }

}
