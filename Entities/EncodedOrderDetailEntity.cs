using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class EncodedOrderDetailEntity
    {
        public int PEncOrder_Id { get; set; }
        public int POrder_Id { get; set; }
        public string Quantity { get; set; }
        public string GiveCodeIdentifier { get; set; }
        public string GiveCodeText { get; set; }
        public string AGiveCodeIdentifier { get; set; }
        public string GiveAmountMin { get; set; }
        public string GiveAmountMax { get; set; }
        public string GiveUnits { get; set; }
        public string GiveDosageForm { get; set; }
        public string ProviderAdminDrugIdentifier { get; set; }
        public string ProviderAdminDrugInsText { get; set; }
        public string DeliverToLocation { get; set; }
        public string SubstitutionStatus { get; set; }
        public string DispenseAmount { get; set; }
        public string DispenseUnits { get; set; }
        public string NumberOfRefills { get; set; }
        public string PhysicianDEANumber { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianFirstName { get; set; }
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
        public string DispensingPharmacyID { get; set; }
        public string DispensingPharmacyName { get; set; }
        public string DispensingPharmacyAddr1 { get; set; }
        public string DispensingPharmacyAddr2 { get; set; }
        public string DispensingPharmacyCity { get; set; }
        public string DispensingPharmacyState { get; set; }
        public string DispensingPharmacyZip { get; set; }
        public string DeliverToPatientLocation { get; set; }
        public string DeliverToAddress { get; set; }
        public string PharmacyOrderType { get; set; }
        public int PEncOrder_Status { get; set; }
        public Nullable<int> PEncOrder_CreatedBy { get; set; }
        public System.DateTime PEncOrder_CreatedDate { get; set; }
        public Nullable<int> PEOutBoundFileStatus { get; set; }
        public Nullable<int> PEOutBoundApproval { get; set; }
        public Nullable<int> PEOutBoundApprovalBy { get; set; }
        public Nullable<System.DateTime> PEOutBoundApprovalOn { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual UserEntity User1 { get; set; }
        public virtual CommonOrderInfoEntity CommonOrderInfo { get; set; }
    }
    public class NurseCommentsEntity
    {
        public long Comments_Id { get; set; }
        public Nullable<long> DrugAdminister_Id { get; set; }
        public Nullable<int> NurseCommentType_Id { get; set; }
        public string Comment { get; set; }
        public Nullable<int> comment_Status { get; set; }
        public Nullable<int> comment_CreatedBy { get; set; }
        public Nullable<System.DateTime> Comment_CreatedOn { get; set; }
        public Nullable<int> PQuantity_Id { get; set; }
        public string UserName { get; set; }
        public Nullable<int> Patient_Id { get; set; }

        public virtual User User { get; set; }
        public virtual DrugAdminister DrugAdminister { get; set; }
    }
    public class VitalsEntity
    {
        public string AdminsIds { get; set; }
        public List<checkedList>  checkedVitals { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
public class checkedList
{
    public int IdValue { get; set; }
    public string value { get; set; }
}
}
