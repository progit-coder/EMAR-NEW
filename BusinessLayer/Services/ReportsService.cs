using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using System.Net.Mail;
using System.Collections;
using System.Data;
using LTCPro.DAL;
using System.Data.Entity;

namespace LTCPro.ServiceLayer
{
    public class ReportsService : IReportsService
    {
        private readonly IDbContextEmar dbContext;
        private readonly IAutoMapper _autoMapper;
        private readonly IReportsRepository _reportsRepository;
        private readonly ILogger _log;
        public ReportsService(IAutoMapper autoMapper, IDbContextEmar dbContext, IReportsRepository reportsRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._reportsRepository = reportsRepository;
            this._log = log;
        }
        public async Task<IList> GetReports()
        {
            this._log.Debug("---Executing GetReports() in ReportsService----");
            var entity = this._reportsRepository.GetReports();
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetCompanyReports()
        {
            this._log.Debug("---Executing GetCompanyReports() in ReportsService----");
            var entity = this._reportsRepository.GetCompanyReports();
            return await Task.FromResult(entity);

        }

        public async Task<IList> GetFacilityReports(int companyId)
        {
            this._log.Debug("---Executing GetFacilityReports() in ReportsService----");
            var entity = this._reportsRepository.GetFacilityReports(companyId);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetNursestationReports(int companyId, int facilityId)
        {
            this._log.Debug("---Executing GetNursestationReports() in ReportsService----");
            var entity = this._reportsRepository.GetNursestationReports(companyId, facilityId);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetCensusReports(int userId, string fromDate, string toDate)
        {

            this._log.Debug("---Executing GetCensusReports() in ReportsService----");
            var entity = this._reportsRepository.GetCensusReports(userId, fromDate, toDate);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetFloorReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetFloorsReport() in ReportsService----");
            var entity = this._reportsRepository.GetFloorReport(companyId, facilityId, nursestationId,floorId, wingId, roomId,bedId );
            return await Task.FromResult(entity);

        }

        public async Task<IList> GetEmarlogo(int Id = 0)
        {
            this._log.Debug("---Executing GetEmarlogo() in ReportsService----");
            var entity = this._reportsRepository.GetEmarlogo(Id);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetHeaderlogo(Nullable<int> companyId, Nullable<int> facilityId)
        {
            this._log.Debug("---Executing GetFacilitylogo() in ReportsService----");
            var entity = this._reportsRepository.GetHeaderlogo(companyId, facilityId);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetWingReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetWingReport() in ReportsService----");
            var entity = this._reportsRepository.GetWingReport(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId );
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetRoomReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetRoomReport() in ReportsService----");
            var entity = this._reportsRepository.GetRoomReport(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId );
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetBedReport(int companyId, int facilityId, int nursestationId, int floorId, int wingId, int roomId, int bedId)
        {
            this._log.Debug("---Executing GetBedReport() in ReportsService----");
            var entity = this._reportsRepository.GetBedReport(companyId, facilityId, nursestationId, floorId, wingId, roomId, bedId );
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetCensusReportByfilter(string fromdate, string todate, int userId, string nursestationId, int type, int reportType)
        {
            this._log.Debug("---Executing GetCensusReportByfilter() in ReportsService----");
            var entity = this._reportsRepository.GetCensusReportByfilter(fromdate, todate, userId, nursestationId, type, reportType);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetCensusReportByDates(string fromdate, string todate, int userId, string nursestationname, int type, int reportType, string value)
        {
            this._log.Debug("---Executing GetCensusReportByDates() in ReportsService----");
            var entity = this._reportsRepository.GetCensusReportByDates(fromdate, todate, userId, nursestationname, type, reportType, value);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetAllAllergiesByClassReport()
        {
            this._log.Debug("---Executing GetAllAllergiesByClassReport() in ReportsService----");
            var entity = this._reportsRepository.GetAllAllergiesByClassReport();
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetAllAllergiesByDrugReport()
        {
            this._log.Debug("---Executing GetAllAllergiesByDrugReport() in ReportsService----");
            var entity = this._reportsRepository.GetAllAllergiesByDrugReport();
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetAllICD10Report()
        {
            this._log.Debug("---Executing GetAllICD10Report() in ReportsService----");
            var entity = this._reportsRepository.GetAllICD10Report();
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> Get72HoursReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing Get72HoursReport() in ReportsService----");
            var entity = this._reportsRepository.Get72HoursReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetPRNDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetPRNDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetPRNDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetOrderDetailsReport(string patientIds, string fromdate, string todate, int ordertype, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetOrderDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetOrderDetailsReport(patientIds, fromdate, todate, ordertype, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetWithoutBarcodeDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetWithoutBarcodeDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetWithoutBarcodeDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetBiometricsDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetBiometricsDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetBiometricsDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetOrderControlSignoffReport(string fromdate, string todate, string nursestationId, int userId)
        {
            this._log.Debug("---Executing GetOrderControlSignoffReport() in ReportsService----");
            var entity = this._reportsRepository.GetOrderControlSignoffReport(fromdate, todate, nursestationId, userId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetOrderControlSubstanceReport(string nursestationId, int userId)
        {
            this._log.Debug("---Executing GetOrderControlSubstanceReport() in ReportsService----");
            var entity = this._reportsRepository.GetOrderControlSubstanceReport(nursestationId, userId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetOrderHoldDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetOrderControlSubstanceReport() in ReportsService----");
            var entity = this._reportsRepository.GetOrderHoldDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetOrderWithFavouritesReport(string passtime, string fromdate, string todate, int userId, string nursestationId,int shiftTime)
        {
            this._log.Debug("---Executing GetOrderWithFavouritesReport() in ReportsService----");
            var entity = this._reportsRepository.GetOrderWithFavouritesReport(passtime, fromdate, todate, userId, nursestationId, shiftTime);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetCompareCensusDataReport(string year, int userId, string nursestationId, int type, int reporttype)
        {
            this._log.Debug("---Executing GetCompareCensusDataReport() in ReportsService----");
            var entity = this._reportsRepository.GetCompareCensusDataReport(year, userId, nursestationId, type, reporttype);
            return await Task.FromResult(entity);
        }
         
        public async Task<DataSet> GetRefusedByResidentDetailsReport(string fromdate, string todate, int userId, string nursestationId,string gpi, string datetime)
        {
            this._log.Debug("---Executing GetRefusedByResidentDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetRefusedByResidentDetailsReport(fromdate, todate, userId, nursestationId, gpi, datetime);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetMedrefReport(string fromdate, string todate, int userId, string nursestationId, string gpi)
        {
            this._log.Debug("---Executing GetRefusedByResidentDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetMedrefReport(fromdate, todate, userId, nursestationId, gpi);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetOrderChangeDetailsReport(string fromdate, string todate, int userId, string nursestationId,string residentId)
        {
            //this._log.Debug("---Executing GetOrderChangeDetailsReport() in ReportsService----");
            //var entity = this._reportsRepository.GetOrderChangeDetailsReport(fromdate, todate,  userId, nursestationId,residentId);
            //return await Task.FromResult(entity);
            DataSet result = new DataSet();
            this._log.Debug("---Executing GetOrderChangeDetailsReport() in OrdersService----");
            using (EMAREntities context = new EMAREntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        result = this._reportsRepository.GetOrderChangeDetailsReport(fromdate, todate, userId, nursestationId, residentId);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return result;
                    }
                }
            }
            return result;
        }

        public async Task<DataSet> GetWithoutScanningDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetWithoutScanningDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetWithoutScanningDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetDestructionDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetWithoutScanningDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetDestructionDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetFloorStockDetailsReport(int userId, string nursestationId,int facilityId)
        {
            this._log.Debug("---Executing GetFloorStockDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetFloorStockDetailsReport(userId, nursestationId,facilityId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetPharmacyMedsDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetPharmacyMedsDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetPharmacyMedsDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetAverageCensusDataReport(int year, int month, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetAverageCensusDataReport() in ReportsService----");
            var entity = this._reportsRepository.GetAverageCensusDataReport(year, month, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetPsychiatricDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetPsychiatricDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetPsychiatricDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetNurseNotesDetailsReport(string fromdate, string todate, int userId, string nursestationId, string commentType,string MedicationReason)
        {
            this._log.Debug("---Executing GetNurseNotesDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetNurseNotesDetailsReport(fromdate, todate, userId, nursestationId, commentType, MedicationReason);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetPrescriberNotesReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetPrescriberNotesReport() in ReportsService----");
            var entity = this._reportsRepository.GetPrescriberNotesReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async  Task<DataSet> GetEmarResidentdetailsReport(int month, int year, string nursestationId, string patientId, int userId)
        {
            this._log.Debug("---Executing GetEmarResidentdetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetEmarResidentdetailsReport(month, year, nursestationId, patientId, userId);
            return await Task.FromResult(entity);
        }
        
        public async Task<string> GetEmarResidentlegendReport(int month, int year, string nursestationId, string patientId, int userId)
        {
            this._log.Debug("---Executing GetEmarResidentdetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetEmarResidentlegendReport(month, year, nursestationId, patientId, userId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetTherapeuticaltwo(int month, int year, string nursestationId, string patientId, string commentType)
        {
            this._log.Debug("---Executing GetTherapeuticaltwoReport() in ReportsService----");
            var entity = this._reportsRepository.GetTherapeuticaltwo(month, year, nursestationId, patientId, commentType);
            return await Task.FromResult(entity);
        }
        public async Task<string> GetTherapeuticaltwolegend(int month, int year, string nursestationId, string patientId, string commentType)
        {
            this._log.Debug("---Executing GetTherapeuticaltwoReport() in ReportsService----");
            var entity = this._reportsRepository.GetTherapeuticaltwolegend(month, year, nursestationId, patientId, commentType);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetTherapeuticalMedication(int month, int year, string nursestationId, string patientId, string commentType, string shiftTime)
        {
            this._log.Debug("---Executing GetTherapeuticalMedicationReport() in ReportsService----");
            var entity = this._reportsRepository.GetTherapeuticalMedication(month, year, nursestationId, patientId, commentType, shiftTime);
            return await Task.FromResult(entity);
        }
        public async Task<string> GetTherapeuticalMedicationlegend(int month, int year, string nursestationId, string patientId, string commentType, string shiftTime)
        {
            this._log.Debug("---Executing GetTherapeuticalMedicationReport() in ReportsService----");
            var entity = this._reportsRepository.GetTherapeuticalMedicationlegend(month, year, nursestationId, patientId, commentType, shiftTime);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetEmarResidentdetailsReportAllergyAndDiagnosis(int month, int year, string nursestationId, string patientId, int userId)
        {
            this._log.Debug("---Executing GetEmarResidentdetailsReportAllergyAndDiagnosis() in ReportsService----");
            var entity = this._reportsRepository.GetEmarResidentdetailsReportAllergyAndDiagnosis(month, year, nursestationId, patientId, userId);
            return await Task.FromResult(entity);
        }

        public async Task<string> GetEmarResidentdetailsReportAllergyAndDiagnosisCPOE(int month, int year, string nursestationId, string patientId, int userId)
        {
            this._log.Debug("---Executing GetEmarResidentdetailsReportAllergyAndDiagnosisCPOE() in ReportsService----");
            var entity = this._reportsRepository.GetEmarResidentdetailsReportAllergyAndDiagnosisCPOE(month, year, nursestationId, patientId, userId);
            return await Task.FromResult(entity);
        }

        

        public async Task<DataSet> GetInboundDetails(string fromdate,string todate,string residentName, int inboundStatus)
        {
            this._log.Debug("---Executing GetInboundDetails() in ReportService----");
            var entity = this._reportsRepository.GetInboundDetails(fromdate, todate, residentName, inboundStatus);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetMARHoledetails(int month, int year, string nursingStationId, int userId, string patientID)
        {
            this._log.Debug("---Executing GetMARHoledetails() in ReportService----");
            var entity = this._reportsRepository.GetMARHoledetails(month, year, nursingStationId, userId, patientID);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetMARHistorydetails(int Month, int Year, string NursingStationId, int UserId, string PatientID)
        {
            this._log.Debug("---Executing GetMARHistorydetails() in ReportService----");
            var entity = this._reportsRepository.GetMARHistorydetails(Month, Year, NursingStationId, UserId, PatientID);
            return await Task.FromResult(entity);
        }

        public async Task<IList> GetResidentnamesmarReport(string nursingStationIds , int? ResidentStatus)
        {
            this._log.Debug("---Executing GetResidentnamesmarReport() in ReportService----");
            var entity = this._reportsRepository.GetResidentnamesmarReport(nursingStationIds , ResidentStatus);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetEmarResidentReportSecurity(int nursingStationId, string date, string passTime, int shiftId, int window)
        {
            this._log.Debug("---Executing GetEmarResidentReportSecurity() in ReportService----");
            var entity = this._reportsRepository.GetEmarResidentReportSecurity(nursingStationId, date, passTime, shiftId, window);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetOutBoundErrorDetails(string fromdate, string todate)
        {
            this._log.Debug("---Executing GetOutBoundErrorDetails() in ReportSErvice----");
            var entity = this._reportsRepository.GetOutBoundErrorDetails(fromdate, todate);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetUserActivityDetailsReport(string userId, string fromdate, string todate)
        {
            this._log.Debug("---Executing GetOutBoundErrorDetails() in ReportSErvice----");
            var entity = this._reportsRepository.GetUserActivityDetailsReport(userId, fromdate, todate);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetEkitDetailsReport(/*string fromdate, string todate,*/ int userId, string nursestationId, int ekitType)
        {
            this._log.Debug("---Executing GetEkitDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetEkitDetailsReport(/*fromdate, todate,*/ userId, nursestationId,ekitType);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetStockDetailsReport(int userId,int companyId)
        {
            this._log.Debug("---Executing GetStockDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetStockDetailsReport(userId, companyId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetAdminUsersDetailsReport(string companyId, int userId)
        {
            this._log.Debug("---Executing GetAdminUsersDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetAdminUsersDetailsReport(companyId,userId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetSetUpConfigDetailsReport(string companyId)
        {
            this._log.Debug("---Executing GetSetUpConfigDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetSetUpConfigDetailsReport(companyId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetRefillDetailsReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetRefillDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetRefillDetailsReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }
        public async Task<string> GetNurseStationShiftTime(int nsShiftId, int nurseStationId)
        {
            this._log.Debug("---Executing GetOutBoundErrorDetails() in ReportSErvice----");
            var entity = this._reportsRepository.GetNurseStationShiftTime(nsShiftId, nurseStationId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetAllergyMasterExcel(int type)
        {
            this._log.Debug("---Executing GetAllergyMasterExcel() in ReportsService----");
            var entity = this._reportsRepository.GetAllergyMasterExcel(type);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetICDMasterExcel()
        {
            this._log.Debug("---Executing GetICDMasterExcel() in ReportsService----");
            var entity = this._reportsRepository.GetICDMasterExcel();
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetRoleConfigExcel(int userId, int roleId)
        {
            this._log.Debug("---Executing GetRoleConfigExcel() in ReportsService----");
            var entity = this._reportsRepository.GetRoleConfigExcel(userId, roleId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetEkitMedsDispensingReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetEkitMedsDispensingReport() in ReportsService----");
            var entity = this._reportsRepository.GetEkitMedsDispensingReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }

        public async Task<DataSet> GetDocAdministerOrderReport(string fromdate, string todate, int OrderType, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetDocAdministerOrderReport() in ReportsService----");
            var entity = this._reportsRepository.GetDocAdministerOrderReport(fromdate, todate, OrderType, userId, nursestationId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetCertifiedOrderReport(int userId, int certTimeId, int patientId)
        {
            this._log.Debug("---Executing GetCertifiedOrderReport() in ReportsService----");
            var entity = this._reportsRepository.GetCertifiedOrderReport(userId, certTimeId, patientId);
            return await Task.FromResult(entity);
        }
        public async Task<string> GetNursingStationNameByPId(int patientId)
        {
            this._log.Debug("---Executing GetCertifiedOrderReport() in ReportsService----");
            string nursingStationName = "";
            var record = this.dbContext.VisitInfoes.Where(v => v.Patient_Id == patientId).OrderByDescending(v => v.AdmitDate).FirstOrDefault();
            if(record!=null)
            {
                nursingStationName = this.dbContext.NursingStations.Where(n => n.NurseStation_Id == record.NursingStationId).Select(n => n.NurseStation_Name).FirstOrDefault();
            }
            return await Task<string>.FromResult(nursingStationName);
        }
        public async Task<DataSet> GetPharmacyMedsExpiryReport(string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate, string nursestationId,int userId)
        {
            this._log.Debug("---Executing GetPharmacyMedsExpiryReport() in ReportsService----");
            var entity = this._reportsRepository.GetPharmacyMedsExpiryReport(checkinFromDate, checkinToDate, expireFromDate, expireTodate,nursestationId,userId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetEkitMedsExpiryReport(/*string checkinFromDate, string checkinToDate, string expireFromDate, string expireTodate,*/ string nursestationId, int facilityId,int userId)
        {
            this._log.Debug("---Executing GetEkitMedsExpiryReport() in ReportsService----");
            var entity = this._reportsRepository.GetEkitMedsExpiryReport(/*checkinFromDate, checkinToDate, expireFromDate, expireTodate,*/ nursestationId, facilityId, userId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetCPOEOrderDetailsReport(int porder_id, int quantityId)
        {
            this._log.Debug("---Executing GetCPOEOrderDetailsReport() in ReportsService----");
            var entity = this._reportsRepository.GetCPOEOrderDetailsReport(porder_id, quantityId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetProfileCertifiedOrderReport(int userId, int certTimeId, int patientId)
        {
            this._log.Debug("---Executing GetProfileCertifiedOrderReport() in ReportsService----");
            var entity = this._reportsRepository.GetProfileCertifiedOrderReport(userId, certTimeId, patientId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetEkitMedicationCheckInReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetEkitMedicationCheckInReport() in ReportsService----");
            var entity = this._reportsRepository.GetEkitMedicationCheckInReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }
        public async Task<DataSet> GetMedicationQtyonhandUpdateReport(string fromdate, string todate, int userId, string nursestationId)
        {
            this._log.Debug("---Executing GetMedicationQtyonhandUpdateReport() in ReportsService----");
            var entity = this._reportsRepository.GetMedicationQtyonhandUpdateReport(fromdate, todate, userId, nursestationId);
            return await Task.FromResult(entity);
        }
    }
}
