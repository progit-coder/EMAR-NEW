using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CertifyAndApprovalCheckEntity
    {
        public string Cert_UserName { get; set; }
        public string Cert_Password { get; set; }
        public string Approval_UserName { get; set; }
        public string Approval_Password { get; set; }
        public List<ControlSubstanceSave> ControlOrders;
        public int User_Id { get; set; }
        public int Facility_Id { get; set; }
        public System.DateTime ApprovedOn { get; set; }

    }
    public class ControlSubstanceSave
    {
        public int POrderId { get; set; }
        public int PQuantity_Id { get; set; }
        public string Quantity { get; set; }
        public string DiscrepancyReason { get; set; }
        public int eKitFlag { get; set; }
        public int Ekit_Id { get; set; }
    }
}
