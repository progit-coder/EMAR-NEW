using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class DashBoardEntity
    {
        public IList YaxisData { get; set; }
        public IList XaxisData { get; set; }
        public List<DashboardColumnsEntity> ColumnNames { get; set; }
        public IList GridData { get; set; }
        public int TotalRecordsCount { get; set; }
    }
    public class UserActvivtysEntity
    {
        public string dashboardName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int userId { get; set; }
        public string nursingstationId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public string passTime { get; set; }
        public int? orderType { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public int patientId { get; set; }
        public string patientName { get; set; }
        public string commentType { get; set; }
        public string userIds { get; set; }
        public string shiftTime { get; set; }
        public int facilityid { get; set; }
    }
    public class UserActvivtysReportEntity
    {
        public string userIds { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string userId { get; set; }
        public string dateTime { get; set; }


    }
    public class UserActvivtysExcelEntity
    {
        public string userIds { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string userId { get; set; }
       


    }

    public class MedRefEntity
    {
        public string dashboardName { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public int userId { get; set; }
        public string nursingstationId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public string passTime { get; set; }
        public string dateTime { get; set; }
        public int facilityId { get; set; }

    }


    public class InboundDashBoardData
    {
        public int TotalRecords { get; set; }
        public int ErrorRecords { get; set; }
        public int SuccessRecords { get; set; }
        public string CreatedDate { get; set; }
    }
    public class InboundDashBoard
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        //public List<int> ErrorCount { get; set; }
        //public List<string> CreatedDate { get; set; }
    }
    public class InboundDashBoardDisplay
    {
        public List<InboundDashBoard> yaxisdata { get; set; }
        public List<string> xaxisdata { get; set; }
    }

    public class InboundDashBoardCompanyCountDate
    {
        public string name { get; set; }
        public int data { get; set; }
        //public List<int> ErrorCount { get; set; }
        public string CreatedDate { get; set; }
    }
    public class SeriesDataEntity
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public string nsname { get; set; }
    }
    public class PieSeriesDataEntity
    {
        public string name { get; set; }
        public int y { get; set; }
        public string nsname { get; set; }
    }
    public class DashboardColumnsEntity
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public bool display { get; set; }
        public string alignType { get; set; }
        public string columnType { get; set; }
    }
    public class EMARResidentEntity
    {
        public List<DashboardColumnsEntity> ColumnNames { get; set; }
        public DataTable GridData { get; set; }
        public int TotalRecordsCount { get; set; }
        public string Legend { get; set; }
    }
    public class EmarHoleHistoryEntity
    {
        public string DashboardName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int UserId { get; set; }
        public string NursingstationId { get; set; }
        public int currentPage { get; set; }
        public string PassTime { get; set; }
        public int pageSize { get; set; }
        public Nullable<int> OrderType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int PatientId { get; set; }
        public string patientName { get; set; }
        public string commentType { get; set; }
        public string userIds { get; set; }
        public string shiftTime { get; set; }
        public int facilityId { get; set; }
    }
    public class EmarReportEntity
    {
        public int UserId { get; set; }
        public string NursingstationId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string patientName { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string DateTime { get; set; }
        
    }
    public class EmarthReportEntity
    {
        public int UserId { get; set; }
        public string NursingstationId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string patientName { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public string DateTime { get; set; }
        public string druglassification { get; set; }
        public string shiftTime { get; set; }

    }
    public class ScheduleOrderEntity
    {
        public string NusingStationId { get; set; }
        public int FacilityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PatientIds { get; set; }
        public Nullable<int> OrderType { get; set; }
        public int UserId { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public string DateTime { get; set; }
    }
    public class EkitQtyOnHandUpdate
    {
        public List<DashboardColumnsEntity> columnNames;
        public string NurseStation_Name { get; set; }
        public string Drugname { get; set; }
        public string BarcodeDetail { get; set; }
        public string LotNumber { get; set; }
        public string reason { get; set; }
        public string ExpiryDate { get; set; }
        public string qtybeforeupdate { get; set; }
        public string qtyafterupdate { get; set; }
        public string updatedby { get; set; }
        public string updateddate { get; set; }
        public string witness { get; set; }
    }
    public class MedicationCheckinDr
    {
        public List<DashboardColumnsEntity> columnNames;
        public string NurseStationName { get; set; }
        public string DrugName { get; set; }
        public string Barcode { get; set; }
        public string Lot { get; set; }
        public string ExpirationDate { get; set; }
        public string QtyReceived { get; set; }
        public string ReceivedBy { get; set; }
        public string ReceivedDate { get; set; }
    }
    public class ekitdispensingentity
    {
        public string NurseStationName { get; set; }
        public string ResidentName { get; set; }
        public string DOB { get; set; }
        public string DrugName { get; set; }
        public string QtyOnHandRemaining { get; set; }
        public Nullable<decimal> QuantityAdministered { get; set; }
        //public string QuantityAdministered { get; set; }
        public string EkitAdministeredBy { get; set; }
        public string AdministeredDateTime { get; set; }
        public string Physician { get; set; }
        public string Expirydate { get; set; }
        public string LotNumber { get; set; }
    }
    public class EKitExpiration
    {
        public string NurseStation_Name { get; set; }
        public string Drugname { get; set; }
        public string BarcodeDetail { get; set; }
        public string LotNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Quantity_on_Hand { get; set; }
        public string Expired { get; set; }
    }
}
