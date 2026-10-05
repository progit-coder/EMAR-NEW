using LTCPro.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class StockEntity
    {
        public int Stock_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string NurseStationName { get; set; }
        public string DrugName { get; set; }
        public string InHand { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> Stock_Status { get; set; }
        public Nullable<int> Stock_CreatedBy { get; set; }
        public Nullable<System.DateTime> Stock_CreatedDate { get; set; }
        public string GPICode { get; set; }
        public string UserName { get; set; }
        public string FacilityName { get; set; }
        public int? Facility_Id { get; set; }
        public int FacilityStatus { get; set; }
        public int NurseStatioStatus { get; set; }
        public string Trackable { get; set; }
        public int TrackableBit { get; set; }
        public string Shared { get; set; }
        public int SharedStockBit { get; set; }
        public int ControlledSubstanceSchedule { get; set; }
        public string MergedDrugNames { get; set; }
        public string MergedStockIds { get; set; }
    }
    public partial class StockCustomEntity
    {
        public int Stock_Id { get; set; }
        public Nullable<int> Facility_Id { get; set; }
        public Nullable<int> NurseStation_Id { get; set; }
        public string DrugName { get; set; }
        public string InHand { get; set; }
        public string[] Barcode { get; set; }
        public Nullable<int> Stock_Status { get; set; }
        public Nullable<int> Stock_CreatedBy { get; set; }
        public Nullable<System.DateTime> Stock_CreatedDate { get; set; }
        public string GPICode { get; set; }
        public Nullable<int> TrackableBit { get; set; }
        public Nullable<int> Sharedstockbit { get; set; }
        public string MergedStockIds { get; set; }
    }
    public partial class StockSearchEntity
    {
        public string DrugName { get; set; }
        public int NurseStationId { get; set; }
    }
    public partial class StockEkitSearchCustomEntity
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
    }
    public partial class StockGridEntity
    {
        public List<StockEntity> GridData { get; set; }
        public int TotalRecords { get; set; }
    }
    public class StockEkitCloneEntity
    {
        public int Input { get; set; }
        public string clone { get; set; }
        public int? sharedstockbit { get; set; }
        public int? stockekit { get; set; }
        public int? createdBy { get; set; }
    }
    public class stockGpiInfo
    {
        public string DrugName { get; set; }
        public int StockId { get; set; }
    }
    public class StockUpdateEntity
    {
        public int StockId { get; set; }
        public string InHand { get; set; }
        public string[] Barcode { get; set; }
        public int StockCreatedBy { get; set; }
        public DateTime StockCreatedDate { get; set; }
    }
}
