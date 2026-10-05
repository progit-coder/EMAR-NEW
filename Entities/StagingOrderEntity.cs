using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingOrderEntity
    {
        public int StagOrder_Id { get; set; }
        public Nullable<int> StagPatVisitId { get; set; }
        public string OrderControl { get; set; }
        public string PlacerOrderNumber { get; set; }
        public string FillerOrderNumber { get; set; }
        public string PlacerGroupNumber { get; set; }
        public string OrderStatus { get; set; }
        public string ResponseFlag { get; set; }
        public string QuantityTiming { get; set; }
        public string Parent { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string EnteredBy { get; set; }
        public string VerifiedBy { get; set; }
        public string OrderingProvider { get; set; }
        public string EntererLocation { get; set; }
        public string CallBackPhoneNumber { get; set; }
        public Nullable<System.DateTime> OrderEffectiveDate { get; set; }
        public string OrderControlCodeReason { get; set; }
        public string EnteringOrganisation { get; set; }
        public string EnteringDevice { get; set; }
        public string AltCodingSystem { get; set; }
        public string AdvBeneficiaryNoticeCode { get; set; }
        public string OrderingFacilityName { get; set; }
        public string OrderingFacilityAddress { get; set; }
        public string OrderingFacilityPhone { get; set; }
        public string OrderingProviderAddress { get; set; }
        public string OrderStatusModifier { get; set; }
        public byte[] AdvBeneficiaryNoticeOverrideReason { get; set; }
        public Nullable<System.DateTime> ExpectedAvailabilityDate { get; set; }
        public string ConfidentialityCode { get; set; }
        public string OrderType { get; set; }
        public string EntererAuthorizationMode { get; set; }
    }
}
