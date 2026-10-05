using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class AddlInstructionDetailEntity
    {
        public int PAddl_Id { get; set; }
        public int POrder_Id { get; set; }
        public string PackageType { get; set; }
        public string NumberOfLabels { get; set; }
        public string LabelQuantity { get; set; }
        public string Zone { get; set; }
        public string Bin { get; set; }
        public string TotalQuantityWritten { get; set; }
        public string TotalQuantityDispensed { get; set; }
        public string FillQuantityWritten { get; set; }
        public string MaxDailyQuantity { get; set; }
        public string DaysSupply { get; set; }
        public string TimesPerDay { get; set; }
        public Nullable<System.DateTime> DateWritten { get; set; }
        public string PrePack { get; set; }
        public string CycleFill { get; set; }
        public string MARGroupLevel { get; set; }
        public string MARGroup { get; set; }
        public string PartialStatus { get; set; }
        public string IntendedQuantity { get; set; }
        public string IntendedDaysSupply { get; set; }
        public string OriginCode { get; set; }
        public string RxGuid { get; set; }
        public string ToteId { get; set; }
        public string CustomFieldID { get; set; }
        public string CustomFieldValue { get; set; }
        public string CustomFieldName { get; set; }
        public string ACustomFieldID { get; set; }
        public string ACustomFieldValue { get; set; }
        public string ACustomFieldName { get; set; }
        public string RxType { get; set; }
        public string LinkedReorderNumber { get; set; }
        public string ExtraDoseIndicator { get; set; }
        public string LeaveOfAbsenceIndicator { get; set; }
        public string DeliveryID { get; set; }
        public string PartialTabletIndicator { get; set; }
        public string RawAdministrationTimes { get; set; }
        public string ProductType { get; set; }
        public string Treatment { get; set; }
        public Nullable<System.DateTime> PhRxExpireDate { get; set; }
        public string RxNumber { get; set; }
        public int PAddl_Status { get; set; }
        public Nullable<int> PAddl_CreatedBy { get; set; }
        public System.DateTime PAddl_CreatedDate { get; set; }

    }
}
