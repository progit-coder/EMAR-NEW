using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class TreatmentInfoEntity
    {
        public int PTreatment_Id { get; set; }
        public int POrder_Id { get; set; }
        public string ReqGiveCodeIdentifier { get; set; }
        public string RequestedGiveCode { get; set; }
        public string RequestedGiveAmtMin { get; set; }
        public string RequestedGiveAmtMax { get; set; }
        public string RequestedGiveUnits { get; set; }
        public string RequestedDosageForm { get; set; }
        public string ProvidersTreatmentInstructions { get; set; }
        public string ProvidersAdministrationInstructions { get; set; }
        public string DeliverToLocation { get; set; }
        public string AllowSubstitutions { get; set; }
        public string RequestedDispenseCode { get; set; }
        public string RequestedDispenseAmount { get; set; }
        public string RequestedDispenseUnits { get; set; }
        public string NumberOfRefills { get; set; }
        public string OrderingProviderDEANumber { get; set; }
        public string TreatmentSupplierVerifierID { get; set; }
        public string NeedsHumanReview { get; set; }
        public string RequestedGivePer { get; set; }
        public string RequestedGiveStrength { get; set; }
        public string RequestedGiveStrengthUnits { get; set; }
        public string IndicationIdentifier { get; set; }
        public string IndicationText { get; set; }
        public string IndicationCodingSystem { get; set; }
        public string AIndicationIdentifier { get; set; }
        public string AIndicationText { get; set; }
        public string AIndicationCodingSystem { get; set; }
        public string RequestedGiveRateAmount { get; set; }
        public string RequestedGiveRateUnits { get; set; }
        public string TotalDailyDose { get; set; }
        public string SupplementaryCode { get; set; }
        public string RequestedDrugStrengthVol { get; set; }
        public string RequestedDrugStrengthVolUnits { get; set; }
        public string PharmacyOrderType { get; set; }
        public string DispensingInterval { get; set; }
        public int PTreatment_Status { get; set; }
        public Nullable<int> PTreatment_CreatedBy { get; set; }
        public System.DateTime PTreatment_CreatedDate { get; set; }
        public Nullable<int> PTIOutBoundFileStatus { get; set; }
        public Nullable<int> PTIOutBoundApproval { get; set; }
        public Nullable<int> PTIOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PTIOutBoundApprovalOn { get; set; }
        public string UserName { get; set; }

        //public virtual UserEntity User { get; set; }
        //public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
}
