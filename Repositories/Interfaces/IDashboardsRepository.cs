using LTCPro.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Repositories
{
    public interface IDashboardsRepository
    {
        DashBoardEntity GetCensusDashboard(string moduleName, string seriesName, string yAxis, string category, int userId, string fromDate, string toDate, int filterType, int reportType, string nurseStations);
        DashBoardEntity GetCensusCompareDashboard(string years, int userId, int nursingstationId, int type, int reportType);
        DashBoardEntity GetCommonDashboard(string dashboardName, string fromDate, string toDate, int userId, string nursingstationId, int currentPage, int pageSize, string passTime, Nullable<int> orderType, int month,int year, int patientId,string patientName,string commentType, string userIds, string shiftTime, int facilityid,string MedicationReason,string datetime);
        DashBoardEntity GetAverageCensusDashboard(int year, int month, int userId, string nursingstation);
        string GetPatientAllergiesByName(string patientName);
        List<UserActivityReportEntity> GetUserActivityDetails(Int64 sessionId);
        PrcReportGetStockDataGridEntity GetStockDashboardGrid(int userId,int companyId, int currentPage, int pageSize);
        PrcReportAdminUsersGridEntity GetAdminUseraDashboardGrid(string companyId,int userId, int currentPage, int pageSize);
        PrcReportSetUpConfigGridEntity GetSetUpConfigDashboardGrid(string companyId, int currentPage, int pageSize);
        DashBoardEntity GetInboundDetailsDashboard(InboundDashboardCustomEntity entity);
        DashBoardEntity GetOutboundDetailsDashboard(OutboundDashboardCustomEntity entity);
        DashBoardEntity GetPharmacyMedsExpiryDashboard(PharmacyMedsExpiryDateDashboardd entity);
        DashBoardEntity GetEkitMedsExpiryDashboard(EKitMedsExpiryDateDashboard entity);
        DashBoardEntity GetScheduledOrderDashboard(ScheduleOrderEntity entity);
        DashBoardEntity GetMedicationActiveInventoryReportDashboard(GetMedicationActiveInventoryReportDashboard entity);
    }
}
