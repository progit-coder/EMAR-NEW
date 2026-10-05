using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class OrdersinfoCustomEntity
    {
        public string Allergy { get; set; }
        public string Diet { get; set; }
        public string Diagnosis { get; set; }
    }
    public class OrderRouteEntity
    {
        public int Route_Id { get; set; }
        public string Route { get; set; }
    }
    public class OrdersDataEntity
    {
        public int porder_Id { get; set; }
        public string OrderingPhysicianNPI { get; set; }
        public string DrugName { get; set; }
        public string Quantity { get; set; }
        public string Directions { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Refill { get; set; }
        public Nullable<decimal> Dayssupply { get; set; }
        public Nullable<decimal> MaxPerdays { get; set; }
        public string AlertText { get; set; }
        public string InsulinComments { get; set; }
        public Nullable<bool> OrderStockFlag { get; set; }
        public Nullable<bool> PRNFlag { get; set; }
        public Nullable<bool> TreatmentFlag { get; set; }
        public Nullable<bool> SelfAdministeredFlag { get; set; }
        public int Favouriteflag { get; set; }
        public string Barcode { get; set; }
        public string Schedule { get; set; }
        public int split { get; set; }
        public int PQuantity_Id { get; set; }
        public int Route_Id { get; set; }
        public string Inhand { get; set; }
        public int HoldStatus { get; set; }
        public int POrder_Status { get; set; }
        public int DestroyStatus { get; set; }
        public int ReviewFlag { get; set; }
        public int MergeFlag { get; set; }
        public int ControlSubstanceBit { get; set; }
        public int scheduleTimeflag { get; set; }
        public Nullable<int> ControlSubCreatedBy { get; set; }
        public Nullable<int> Stock_Id { get; set; }
        public Nullable<int> OrderTypeID { get; set; }
        public int MedispanControlSubBit { get; set; }
        public Nullable<int> DiscontinueFlag { get; set; }
        public string OrderOrigin { get; set; }
        public string AGiveCodeIdentifier { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Daw { get; set; }
        public Nullable<decimal> DispenseQty { get; set; }
        public Nullable<System.DateTime> WrittenDate { get; set; }
        public Nullable<int> DiagIndication { get; set; }
        public Nullable<int> WaitforPharmacy { get; set; }
        public Nullable<int> Hospice { get; set; }
        public int CpoeFlag { get; set; }
        public Nullable<int> Source { get; set; }
        public Nullable<int> UOM { get; set; }
        public string DiagIndicationText { get; set; }
        public string NursingFreq_Id { get; set; }
        public string NurseShifts_Id { get; set; }
        public Nullable<int> DUom { get; set; }
        public int Refill_Request { get; set; }
        public string Refill_Note { get; set; }
        public Nullable<int> SchFlag { get; set; }

        public string PharmacyName { get; set; }
        public string AutoBarcode { get; set; }

    }
    public class OrdersCommonStatusEntity
    {
        public int OrderId { get; set; }
        public int DAdminId { get; set; }
        public int QuantityId { get; set; }
        public int PatientId { get; set; }
        public int POrderStatus { get; set; }
        public Nullable<int> POrderCreatedBy { get; set; }
        public System.DateTime POrderCreatedDate { get; set; }
        public Nullable<int> POOutBoundApproval { get; set; }
        public Nullable<int> POOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> POOutBoundApprovalOn { get; set; }
        public string OrderType { get; set; }
        public Nullable<int> DiscontinueFlag { get; set; }
        public string DiscontinueReason { get; set; }
        public Nullable<System.DateTime> DiscontinuedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int DiscontinueAllSplits { get; set; }
        public int Split { get; set; }

    }
    public class OrderUpdateEntity
    {
        public Nullable<int> POrderId { get; set; }
        public Nullable<int> PQuantityId { get; set; }
        public Nullable<int> PhysicianId { get; set; }
        public string DrugName { get; set; }
        public string Quantity { get; set; }
        public string Directions { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string NumberofRefills { get; set; }
        public Nullable<decimal> MaxPerdays { get; set; }
        public string Alerttext { get; set; }
        public string InsulinComments { get; set; }
        public Nullable<bool> OrderstockFlag { get; set; }
        public Nullable<bool> PRNFlag { get; set; }
        public Nullable<bool> TreatmentFlag { get; set; }
        public Nullable<bool> SelfAdministeredFlag { get; set; }
        public int controlSubstance { get; set; }
        public Nullable<int> Route { get; set; }
        public string Inhand { get; set; }
        public Nullable<int> Createdby { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> DAdminId { get; set; }
        public Nullable<int> orderTypeID { get; set; }
        public System.DateTime OrderUpdatedOn { get; set; }
        public int PatientId { get; set; }
        public string RequestedGiveCode { get; set; }
        public string ScheduleText { get; set; }

    }
    public class HOAEntity
    {
        public Nullable<int> dadminId { get; set; }
        public Nullable<int> porderId { get; set; }
        public Nullable<int> pquantityId { get; set; }
        public Nullable<int> freqId { get; set; }
        public Nullable<int> hourId { get; set; }
        public Nullable<int> timeformatId { get; set; }
        public Nullable<int> hours { get; set; }
        public Nullable<bool> monday { get; set; }
        public Nullable<bool> tuesday { get; set; }
        public Nullable<bool> wednesday { get; set; }
        public Nullable<bool> thursday { get; set; }
        public Nullable<bool> friday { get; set; }
        public Nullable<bool> saturday { get; set; }
        public Nullable<bool> sunday { get; set; }
        public string weekId { get; set; }
        public string monthId { get; set; }
        public string days { get; set; }
        public Nullable<int> createdby { get; set; }
        public Nullable<int> activedays { get; set; }
        public Nullable<int> holddays { get;set; }
        public string hourIds { get; set; }
        public int NurseStationId { get; set; }
        public string NurseShiftId { get; set; }
        public Nullable<int> DUom { get; set; }
    }
    public class OrderStockEntity
    {
        public int OrderStockId { get; set; }
        public Nullable<int> StockId { get; set; }
        public Nullable<int> PorderId { get; set; }
        public string Inhand { get; set; }
        public string Remaining { get; set; }
        public string LotNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
    }
    public class DrFirstOrderXMLTransEntity
    {
        public int DrFirstOrderId { get; set; }
        public Nullable<int> DrFirstOrderXMLTransApproval { get; set; }
        public Nullable<int> DrFirstOrderXMLTransApprovalBy { get; set; }
        public Nullable<System.DateTime> DrFirstOrderXMLTransApprovalOn { get; set; }

    }
    public class LiteralOrderEntity
    {
      public string  Directions { get; set; }
      public string StartDate { get; set; }
      public  string PhysicianName { get; set; }
      public string OrderCreatedBy { get; set; }
      public System.DateTime? OrderCreatedDate { get; set; }
    }

}
