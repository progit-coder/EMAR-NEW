using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class ControlSubstanceGridEntity
    {
        public int PorderId { get; set; }
        public string ResidentName { get; set; }
        public string DOB { get; set; }
        public string Order { get; set; }
        public string LastCertified { get; set; }
        public string CertifiedDate { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public string NurseStationName { get; set; }
        public int Porder_Status { get; set; }
        public int QuantityZeroStatus { get; set; }
        public Nullable<decimal> InitialQuantity { get; set; }
        public int Patient_Id { get; set; }
        public int QuantityId { get; set; }
        public string GPI { get; set; }
        public int ConsolidatedAllow { get; set; }
        public string Directions { get; set; }
        public string QtyPerDose { get; set; }
        public Nullable<int> ConsolidateFlag { get; set; }
        public Nullable<decimal> AdministeredQty { get; set; }
        public Nullable<int> CheckInFlag { get; set; }
        public int NsChangeFlag { get; set; }
        public int eKitFlag { get; set; }
        public int Ekit_Id { get; set; }
        public int DFalg { get; set; }
    }
    public class ControlSubstanceTransEntity
    {

        public string Order { get; set; }
        public string LastCertified { get; set; }
        public Nullable<System.DateTime> CertifiedDate { get; set; }
        public string InitialQuantity { get; set; }
        public string Reason { get; set; }
    }
    public class ConsolidateCustomEntity
    {
        public string Quantity { get; set; }
        public List<ControlSubstanceSave> ConsolidateOrders;
        public int User_Id { get; set; }
        public DateTime ConsolidatedOn { get; set; }
        public string Cert_UserName { get; set; }
        public string Cert_Password { get; set; }
        public string Approval_UserName { get; set; }
        public string Approval_Password { get; set; }
        public int CertifiedUserId { get; set; }
        public int ApprovedUserId { get; set; }
        public int Facility_Id { get; set; }
        public string Reason { get; set; }
    }
    public class TimezoneEntity
    {
        public string LastPassedDate { get; set; }
        public Nullable<int> FacilityId { get; set; }

    }
}
