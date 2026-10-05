using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using WebApi.Filters;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [CustomAuthorizationFilter]
    [RoutePrefix("Dashboard")]
    public class DashboardsController : ApiController
    {
        private readonly IDashboardsService _dashboardService;
        private readonly ILogger _log;
        public DashboardsController(IDashboardsService dashboardService, ILogger log)
        {
            this._dashboardService = dashboardService;
            this._log = log;
        }
        [Route("GetCensusDashboard/{ModuleName}/{UserId}/{FromDate}/{ToDate}/{FilterType}/{ReportType}/{SeriesName?}/{Yaxis?}/{Category?}/{NurseStations?}")]
        [HttpGet]
        //category - zaxis, series - xaxis, 
        public JsonResult<DashBoardEntity> GetCensusDashboard(string moduleName, int userId, string fromDate, string toDate, int filterType, int reportType, string seriesName = "", string yAxis = "", string category = "", string nurseStations = "")
        {
            this._log.Debug("---Executing GetCensusDashboard() for " + moduleName + " in DashboardsController----");
            var records = this._dashboardService.GetCensusDashboard(moduleName, seriesName, yAxis, category, userId, fromDate, toDate, filterType, reportType, nurseStations).Result;
            this._log.Debug("---Executed Successfully GetCensusDashboard() " + moduleName + "  in DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetCensusCompareDashboard/{Years}/{UserId}/{nursingstationId}/{Type}/{ReportType}")]
        [HttpGet]
        public JsonResult<DashBoardEntity> GetCensusCompareDashboard(string years, int userId, int nursingstationId, int type, int reportType)
        {
            this._log.Debug("---Executing GetCensusCompareDashboard() in DashboardsController----");
            var records = this._dashboardService.GetCensusCompareDashboard(years, userId, nursingstationId, type, reportType).Result;
            this._log.Debug("---Executed Successfully GetCensusCompareDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetAverageCensusDashboard/{Year}/{Month}/{UserId}/{Nursingstation}")]
        [HttpGet]
        public JsonResult<DashBoardEntity> GetAverageCensusDashboard(int year, int month, int userId, string nursingStation)
        {
            this._log.Debug("---Executing GetAverageCensusDashboard() in DashboardsController----");
            var records = this._dashboardService.GetAverageCensusDashboard(year, month, userId, nursingStation).Result;
            this._log.Debug("---Executed Successfully GetAverageCensusDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetCommonDashboard/{DashboardName}/{FromDate}/{ToDate}/{UserId}/{NursingstationId}/{currentPage}/{pageSize}/{PassTime?}/{OrderType?}/{Month?}/{Year?}/{PatientId?}/{patientName?}/{commentType?}/{userIds?}/{shiftTime?}/{facilityid?}/{MedicationReason?}")]
        [HttpGet]
        public JsonResult<DashBoardEntity> GetCommonDashboard(string dashboardName, string fromDate, string toDate, int userId, string nursingstationId, int currentPage, int pageSize,string passTime = "", Nullable<int> orderType = null, int month =0 ,int year =0, int patientId = 0,string patientName="",string commentType="",string userIds ="",string shiftTime ="", int facilityid = 0,string MedicationReason="")
         {
            this._log.Debug("---Executing GetCommonDashboard() in DashboardsController----");
            var records = this._dashboardService.GetCommonDashboard(dashboardName, fromDate, toDate, userId, nursingstationId, currentPage,pageSize,passTime, orderType, month,year, patientId, patientName, commentType,userIds,shiftTime, facilityid, MedicationReason,"").Result;
            this._log.Debug("---Executed Successfully GetCommonDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }

       
        [Route("GetRefdata")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetRefdata(MedRefEntity data)
        {
            // string dashboardName, string fromDate, string toDate, int userId, string nursingstationId, int currentPage, int pageSize, string passTime = "",
            Nullable<int> orderType = null;
            try
            {
                int month = 0;
                int year = 0;
                int patientId = 0;
                string patientName = "";
                string commentType = "";
                string userIds = "";
                string shiftTime = "";
                int facilityid = 0;
                string MedicationReason = "";
               
                this._log.Debug("---Executing GetCommonDashboard() in DashboardsController----");
                var records = this._dashboardService.GetCommonDashboard(data.dashboardName, data.fromdate, data.todate, data.userId, data.nursingstationId, data.currentPage, data.pageSize, data.passTime, orderType, month, year, patientId, patientName, commentType, userIds, shiftTime, facilityid, MedicationReason,data.dateTime).Result;
                this._log.Debug("---Executed Successfully GetCommonDashboard() DashboardsController----");
                return Json<DashBoardEntity>(records);
            }
            catch(Exception ex)
            {
                this._log.Debug("---Executing GetCommonDashboard() in DashboardsController Error----" + ex.Message.ToString() + "---" + ex.InnerException.Message.ToString());
                return null;
            }
           
        }
        [Route("GetCommonDashboard1")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetCommonDashboard1(UserActvivtysEntity Model)
        {
            this._log.Debug("---Executing GetCommonDashboard() in DashboardsController----");
            var records = this._dashboardService.GetCommonDashboard(Model.dashboardName, Model.fromDate, Model.toDate, Model.userId, Model.nursingstationId, Model.currentPage, Model.pageSize, Model.passTime, Model.orderType, Model.month, Model.year, Model.patientId, Model.patientName, Model.commentType, Model.userIds, Model.shiftTime, Model. facilityid,"","").Result;
            this._log.Debug("---Executed Successfully GetCommonDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetPatientAllergiesByName/{PatientName}")]
        [HttpGet]
        public JsonResult<string> GetPatientAllergiesByName(string patientName)
        {
            this._log.Debug("---Executing GetPatientAllergiesByName() in DashboardsController----");
            var records = this._dashboardService.GetPatientAllergiesByName(patientName).Result;
            this._log.Debug("---Executed Successfully GetPatientAllergiesByName() DashboardsController----");
            return Json<string>(records);
        }
        [Route("GetUserActivityDetails/{sessionId}")]
        [HttpGet]
        public JsonResult<List<UserActivityReportEntity>> GetUserActivityDetails(Int64 sessionId)
        {
            this._log.Debug("---Executing GetUserActivityDetails() in DashboardsController----");
            var records = this._dashboardService.GetUserActivityDetails(sessionId).Result;
            this._log.Debug("---Executed Successfully GetUserActivityDetails() DashboardsController----");
            return Json<List<UserActivityReportEntity>>(records);
        }
        [Route("GetStockDashboardGrid/{userId}/{companyId}/{currentPage}/{pageSize}")]
        [HttpGet]
        public JsonResult<PrcReportGetStockDataGridEntity> GetStockDashboardGrid(int userId, int companyId, int currentPage, int pageSize)
        {
            this._log.Debug("---Executing GetStockDashboardGrid() in DashboardsController----");
            var records = this._dashboardService.GetStockDashboardGrid(userId, companyId, currentPage, pageSize).Result;
            this._log.Debug("---Executed Successfully GetStockDashboardGrid() DashboardsController----");
            return Json<PrcReportGetStockDataGridEntity>(records);
        }
        [Route("GetAdminUseraDashboardGrid/{companyId}/{userId}/{currentPage}/{pageSize}")]
        [HttpGet]
        public JsonResult<PrcReportAdminUsersGridEntity> GetAdminUseraDashboardGrid(string companyId, int userId, int currentPage, int pageSize)
        {
            this._log.Debug("---Executing GetAdminUseraDashboardGrid() in DashboardsController----");
            var records = this._dashboardService.GetAdminUseraDashboardGrid(companyId, userId, currentPage, pageSize).Result;
            this._log.Debug("---Executed Successfully GetAdminUseraDashboardGrid() DashboardsController----");
            return Json<PrcReportAdminUsersGridEntity>(records);
        }
        [Route("GetSetUpConfigDashboardGrid/{companyId}/{currentPage}/{pageSize}")]
        [HttpGet]
        public JsonResult<PrcReportSetUpConfigGridEntity> GetSetUpConfigDashboardGrid(string companyId, int currentPage, int pageSize)
        {
            this._log.Debug("---Executing GetSetUpConfigDashboardGrid() in DashboardsController----");
            var records = this._dashboardService.GetSetUpConfigDashboardGrid(companyId, currentPage, pageSize).Result;
            this._log.Debug("---Executed Successfully GetSetUpConfigDashboardGrid() DashboardsController----");
            return Json<PrcReportSetUpConfigGridEntity> (records);
        }
        [Route("GetInboundDetailsDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetInboundDetailsDashboard(InboundDashboardCustomEntity entity)
        {
            this._log.Debug("---Executing GetInboundDetailsDashboard() in DashboardsController----");
            var records = this._dashboardService.GetInboundDetailsDashboard(entity).Result;
            this._log.Debug("---Executed Successfully GetInboundDetailsDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetOutbondDetailsDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetOutboundDetailsDashboard(OutboundDashboardCustomEntity entity)
        {
            this._log.Debug("---Executing GetOutboundDetailsDashboard() in DashboardsController----");
            var records = this._dashboardService.GetOutboundDetailsDashboard(entity).Result;
            this._log.Debug("---Executed Successfully GetOutboundDetailsDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetEmarHoleHistoryDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetEmarHoleHistoryDashboard(EmarHoleHistoryEntity entity)
        {
            this._log.Debug("---Executing GetCommonDashboard() in DashboardsController----");
            var records = this._dashboardService.GetCommonDashboard(entity.DashboardName,entity.FromDate, entity.ToDate, entity.UserId, entity.NursingstationId, entity.currentPage, entity.pageSize, entity.PassTime, entity.OrderType, entity.Month, entity.Year, entity.PatientId, entity.patientName, entity.commentType, entity.userIds, entity.shiftTime,entity.facilityId,"","").Result;
            this._log.Debug("---Executed Successfully GetCommonDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetPharmacyMedsExpiryDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetPharmacyMedsExpiryDashboard(PharmacyMedsExpiryDateDashboardd entity)
        {
            this._log.Debug("---Executing GetPharmacyMedsExpiryDashboard() in DashboardsController----");
            var records = this._dashboardService.GetPharmacyMedsExpiryDashboard(entity).Result;
            this._log.Debug("---Executed Successfully GetPharmacyMedsExpiryDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetEkitMedsExpiryDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetEkitMedsExpiryDashboard(EKitMedsExpiryDateDashboard entity)
        {
            this._log.Debug("---Executing GetEkitMedsExpiryDashboard() in DashboardsController----");
            var records = this._dashboardService.GetEkitMedsExpiryDashboard(entity).Result;
            this._log.Debug("---Executed Successfully GetEkitMedsExpiryDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetScheduledOrderDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetScheduledOrderDashboard(ScheduleOrderEntity obj)
        {
            this._log.Debug("---Executing GetScheduledOrderDashboard() in DashboardsController----");
            var records = this._dashboardService.GetScheduledOrderDashboard(obj).Result;
            this._log.Debug("---Executed Successfully GetScheduledOrderDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        [Route("GetMedicationActiveInventoryReportDashboard")]
        [HttpPost]
        public JsonResult<DashBoardEntity> GetMedicationActiveInventoryReportDashboard(GetMedicationActiveInventoryReportDashboard entity)
        {
            this._log.Debug("---Executing GetMedicationActiveInventoryReportDashboard() in DashboardsController----");
            var records = this._dashboardService.GetMedicationActiveInventoryReportDashboard(entity).Result;
            this._log.Debug("---Executed Successfully GetMedicationActiveInventoryReportDashboard() DashboardsController----");
            return Json<DashBoardEntity>(records);
        }
        public async Task<JsonResult<string>> PostDrFirstTest()
        {
            try
            {

                HttpClient httpClient = new HttpClient();

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string sampledata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.19'><Caller><VendorName>avendor2728</VendorName><VendorPassword>mr41zw7k</VendorPassword></Caller><SystemName>avendor2728</SystemName><RcopiaPracticeUsername>ph6214</RcopiaPracticeUsername><Request><Command>update_allergy</Command><LastUpdateDate>05/10/2023 06:22:03</LastUpdateDate><ReturnAllNDCIDs>y</ReturnAllNDCIDs><Patient><RcopiaID>26155609419</RcopiaID><ExternalID>abingdonperseus</ExternalID></Patient></Request></RCExtRequest>";


                var content = new FormUrlEncodedContent(new[]
                {
    new KeyValuePair<string, string>("xml", sampledata)
});
                var response = await httpClient.PostAsync("https://update201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet", content).ConfigureAwait(false);

                var responseText = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        [HttpGet]
        public async Task<JsonResult<string>> GetDrFirstTest()
        {

            try
            {
                HttpClient client = new HttpClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client.BaseAddress = new Uri("https://engine201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet");

                string sampledata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.19'><Caller><VendorName>avendor2728</VendorName >< VendorPassword > mr41zw7k </ VendorPassword ></ Caller >< SystemName > avendor2728 </ SystemName >< RcopiaPracticeUsername > ph6214 </ RcopiaPracticeUsername >< Request > ";

                client.BaseAddress = new Uri("https://engine201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet?xml=" + sampledata);



                HttpResponseMessage Res = client.GetAsync("").Result;


                if (Res.IsSuccessStatusCode)
                {

                    var Response = Res.Content.ReadAsStringAsync().Result;
                    // BulkStatusRoot data = JsonConvert.DeserializeObject<BulkStatusRoot>(Response);
                   

                }
                else
                {
                    
                }
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }

        }


        public JsonResult<string> CreateMD5(string input = "")
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // string Key =  Convert.ToString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to.NET 5
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                string Key = sb.ToString();
                return null;


            }
        }
    }
}
