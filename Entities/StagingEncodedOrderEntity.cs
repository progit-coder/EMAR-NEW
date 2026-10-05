using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{    
    public class StagingEncodedOrder
    {
        public int StagEncOrder_Id { get; set; }
        public Nullable<int> StagRoute_Id { get; set; }
        public string Quantity { get; set; }
        public string GiveCode { get; set; }
        public string GiveAmountMin { get; set; }
        public string GiveAmountMax { get; set; }
        public string GiveUnits { get; set; }
        public string GiveDosageForm { get; set; }
        public string ProviderAdministrationIns { get; set; }
        public string DeliverToLocation { get; set; }
        public string SubstitutionStatus { get; set; }
        public string DispenseAmount { get; set; }
        public string DispenseUnits { get; set; }
        public string NumberOfRefills { get; set; }
        public string OrderingProvidersDEANumber { get; set; }
        public string TreatmentSupplierVerifierID { get; set; }
        public string PrescriptionNumber { get; set; }
        public string NumberOfRefillsRemaining { get; set; }
        public string NumberOfRefillsDispensed { get; set; }
        public Nullable<System.DateTime> RecentRefillDate { get; set; }
        public string TotalDailyDose { get; set; }
        public string NeedsHumanReview { get; set; }
        public string SpecialDispensingInstruction { get; set; }
        public string GivePer { get; set; }
        public string GiveRateAmount { get; set; }
        public string GiveRateUnits { get; set; }
        public string GiveStrength { get; set; }
        public string GiveStrengthUnits { get; set; }
        public string GiveIndication { get; set; }
        public string DispensePackageSize { get; set; }
        public string DispensePackageSizeUnit { get; set; }
        public string DispensePackageMethod { get; set; }
        public string SupplementaryCode { get; set; }
        public Nullable<System.DateTime> OriginalOrderDate { get; set; }
        public string GiveDrugStrengthVolume { get; set; }
        public string GiveDrugStrengthVolUnits { get; set; }
        public string ControlledSubstanceSchedule { get; set; }
        public string FormularyStatus { get; set; }
        public string PharmaceuticalSubstance { get; set; }
        public string PharmacyRecentFill { get; set; }
        public string InitialDispenseAmount { get; set; }
        public string DispensingPharmacy { get; set; }
        public string DispensingPharmacyAddr { get; set; }
        public string DeliverToPatientLocation { get; set; }
        public string DeliverToAddress { get; set; }
        public string PharmacyOrderType { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }
    }
}
