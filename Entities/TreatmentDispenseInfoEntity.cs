using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class TreatmentDispenseInfoEntity
    {
        public int PDispense_Id { get; set; }
        public int Patient_Id { get; set; }
        public string DispenseCounter { get; set; }
        public string DispenseCodeID { get; set; }
        public string DispenseCodeText { get; set; }
        public string ADispenseCodeID { get; set; }
        public string DispensedDate { get; set; }
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
        public string SubstanceExpirationDate { get; set; }
        public string SubstanceManufacturer { get; set; }
        public string Indication { get; set; }
        public string DispensePackageSize { get; set; }
        public string DispensePackageSizeUnits { get; set; }
        public string DispensePackageMethod { get; set; }
        public string SupplementaryCode { get; set; }
        public string InitiatingLocation { get; set; }
        public string AssemblyLocation { get; set; }
        public string ActualDrugStrengthVolume { get; set; }
        public string ActualDrugStrengthVolUnits { get; set; }
        public string DispenseToPharmacy { get; set; }
        public string DispenseToPharmacyAddress { get; set; }
        public string PharmaceuticalSubstance { get; set; }
        public string PharmacyOrderType { get; set; }
        public string DispenceType { get; set; }
        public int PDispense_Status { get; set; }
        public Nullable<int> PDispense_CreatedBy { get; set; }
        public System.DateTime PDispense_CreatedDate { get; set; }

        public virtual DemographicEntity Demographic { get; set; }
    }
}
