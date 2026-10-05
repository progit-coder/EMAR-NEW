using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using LTCPro.Entities;
using System.Web;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Inbound")]
    [CustomAuthorizationFilter]
    public class InboundFilesController : ApiController
    {
        private readonly IInboundFilesService _inboundFilesService;
        private readonly ILogger _log;

        public InboundFilesController(IInboundFilesService inboundFilesService, ILogger log)
        {
            this._inboundFilesService = inboundFilesService;
            this._log = log;
        }
        [Route("ListFiles")]
        [HttpGet]
        public JsonResult<List<string>> ListFiles()
        {
            // WriteDataToFile();
            this._log.Debug("---Executing ListFiles() in InboundFilesController----");
            var fteConfigs = this._inboundFilesService.DecryptFTPData(1).Result;
            string serverIp = fteConfigs.ServerIp + "/InBound/";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://50.87.144.11/ITTeam/InBound/");
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(fteConfigs.UserName, fteConfigs.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string names = reader.ReadToEnd();

            reader.Close();
            response.Close();

            List<string> data = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this._log.Debug("---Executed Successfully ListFiles() in InboundFilesController----");
            return Json<List<string>>(data);
        }
        [Route("GetInboundFilesFromFTP/{CompanyId}")]
        [HttpGet]
        public JsonResult<List<InboundFileInformationEntity>> GetInboundFilesFromFTP(int companyId)
        {
            // WriteDataToFile();
            this._log.Debug("---Executing GetInboundFilesFromFTP() in InboundFilesController----");
            var data = this._inboundFilesService.GetInboundFilesFromFTP(companyId).Result;
            this._log.Debug("---Executed Successfully GetInboundFilesFromFTP() in InboundFilesController----");
            return Json<List<InboundFileInformationEntity>>(data);
        }
        [Route("GetInboundFilesFromLocal/{CompanyId}")]
        [HttpGet]
        public JsonResult<List<InboundFileInformationEntity>> GetInboundFilesFromLocal(int companyId)
        {
            //  WriteDataToFile();
            this._log.Debug("---Executing GetInboundFilesFromLocal() in InboundFilesController----");
            var data = this._inboundFilesService.GetInboundFilesFromLocal(companyId).Result;
            this._log.Debug("---Executed Successfully GetInboundFilesFromLocal() in InboundFilesController----");
            return Json<List<InboundFileInformationEntity>>(data);
        }
        [Route("SaveInboundFilesData")]
        [HttpPost]
        public int SaveInboundFilesData(List<string> records)
        {
            this._log.Debug("---Executing SaveInboundFilesData() in InboundFilesController----");
            var data = this._inboundFilesService.SaveInboundFilesData(records).Result;
            this._log.Debug("---Executed Successfully SaveInboundFilesData() in InboundFilesController----");
            return data;
        }
        [Route("SaveInboundFilesDownGridData")]
        [HttpPost]
        public int SaveInboundFilesDownGridData(string body)
        {
            this._log.Debug("---Executing SaveInboundFilesDownGridData() in InboundFilesController----");
            List<int> records = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(body);
            int userId = 1;
            var data = this._inboundFilesService.SaveInboundFilesDownGridData(records, userId).Result;
            this._log.Debug("---Executed Successfully SaveInboundFilesDownGridData() in InboundFilesController----");
            return data;
        }
        [Route("GetSavedFilesList/{CompanyId}/{FileCategory}")]
        [HttpGet]
        public JsonResult<List<FileInformationEntity>> GetSavedFilesList(int companyId, string fileCategory)
        {
            this._log.Debug("---Executing GetSavedFilesList() in InboundFilesController----");
            var data = this._inboundFilesService.GetSavedFilesList(companyId, fileCategory).Result;
            this._log.Debug("---Executed Successfully GetSavedFilesList() in InboundFilesController----");
            return Json<List<FileInformationEntity>>(data);
        }
        [Route("GetHlFieldsInformation/{FileID}")]
        [HttpGet]
        public JsonResult<List<HlFieldsEntity>> GetHlFieldsInformation(int FileID)
        {
            this._log.Debug("---Executing GetHlFieldsInformation() in InboundFilesController----");
            var data = this._inboundFilesService.GetHlFieldsInformation(FileID).Result;
            this._log.Debug("---Executed Successfully GetHlFieldsInformation() in InboundFilesController----");
            return Json<List<HlFieldsEntity>>(data);
        }
        [Route("ImportInboundFiles")]
        [HttpGet]
        public int ImportInboundFiles()
        {
            this._log.Debug("---Executing ImportInboundFiles() in InboundFilesController----");
            var result = this._inboundFilesService.ImportInboundFiles();
            this._log.Debug("---Executed Successfully ImportInboundFiles() in InboundFilesController----");
            return 1;
        }
        [Route("GetFileAckData/{FileId}")]
        [HttpGet]
        public JsonResult<FileInformationViewCustomEntity> GetFileAckData(int FileId)
        {
            this._log.Debug("---Executing GetFileAckData() in InboundFilesController----");
            var data = this._inboundFilesService.GetFileAckData(FileId).Result;
            this._log.Debug("---Executed Successfully GetFileAckData() in InboundFilesController----");
            return Json<FileInformationViewCustomEntity>(data);
        }
        [Route("GetInboundDashboard")]
        [HttpGet]
        public JsonResult<InboundDashBoardDisplay> GetInboundDashboard()
        {
            this._log.Debug("---Executing GetInboundDashboard() in InboundFilesController----");
            var data = this._inboundFilesService.GetInboundDashboard().Result;
            this._log.Debug("---Executed Successfully GetInboundDashboard() in InboundFilesController----");
            return Json<InboundDashBoardDisplay>(data);

        }
        [Route("GetInboundDashboardPopup")]
        [HttpPost]
        public JsonResult<InboundDashBoardDisplay> GetInboundDashboardPopup(string body)
        {
            this._log.Debug("---Executing GetInboundDashboardPopup() in InboundFilesController----");
            dynamic entity = Newtonsoft.Json.JsonConvert.DeserializeObject(body);
            string createdDate = HttpContext.Current.Request["createdDate"].ToString();
            var data = this._inboundFilesService.GetInboundDashboardPopup(Convert.ToInt32(entity), createdDate).Result;
            this._log.Debug("---Executed Successfully GetInboundDashboardPopup() in InboundFilesController----");
            return Json<InboundDashBoardDisplay>(data);

        }
        [Route("GetInboundFileCountByTime/{body}")]
        [HttpGet]
        public int GetInboundFileCountByTime(string body)
        {
            this._log.Debug("---Executing GetInboundFileCountByTime() in InboundFilesController----");
            var result = this._inboundFilesService.GetInboundFileCountByTime(body).Result;
            this._log.Debug("---Executed Successfully GetInboundFileCountByTime() in InboundFilesController----");
            return result;
        }
        [Route("GetInboundFoltChartData/{body}")]
        [HttpGet]
        public JsonResult<List<FlotChartEntity>> GetInboundFoltChartData(string body)
        {
            this._log.Debug("---Executing GetInboundFoltChartData() in InboundFilesController----");
            var result = this._inboundFilesService.GetInboundFoltChartData(body).Result;
            this._log.Debug("---Executed Successfully GetInboundFoltChartData() in InboundFilesController----");
            return Json<List<FlotChartEntity>>(result);
        }
        [Route("GetInboundFileErrorCountByTime/{body}")]
        [HttpGet]
        public int GetInboundFileErrorCountByTime(string body)
        {
            this._log.Debug("---Executing GetInboundFileErrorCountByTime() in InboundFilesController----");
            var result = this._inboundFilesService.GetInboundFileErrorCountByTime(body).Result;
            this._log.Debug("---Executed Successfully GetInboundFileErrorCountByTime() in InboundFilesController----");
            return result;
        }
        [Route("GetInboundImportFoltChartData/{body}")]
        [HttpGet]
        public JsonResult<List<FlotChartEntity>> GetInboundImportFoltChartData(string body)
        {
            this._log.Debug("---Executing GetInboundImportFoltChartData() in InboundFilesController----");
            var result = this._inboundFilesService.GetInboundImportFoltChartData(body).Result;
            this._log.Debug("---Executed Successfully GetInboundImportFoltChartData() in InboundFilesController----");
            return Json<List<FlotChartEntity>>(result);
        }
        [Route("ReceiveMessageService/{fteConfigId}")]
        [HttpGet]
        public string ReceiveMessageService(int fteConfigId)
        {
            this._log.Debug("---Executing ReceiveMessageService() in InboundFilesController----");
            var result = this._inboundFilesService.ReceiveMessageService(fteConfigId);
            this._log.Debug("---Executed Successfully ReceiveMessageService() in InboundFilesController----");
            return result;
        }
        [Route("GetInboundSuccessErrorCount/{companyId}")]
        [HttpGet]
        public JsonResult<List<InboundHeaderCountEntity>> GetInboundSuccessErrorCount(int companyId)
        {
            this._log.Debug("---Executing GetInboundSuccessErrorCount() in InboundFilesController----");
            var result = this._inboundFilesService.GetInboundSuccessErrorCount(companyId).Result;
            this._log.Debug("---Executed Successfully GetInboundSuccessErrorCount() in InboundFilesController----");
            return Json<List<InboundHeaderCountEntity>>(result);
        }
        //[Route("GetInboundFilesByStatus/{CompanyId}/{fileStatus}/{fromDate}/{toDate}/{patientName?}")]
        //[HttpGet]
        public JsonResult<List<FileInformationEntity>> GetInboundFilesByStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName = "")
        {
            this._log.Debug("---Executing GetInboundFilesByStatus() in InboundFilesController----");
            var data = this._inboundFilesService.GetInboundFilesByStatus(companyId, fileStatus, fromDate, toDate, patientName).Result;
            this._log.Debug("---Executed Successfully GetInboundFilesByStatus() in InboundFilesController----");
            return Json<List<FileInformationEntity>>(data);
        }
        [Route("HLSevenServiceStatus/{Port}")]
        [HttpGet]
        public string HLSevenServiceStatus(int Port)
        {
            this._log.Debug("---Executing HLSevenServiceStatus() in FTEConfigurationController----");
            string result = this._inboundFilesService.HLSevenServiceStatus(Port);
            this._log.Debug("---Executed Successfully HLSevenServiceStatus() in FTEConfigurationController----");
            return result;
        }
        [Route("GetFilesCount/{CompanyId}")]
        [HttpGet]
        public JsonResult<FilesCountEntity> GetFilesCount(int companyId)
        {
            this._log.Debug("---Executing GetFilesCount() in InboundFilesController----");
            var data = this._inboundFilesService.GetFilesCount(companyId).Result;
            this._log.Debug("---Executed Successfully GetFilesCount() in InboundFilesController----");
            return Json<FilesCountEntity>(data);
        }
        [Route("GetInboundResidentsDrop/{userId}/{CompanyId}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetInboundResidentsDrop(int userId,int companyId)
        {
            this._log.Debug("---Executing GetInboundResidentsDrop() in InboundFilesController----");
            var data = this._inboundFilesService.GetInboundResidentsDrop(userId,companyId).Result;
            this._log.Debug("---Executed Successfully GetInboundResidentsDrop() in InboundFilesController----");
            return Json<List<ResidentDropEntity>>(data);
        }
        [Route("GetInboundOutboundFilesByResident/{companyId}/{fileCategory}/{fromDate}/{toDate}/{currentPage}/{pageSize}/{status}/{searchValue?}/{patientName?}")]
        [HttpGet]
        public JsonResult<InboundFilesGridEntity> GetInboundOutboundFilesByResident(int companyId, string fileCategory, string fromDate, string toDate, int currentPage, int pageSize, int status, string searchValue="", string patientName="")
        {
            this._log.Debug("---Executing GetInbounddFilesByResident() in InboundFilesController----");
            var data = this._inboundFilesService.GetInboundFilesByResident(companyId, fileCategory, fromDate, toDate, currentPage, pageSize, searchValue, patientName, status).Result;
            this._log.Debug("---Executed Successfully GetInboundFilesByResident() in InboundFilesController----");
            return Json<InboundFilesGridEntity>(data);
        }
        //[Route("GetInboundRejectedFiles/{fromDate}/{toDate}/{currentPage}/{pageSize}")]
        //[HttpGet]
        //public JsonResult<List<FileInformationEntity>> GetInboundRejectedFiles(string fromDate, string toDate, int currentPage, int pageSize)
        //{
        //    this._log.Debug("---Executing GetInboundRejectedFiles() in InboundFilesController----");
        //    var data = this._inboundFilesService.GetInboundRejectedFiles(fromDate, toDate).Result;
        //    this._log.Debug("---Executed Successfully GetInboundRejectedFiles() in InboundFilesController----");
        //    return Json<List<FileInformationEntity>>(data);
        //}
        [Route("ResendInboundFile")]
        [HttpPost]
        public JsonResult<int> ResendInboundFile(ResendFileEntity entity)
        {
            this._log.Debug("---Executing ResendInboundFile() in InboundFilesController----");
            var result = this._inboundFilesService.ResendInboundFile(entity);
            this._log.Debug("---Executed Successfully ResendInboundFile() in InboundFilesController----");
            return Json<int>(result);
        }
    }
}
