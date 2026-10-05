using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CommonOrderInfoEntity
    {
        public int POrder_Id { get; set; }
        public int Patient_Id { get; set; }
        public Nullable<int> OrderingPhysicianID { get; set; }
        public string OrderControl { get; set; }
        public string PlacerOrderNumber { get; set; }
        public string FacilityId { get; set; }
        public string PatientId { get; set; }
        public string Room { get; set; }
        public Nullable<int> OrderTypeID { get; set; }
        public string PlacerGroupNumber { get; set; }
        public string OrderStatus { get; set; }
        public string ResponseFlag { get; set; }
        public string QuantityTiming { get; set; }
        public string Parent { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string EnteredBy { get; set; }
        public string EPharmacistLName { get; set; }
        public string EPharmacistFName { get; set; }
        public string VerifiedBy { get; set; }
        public string VPharmacistLName { get; set; }
        public string VPharmacistFName { get; set; }
        public Nullable<System.DateTime> VEffectivedate { get; set; }
        public string OrderingPhysicianNPI { get; set; }
        public string OPhysicianLname { get; set; }
        public string OPhysicianFname { get; set; }
        public string EntererLocation { get; set; }
        public string CallBackPhoneNumber { get; set; }
        public Nullable<System.DateTime> OrderEffectiveDate { get; set; }
        public string OrderControlCodeReason { get; set; }
        public string EnteringOrganisation { get; set; }
        public string EnteringDevice { get; set; }
        public string AltCodingSystem { get; set; }
        public string AdvBeneficiaryNoticeCode { get; set; }
        public string OrderingFacilityName { get; set; }
        public string OrderingFacilityAddress1 { get; set; }
        public string OrderingFacilityAddress2 { get; set; }
        public string OrderingFacilityCity { get; set; }
        public string OrderingFacilityState { get; set; }
        public string OrderingFacilityZip { get; set; }
        public string OrderingFacilityPhone { get; set; }
        public string OrderingPhysicianAddress1 { get; set; }
        public string OrderingPhysicianAddress2 { get; set; }
        public string OrderingPhysicianCity { get; set; }
        public string OrderingPhysicianState { get; set; }
        public string OrderingProviderZip { get; set; }
        public string OrderStatusModifier { get; set; }
        public string AdvBeneficiaryNoticeOverrideReason { get; set; }
        public Nullable<System.DateTime> ExpectedAvailabilityDate { get; set; }
        public string ConfidentialityCode { get; set; }
        public string OrderType { get; set; }
        public string EntererAuthorizationMode { get; set; }
        public int POrder_Status { get; set; }
        public Nullable<int> POrder_CreatedBy { get; set; }
        public System.DateTime POrder_CreatedDate { get; set; }
        public Nullable<int> POOutBoundFileStatus { get; set; }
        public Nullable<int> POOutBoundApproval { get; set; }
        public Nullable<int> POOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> POOutBoundApprovalOn { get; set; }
        public string AlertText { get; set; }
        public Nullable<int> MaxPerdays { get; set; }
        public Nullable<bool> OrderStockFlag { get; set; }
        public Nullable<bool> PRNFlag { get; set; }
        public Nullable<bool> SelfAdministeredFlag { get; set; }
        public Nullable<bool> TreatmentFlag { get; set; }
        public Nullable<bool> Maysubstitute { get; set; }
        public string InsulinComments { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Daw { get; set; }
        public Nullable<int> DispenseQty { get; set; }
        public Nullable<System.DateTime> WrittenDate { get; set; }
        public Nullable<int> DiagIndication { get; set; }
        public Nullable<int> WaitforPharmacy { get; set; }
        public Nullable<int> Hospice { get; set; }
        public Nullable<int> Source { get; set; }
        public Nullable<int> UOM { get; set; }
        public string DiagIndicationText { get; set; }
    }
    public class OutboundErrorDetailsEntity
    {
        public int File_Id { get; set; }
        public string ResidentName { get; set; }
        public string DOB { get; set; }
        public string DrugName { get; set; }
        public string Event { get; set; }
        public System.DateTime File_CreatedDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class OrdersListFilter
    {
        public int residentId { get; set; }
        public string shiftId { get; set; }
        public string dateValue { get; set; }
        public string[] time { get; set; }
    }
}
