using LTCPro.Entities;
using LTCPro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public class DashboardsService : IDashboardsService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDashboardsRepository _dashboardsRepository;
        private readonly ILogger _log;

        public DashboardsService(IAutoMapper autoMapper, IDashboardsRepository dashboardsRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._dashboardsRepository = dashboardsRepository;
            this._log = log;
        }
        public async Task<DashBoardEntity> GetCensusDashboard(string moduleName, string seriesName, string yAxis, string category, int userId, string fromDate, string toDate, int filterType, int reportType, string nurseStations)
        {
            this._log.Debug("---Executing GetCommonDashboard() in DashboardsService---- ");
            DashBoardEntity entity = this._dashboardsRepository.GetCensusDashboard(moduleName, seriesName, yAxis, category, userId, fromDate, toDate, filterType, reportType, nurseStations);
            return await Task.FromResult(entity);
        }
        public async Task<DashBoardEntity> GetCensusCompareDashboard(string years, int userId, int nursingstationId, int type, int reportType)
        {
            this._log.Debug("---Executing GetCensusCompareDashboard() in DashboardsService---- ");
            DashBoardEntity entity = this._dashboardsRepository.GetCensusCompareDashboard(years, userId, nursingstationId, type, reportType);
            return await Task.FromResult(entity);
        }
        public async Task<DashBoardEntity> GetAverageCensusDashboard(int year, int month, int userId, string nursingstation)
        {
            this._log.Debug("---Executing GetCensusCompareDashboard() in DashboardsService---- ");
            DashBoardEntity entity = this._dashboardsRepository.GetAverageCensusDashboard(year, month, userId, nursingstation);
            return await Task.FromResult(entity);
        }
       
        public async Task<DashBoardEntity> GetCommonDashboard(string dashboardName, string fromDate, string toDate, int userId, string nursingstationId, int currentPage, int pageSize, string passTime, Nullable<int> orderType, int month,int year, int patientId,string patientName,string commentType, string userIds, string shiftTime, int facilityid,string MedicationReason,string dateTime)
        {
            this._log.Debug("---Executing GetCommonDashboard() in DashboardsService---- ");
            DashBoardEntity entity = this._dashboardsRepository.GetCommonDashboard(dashboardName, fromDate, toDate, userId, nursingstationId, currentPage, pageSize, passTime, orderType, month,year, patientId, patientName, commentType, userIds,shiftTime, facilityid, MedicationReason, dateTime);
            return await Task.FromResult(entity);
        }

        public async Task<string> GetPatientAllergiesByName(string patientName)
        {
            this._log.Debug("---Executing GetPatientAllergiesByName() in DashboardsService---- ");
            string allergies = this._dashboardsRepository.GetPatientAllergiesByName(patientName);
            return await Task.FromResult(allergies);
        }
        public async Task<List<UserActivityReportEntity>> GetUserActivityDetails(Int64 sessionId)
        {
            this._log.Debug("---Executing GetUserActivityDetails() in DashboardsService---- ");
            var allergies = this._dashboardsRepository.GetUserActivityDetails(sessionId);
            return await Task.FromResult<List<UserActivityReportEntity>>(allergies);
        }
        public async Task<PrcReportGetStockDataGridEntity> GetStockDashboardGrid(int userId, int companyId, int currentPage, int pageSize)
        {
            this._log.Debug("---Executing GetStockDashboardGrid() in DashboardsService---- ");
            var result = this._dashboardsRepository.GetStockDashboardGrid(userId, companyId, currentPage, pageSize);
            return await Task.FromResult<PrcReportGetStockDataGridEntity>(result);
        }
        public async Task<PrcReportAdminUsersGridEntity> GetAdminUseraDashboardGrid(string companyId, int userId, int currentPage, int pageSize)
        {
            this._log.Debug("---Executing GetAdminUseraDashboardGrid() in DashboardsService---- ");
            var result = this._dashboardsRepository.GetAdminUseraDashboardGrid(companyId, userId, currentPage, pageSize);
            return await Task.FromResult<PrcReportAdminUsersGridEntity>(result);
        }
        public async Task<PrcReportSetUpConfigGridEntity> GetSetUpConfigDashboardGrid(string companyId, int currentPage, int pageSize)
        {
            this._log.Debug("---Executing GetSetUpConfigDashboardGrid() in DashboardsService---- ");
            var result = this._dashboardsRepository.GetSetUpConfigDashboardGrid(companyId, currentPage, pageSize);
            return await Task.FromResult<PrcReportSetUpConfigGridEntity>(result);
        }
        public async Task<DashBoardEntity> GetInboundDetailsDashboard(InboundDashboardCustomEntity entity)
        {
            this._log.Debug("---Executing GetInboundDetailsDashboard() in DashboardsService---- ");
            DashBoardEntity result = this._dashboardsRepository.GetInboundDetailsDashboard(entity);
            return await Task.FromResult(result);
        }
        public async Task<DashBoardEntity> GetOutboundDetailsDashboard(OutboundDashboardCustomEntity entity)
        {
            this._log.Debug("---Executing GetOutbondDetailsDashboard() in DashboardsService---- ");
            DashBoardEntity result = this._dashboardsRepository.GetOutboundDetailsDashboard(entity);
            return await Task.FromResult(result);
        }
        public async Task<DashBoardEntity> GetPharmacyMedsExpiryDashboard(PharmacyMedsExpiryDateDashboardd entity)
        {
            this._log.Debug("---Executing GetPharmacyMedsExpiryDashboard() in DashboardsService---- ");
            DashBoardEntity result = this._dashboardsRepository.GetPharmacyMedsExpiryDashboard(entity);
            return await Task.FromResult(result);
        }
        public async Task<DashBoardEntity> GetEkitMedsExpiryDashboard(EKitMedsExpiryDateDashboard entity)
        {
            this._log.Debug("---Executing GetEkitMedsExpiryDashboard() in DashboardsService---- ");
            DashBoardEntity result = this._dashboardsRepository.GetEkitMedsExpiryDashboard(entity);
            return await Task.FromResult(result);
        }
        public async Task<DashBoardEntity> GetScheduledOrderDashboard(ScheduleOrderEntity obj)
        {
            this._log.Debug("---Executing GetScheduledOrderDashboard() in DashboardsService---- ");
            DashBoardEntity result = this._dashboardsRepository.GetScheduledOrderDashboard(obj);
            return await Task.FromResult(result);
        }
        public async Task<DashBoardEntity> GetMedicationActiveInventoryReportDashboard(GetMedicationActiveInventoryReportDashboard entity)
        {
            this._log.Debug("---Executing GetMedicationActiveInventoryReportDashboard() in DashboardsService---- ");
            DashBoardEntity result = this._dashboardsRepository.GetMedicationActiveInventoryReportDashboard(entity);
            return await Task.FromResult(result);
        }

    }
}
