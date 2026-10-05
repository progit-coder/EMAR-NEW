using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DrugAdministerEntity
    {
        //public int POrder_Id { get; set; }
        //public int pquantity_Id { get; set; }
        //public long DrugAdminister_Id { get; set; }
        //public Nullable<System.DateTime> AdminsterSchedule { get; set; }
        //public string AdministerComment { get; set; }
        //public Nullable<int> AdminsterStatus { get; set; }
        //public Nullable<int> AdminsterBy { get; set; }
        //public Nullable<System.DateTime> AdminsterOn { get; set; }
        //public string AdminsterOnDate { get; set; }
        //public string AdminsterOnTime { get; set; }
        //public Nullable<int> MedicationReason_ID { get; set; }
        //public Nullable<int> BCScanner { get; set; }
        //public string BCScannerText { get; set; }
        //public Nullable<int> Patient_Id { get; set; }
        //public Nullable<int> Ekit_Id { get; set; }
        //public Nullable<decimal> quantity { get; set; }
        //public Nullable<decimal> DrugQuantity { get; set; }
        //public string ByPassReason { get; set; }
        //public string InputTime { get; set; }
        //public Nullable<int> ShiftId { get; set; }
        //public Nullable<int> Window { get; set; }
        //public Nullable<int> ManualDocumentedBy { get; set; }
        //public Nullable<System.DateTime> ManualDocumentedDate { get; set; }
        //public bool PRNFlag { get; set; }
        //public bool UndoFlag { get; set; }
        //public string Last_Passed { get; set; }
        //public string AdditionalComments { get; set; }
        //public string AdministeredBarcode { get; set; }
        //public Nullable<System.DateTime> DiscardDate { get; set; }
        //public string AdministerInsulinSites { get; set; }
        //public string RouteCode { get; set; }
        //public string Reason { get; set; }
        public int is_deleted { get; set; }
        public int POrder_Id { get; set; }
        public int pquantity_Id { get; set; }
        public long DrugAdminister_Id { get; set; }
        public Nullable<System.DateTime> AdminsterSchedule { get; set; }
        public string AdministerComment { get; set; }
        public Nullable<int> AdminsterStatus { get; set; }
        public Nullable<int> AdminsterBy { get; set; }
        public Nullable<System.DateTime> AdminsterOn { get; set; }
        public string AdminsterOnDate { get; set; }
        public string AdminsterOnTime { get; set; }
        public Nullable<int> MedicationReason_ID { get; set; }
        public Nullable<int> BCScanner { get; set; }
        public string BCScannerText { get; set; }
        public Nullable<int> Patient_Id { get; set; }
        public Nullable<int> Ekit_Id { get; set; }
        public Nullable<decimal> quantity { get; set; }
        public Nullable<decimal> DrugQuantity { get; set; }
        public string ByPassReason { get; set; }
        public string InputTime { get; set; }
        public Nullable<int> ShiftId { get; set; }
        public Nullable<int> Window { get; set; }
        public Nullable<int> ManualDocumentedBy { get; set; }
        public Nullable<System.DateTime> ManualDocumentedDate { get; set; }
        public bool PRNFlag { get; set; }
        public bool UndoFlag { get; set; }
        public string Last_Passed { get; set; }
        public string AdditionalComments { get; set; }
        public string AdministeredBarcode { get; set; }
        public Nullable<System.DateTime> DiscardDate { get; set; }
        public string AdministerInsulinSites { get; set; }
        public string RouteCode { get; set; }
        public string Reason { get; set; }
        public List<Lot> Ekit_AllIds { get; set; }

    }
    public class PrcGetDocAdminOrderAudit_ResultEntity
    {
        public string Nurse_Station_Name { get; set; }
        public string Resident_Name { get; set; }
        public string DOB { get; set; }
        public string Drug_Name { get; set; }
        public string Change_Description { get; set; }
        public string Administered_By { get; set; }
        public string Entered_By { get; set; }
        public Nullable<System.DateTime> Entered_Date { get; set; }
        public string Reason { get; set; }
    }
    public class Lot
    {
        public string LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? Ekit_Id { get; set; }
        public decimal InHandQty { get; set; }
    }

}
