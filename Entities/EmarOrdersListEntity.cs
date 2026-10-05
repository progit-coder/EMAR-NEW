using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class EmarOrdersListEntity
    {
        //public string Date { get; set; }
        //public string Medication { get; set; }
        //public string Quantity { get; set; }
        //public string Barcode { get; set; }
        //public string PorderID { get; set; }    
        //public string OrderInst { get; set; }
        //public Int64 DrugAdminister_Id { get; set; }
        //public string Status { get; set; }
        public int POrder_Id { get; set; }
        //public long DrugAdminister_Id { get; set; }
        public string Drug { get; set; }
        public string Quantity { get; set; }
        public string Route { get; set; }
        public string AdditionalInst { get; set; }
        public string Inhand { get; set; }
        public string NumberOfRefillsRemaining { get; set; }
        public decimal MaxPerday { get; set; }
        public string Diagnosis { get; set; }
        public string Barcode { get; set; }
        public string Last_Modified { get; set; }
        //public Nullable<System.DateTime> Administer_Schedule { get; set; }
        public string Pass_Time { get; set; }
        public string Last_Passed { get; set; }
        public string AdministerStatus { get; set; }
        public string NumberOfRefills { get; set; }
        public System.DateTime Last_ModifiedDate { get; set; }
        public string Last_PassedBy { get; set; }
        public int Refill_Request { get; set; }
        public string AlertText { get; set; }
        public int PsychiatricFlag { get; set; }
        public int AllergyFlag { get; set; }
        public Nullable<bool> PRNFlag { get; set; }
        public Nullable<bool> OrderStockFlag { get; set; }
        public string MedicationReason_Desc { get; set; }
        //public int NoDueFlag { get; set; }
        public int ControlSubstanceFlag { get; set; }
        public string ControlSubstanceCertifiedBy { get; set; }
        public int ReviewFlag { get; set; }
        public Nullable<int> pquantity_Id { get; set; }
        public int dueflag { get; set; }
        public string InputTime { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public Nullable<int> Window { get; set; }
        public int Ekit { get; set; }
        public int Undo { get; set; }
        public int PRNAdministered { get; set; }
        public string GPI { get; set; }
        public Nullable<int> DiscardDays { get; set; }
        public Nullable<System.DateTime> DiscardDate { get; set; }
        public Nullable<int> SideEffectFlag { get; set; }
        public string AdministerSites { get; set; }
        public string ABarcode { get; set; }
        public string Refill_Note { get; set; }
        public string DUOM { get; set; }
    }
    public partial class PrcReportsGetEmarDetails
    {
        public int POrder_Id { get; set; }
        public Nullable<int> pquantity_Id { get; set; }
        public string Drug { get; set; }
        public string Quantity { get; set; }
        public string Route { get; set; }
        public string AdditionalInst { get; set; }
        public string Inhand { get; set; }
        public string NumberOfRefills { get; set; }
        public string NumberOfRefillsRemaining { get; set; }
        public decimal MaxPerday { get; set; }
        public string Diagnosis { get; set; }
        public string Barcode { get; set; }
        public string AlertText { get; set; }
        public Nullable<System.DateTime> Last_ModifiedDate { get; set; }
        public string Last_Modified { get; set; }
        public string Pass_Time { get; set; }
        public Nullable<System.DateTime> Last_Passed { get; set; }
        public string Last_PassedBy { get; set; }
        public string AdministerStatus { get; set; }
        public int Refill_Request { get; set; }
        public int PsychiatricFlag { get; set; }
        public int AllergyFlag { get; set; }
        public Nullable<bool> PRNFlag { get; set; }
        public Nullable<bool> OrderStockFlag { get; set; }
        public string MedicationReason_Desc { get; set; }
        public int ControlSubstanceFlag { get; set; }
        public int PRNAdministered { get; set; }
        public string ControlSubstanceCertifiedBy { get; set; }
        public int ReviewFlag { get; set; }
        public int dueflag { get; set; }
        public string InputTime { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public Nullable<int> Window { get; set; }
        public int Ekit { get; set; }
        public int Undo { get; set; }
        public string GPI { get; set; }
        public Nullable<int> DiscardDays { get; set; }
        public Nullable<System.DateTime> DiscardDate { get; set; }
        public Nullable<int> SideEffectFlag { get; set; }
        public string AdministerSites { get; set; }
        public string ABarcode { get; set; }
        public string Refill_Note { get; set; }
        public string DUOM { get; set; }
    }

    public partial class VitalsCheckEntity
    {
        public int OrderFavMaster_ID { get; set; }
        public string OrderFavDesc { get; set; }
        public int PQuantity_Id { get; set; }
    }
    public partial class EkitDropEntity
    {
        public int Ekit_Id { get; set; }
        public string DrugName { get; set; }
        public string InHand { get; set; }
    }
    public partial class DosesDetailsEntity
    {
        public long DrugAdminister_Id { get; set; }
        public string DrugName { get; set; }
        public string ProviderAdminDrugInsText { get; set; }
        public string AdminsterSchedule { get; set; }
        public string PassTime { get; set; }
        public string PatientName { get; set; }
    }
    public partial class RefillDataEntity
    {
        public string Refill_Note { get; set; }
    }
}
