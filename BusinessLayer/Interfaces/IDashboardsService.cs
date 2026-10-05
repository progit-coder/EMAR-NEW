using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.ServiceLayer
{
    public interface IDashboardsService
    {
        Task<DashBoardEntity> GetCensusDashboard(string moduleName, string seriesName, string yAxis, string category, int userId, string fromDate, string toDate, int filterType, int reportType, string nurseStations);
        Task<DashBoardEntity> GetCensusCompareDashboard(string years, int userId, int nursingstationId, int type, int reportType);
        Task<DashBoardEntity> GetCommonDashboard(string dashboardName, string fromDate, string toDate, int userId, string nursingstationId, int currentPage, int pageSize, string passTime, Nullable<int> orderType, int month,int year, int patientId,string patientName,string commentType, string userIds, string shiftTime, int facilityid,string MedicationReason,string datetime);
        Task<DashBoardEntity> GetAverageCensusDashboard(int year, int month, int userId, string nursingstation);
        Task<string> GetPatientAllergiesByName(string patientName);
        Task<List<UserActivityReportEntity>> GetUserActivityDetails(Int64 sessionId);
        Task<PrcReportGetStockDataGridEntity> GetStockDashboardGrid(int userId,int companyId, int currentPage, int pageSize);
        Task<PrcReportAdminUsersGridEntity> GetAdminUseraDashboardGrid(string companyId, int userId, int currentPage, int pageSize);
        Task<PrcReportSetUpConfigGridEntity> GetSetUpConfigDashboardGrid(string companyId, int currentPage, int pageSize);
        Task<DashBoardEntity> GetInboundDetailsDashboard(InboundDashboardCustomEntity entity);
        Task<DashBoardEntity> GetOutboundDetailsDashboard(OutboundDashboardCustomEntity entity);
        Task<DashBoardEntity> GetPharmacyMedsExpiryDashboard(PharmacyMedsExpiryDateDashboardd entity);
        Task<DashBoardEntity> GetEkitMedsExpiryDashboard(EKitMedsExpiryDateDashboard entity);
        Task<DashBoardEntity> GetScheduledOrderDashboard(ScheduleOrderEntity obj);
        Task<DashBoardEntity> GetMedicationActiveInventoryReportDashboard(GetMedicationActiveInventoryReportDashboard entity);


    }
}
