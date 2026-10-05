using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class CompanyConfigEntity
    {
        public int CompanyConfig_Id { get; set; }
        public Nullable<int> Company_Id { get; set; }
        public string Company_Name { get; set; }
        public Nullable<int> ApprovalFlag { get; set; }
        public Nullable<int> Fingersdesc_Id { get; set; }
        public string FingersDesc1 { get; set; }
        public Nullable<int> TimeFormat { get; set; }
        public Nullable<int> Hl7Configured { get; set; }
        public Nullable<int> HLDirectionalWay_Id { get; set; }
        public string HLDirectionalWaysDesc { get; set; }
        public Nullable<int> CompanyConfig_Status { get; set; }
        public Nullable<int> CompanyConfig_CreatedBy { get; set; }
        public Nullable<System.DateTime> CompanyConfig_CreatedOn { get; set; }
        public Nullable<int> StockReport_Id { get; set; }
        public Nullable<int> DrFirstRequired { get; set; }
        // public Nullable<int> Physician_Id { get; set; }
        public string PhysicianFullFName { get; set; }

        public int FteCategory_Id { get; set; }
        public string FteCategory_Desc { get; set; }
        public int EventCat_Id { get; set; }
        public string EventCat_Desc { get; set; }
        public string Category { get; set; }
        public string Events { get; set; }
        public string StockReportFor { get; set; }
        public int Company_Status { get; set; }
    }
    public class NurseStationHierarchyEntity
    {
        public int NSHierarchy_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public Nullable<int> FloorPrior { get; set; }
        public Nullable<int> WingPrior { get; set; }
        public Nullable<int> RoomPrior { get; set; }
        public Nullable<int> BedPrior { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Hierarchy { get; set; }
        public string CompanyName { get; set; }
        public string FacilityName { get; set; }
        public string NursingStationName { get; set; }
        public int CompanyId {get;set ;}
        public int FacilityId { get; set; }

    }
}
