using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingTreatmentEntity
    {
        public int StagTreatment_Id { get; set; }
        public Nullable<int> StagOrder_Id { get; set; }
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
        public string Indication { get; set; }
        public string RequestedGiveRateAmount { get; set; }
        public string RequestedGiveRateUnits { get; set; }
        public string TotalDailyDose { get; set; }
        public string SupplementaryCode { get; set; }
        public string RequestedDrugStrengthVol { get; set; }
        public string RequestedDrugStrengthVolUnits { get; set; }
        public string PharmacyOrderType { get; set; }
        public string DispensingInterval { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }
    }
}
