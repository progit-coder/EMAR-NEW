using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;
using System.Data;


namespace LTCPro.Repositories
{
    public interface IReportsRepository
    {
       // List<NurseCommentTypeDropEntity> GetNurseComments();
        IList GetReports();
        IList GetCompanyReports();
        IList GetFacilityReports(int companyId);
        IList GetNursestationReports(int companyId, int facilityId);
        IList GetCensusReports(int userId, string fromDate, string toDate);
        IList GetFloorReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId);
        IList GetWingReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId);
        IList GetRoomReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId);
        IList GetBedReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId);
        IList GetEmarlogo(int Id = 0);
        IList GetHeaderlogo(Nullable<int> companyId, Nullable<int> facilityId);
        DataSet GetCensusReportByfilter(string fromdate, string todate, int userId, string nursestationId, int type, int reportType);
        DataSet GetCensusReportByDates(string fromdate, string todate, int userId, string nursestationname, int type, int reportType, string value);
        IList GetAllAllergiesByClassReport();
        IList GetAllAllergiesByDrugReport();
        IList GetAllICD10Report();
        DataSet Get72HoursReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetPRNDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetOrderDetailsReport(string patientIds, string fromdate, string todate,int ordertype,int userId,string nursestationId);
        DataSet GetWithoutBarcodeDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetBiometricsDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetOrderControlSignoffReport(string fromdate, string todate, string nursestationId, int userId);
        DataSet GetOrderControlSubstanceReport(string nursestationId, int userId);
        DataSet GetOrderHoldDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetOrderWithFavouritesReport(string passtime, string fromdate, string todate, int userId, string nursestationId,int shiftTime);
        DataSet GetCompareCensusDataReport(string year, int userId, string nursestationId, int type, int reporttype);
        DataSet GetRefusedByResidentDetailsReport(string fromdate, string todate, int userId, string nursestationId,string gpi,string datetime);
        DataSet GetMedrefReport(string fromdate, string todate, int userId, string nursestationId, string gpi);
        DataSet GetOrderChangeDetailsReport(string fromdate, string todate, int userId, string nursestationId,string residentId);
        DataSet GetWithoutScanningDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetDestructionDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetFloorStockDetailsReport(int userId, string nursestationId,int facilityId);
        DataSet GetPharmacyMedsDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetAverageCensusDataReport(int year, int month, int userId, string nursestationId);
        DataSet GetPsychiatricDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetNurseNotesDetailsReport(string fromdate, string todate, int userId, string nursestationId, string commentType,string MedicationReason);
        DataSet GetPrescriberNotesReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetEmarResidentdetailsReport(int month, int year, string nursestationId, string patientId, int userId);
        string GetEmarResidentlegendReport(int month, int year, string nursestationId, string patientId, int userId);        
        DataSet GetEmarResidentdetailsReportAllergyAndDiagnosis(int month, int year, string nursestationId, string patientId, int userId);
        DataSet GetInboundDetails(string fromdate, string todate,string residentName,int inboundStatus);
        DataSet GetMARHoledetails(int month, int year, string nursingStationId, int userId, string patientID);
        DataSet GetMARHistorydetails(int month,int year,string nursingStationId,int userId,string patientID);
        IList GetResidentnamesmarReport(string nursingStationIds ,  int? ResidentStatus);
        DataSet GetEmarResidentReportSecurity(int nursingStationId, string date, string passTime ,int shiftId, int window);
        DataSet GetOutBoundErrorDetails(string fromdate, string todate);
        DataSet GetUserActivityDetailsReport(string userId, string fromdate, string todate);
        DataSet GetEkitDetailsReport(/*string fromdate, string todate, */int userId, string nursestationId,int ekitType);
        DataSet GetStockDetailsReport(int userId,int companyId);
        DataSet GetAdminUsersDetailsReport(string companyId,int userId);
        DataSet GetSetUpConfigDetailsReport(string companyId);
        DataSet GetRefillDetailsReport(string fromdate, string todate, int userId, string nursestationId);
        string GetNurseStationShiftTime(int nsShiftId,int nurseStationId);
        DataSet GetAllergyMasterExcel(int type);
        DataSet GetICDMasterExcel();
        DataSet GetRoleConfigExcel(int userId,int roleId);
        DataSet GetEkitMedsDispensingReport(string fromdate, string todate, int userId, string nursestationId);
        DataSet GetDocAdministerOrderReport(string fromdate, string todate, int OrderType, int userId, string nursestationId);
        DataSet GetCertifiedOrderReport(int userId,int certTimeId, int patientId);
        DataSet GetPharmacyMedsExpiryReport(string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate, string nursestationId,int userId);
        DataSet GetEkitMedsExpiryReport(/*string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate, */string nursestationId,int facilityId, int userId);
        DataSet GetCPOEOrderDetailsReport(int porder_id, int quantityId);
        DataSet GetProfileCertifiedOrderReport(int userId, int certTimeId, int patientId);
        DataSet GetTherapeuticaltwo(int month, int year, string nursestationId, string patientId, string commentType);
        string GetTherapeuticaltwolegend(int month, int year, string nursestationId, string patientId, string commentType);
        DataSet GetTherapeuticalMedication(int month, int year, string nursestationId, string patientId, string commentType, string shiftTime);
        string GetTherapeuticalMedicationlegend(int month, int year, string nursestationId, string patientId, string commentType, string shiftTime);
        string GetEmarResidentdetailsReportAllergyAndDiagnosisCPOE(int month, int year, string nursestationId, string patientId, int userId);
        DataSet GetEkitMedicationCheckInReport(string fromdate, string todate, int userId, string nursestationid);
        DataSet GetMedicationQtyonhandUpdateReport(string fromdate, string todate, int userId, string nursestationid);
    }


}
