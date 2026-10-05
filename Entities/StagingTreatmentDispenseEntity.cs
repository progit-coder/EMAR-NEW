using System;
using System.Collections.Generic;

namespace LTCPro.Entities
{
    public class StagingTreatmentDispenseEntity
    {
        public int StagDispense_Id { get; set; }
        public string DispenseCounter { get; set; }
        public string DispenseCode { get; set; }
        public Nullable<System.DateTime> DispensedDate { get; set; }
        public string ActualDispenseAmt { get; set; }
        public string ActualDispenseUnits { get; set; }
        public string ActualDosageForm { get; set; }
        public string PrescriptionNumber { get; set; }
        public string NumberOfRefillsRemaining { get; set; }
        public string DispenseNotes { get; set; }
        public string DispensingProvider { get; set; }
        public string SubstitutionStatus { get; set; }
        public string TotalDailyDose { get; set; }
        public string DispenseToLocation { get; set; }
        public string NeedsHumanReview { get; set; }
        public string SpecialDispensingInstruction { get; set; }
        public string ActualStrength { get; set; }
        public string ActualStrengthUnit { get; set; }
        public string SubstanceLotNumber { get; set; }
        public Nullable<System.DateTime> SubstanceExpirationDate { get; set; }
        public string SubstanceManufacturer { get; set; }
        public string Indication { get; set; }
        public string DispensePackageSize { get; set; }
        public string DispensePackageSizeUnits { get; set; }
        public string DispensePackageMethod { get; set; }
        public string SupplementaryCode { get; set; }
        public Nullable<System.DateTime> OriginalOrderDate { get; set; }
        public string GiveDrugStrengthVol { get; set; }
        public Nullable<int> File_Id { get; set; }

        public virtual FileInformationEntity FileInformation { get; set; }

    }
}
