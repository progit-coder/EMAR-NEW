using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CommonOrderInfo1Entity
    {
        public int AuditPOrder_Id { get; set; }
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
        public string TransactionDate { get; set; }
        public string EnteredBy { get; set; }
        public string EPharmacistLName { get; set; }
        public string EPharmacistFName { get; set; }
        public string VerifiedBy { get; set; }
        public string VPharmacistLName { get; set; }
        public string VPharmacistFName { get; set; }
        public string VEffectivedate { get; set; }
        public string OrderingPhysicianNPI { get; set; }
        public string OPhysicianLname { get; set; }
        public string OPhysicianFname { get; set; }
        public string EntererLocation { get; set; }
        public string CallBackPhoneNumber { get; set; }
        public string OrderEffectiveDate { get; set; }
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
        public string ExpectedAvailabilityDate { get; set; }
        public string ConfidentialityCode { get; set; }
        public string OrderType { get; set; }
        public string EntererAuthorizationMode { get; set; }
        public int POrder_Status { get; set; }
        // public Nullable<int> POrder_UpdatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime POrder_UpdatedOn { get; set; }
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

    }
}
