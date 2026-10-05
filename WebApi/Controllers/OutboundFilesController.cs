using LTCPro.Entities;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Outbound")]
    //CustomAuthorizationFilter]
    public class OutboundFilesController : ApiController
    {
        private readonly IOutboundFilesService _outboundFilesService;
        private readonly ILogger _log;

        public OutboundFilesController(IOutboundFilesService outboundFilesService, ILogger log)
        {
            this._outboundFilesService = outboundFilesService;
            this._log = log;
        }
        //[Route("GetOutboundList")]
        //[HttpGet]
        //public JsonResult<List<OutboundRecordsCustomEntity>> GetOutBoundList()
        //{
        //    this._log.Debug("---Executing GetOutBoundList() in OutboundFilesController----");
        //    var data = this._outboundFilesService.GetOutBoundList().Result;
        //    this._log.Debug("---Executed Successfully GetOutBoundList() in OutboundFilesController----");
        //    return Json<List<OutboundRecordsCustomEntity>>(data);
        //}
        //[Route("GenerateOutboundFile")]
        //[HttpPost]
        //public int GenerateFileSaveData(List<OutboundRecordsCustomEntity> records)
        //{
        //    try
        //    {
        //        this._log.Debug("---Executing GenerateFileSaveData() in OutboundFilesController----");
        //        var data = this._outboundFilesService.GenerateFileSaveData(records).Result;
        //        this._log.Debug("---Executed Successfully GenerateFileSaveData() in OutboundFilesController----");
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        [Route("GetFileAckDataForOutbound/{FileId}")]
        [HttpGet]
        public JsonResult<FileInformationViewCustomEntity> GetFileAckDataForOutbound(int FileId)
        {
            this._log.Debug("---Excecuting GetFileAckDataForOutbound() in OutboundFilesController----");
            var data = this._outboundFilesService.GetFileAckDataForOutbound(FileId).Result;
            this._log.Debug("---Executed successfully GetFileAckDataForOutbound() in OutboundFilesController----");
            return Json<FileInformationViewCustomEntity>(data);
        }
       [Route("GetOutboundFilesList/{CompanyId}/{FileCategory}/{fromDate}/{toDate}/{patientName?}")]
        [HttpGet]
        public JsonResult<List<OutBoundFileInformationEntity>> GetOutboundFilesList(int companyId, string fileCategory, string fromDate, string toDate, string patientName = "")
        {
            this._log.Debug("---Executing GetOutboundFilesList() in OutboundFilesController----");
            var data = this._outboundFilesService.GetOutboundFiles(companyId, fileCategory,fromDate, toDate, patientName).Result;
            this._log.Debug("---Executed Successfully GetOutboundFilesList() in OutboundFilesController----");
            return Json<List<OutBoundFileInformationEntity>>(data);
        }
        [Route("DownloadOutboundFile/{FileId}")]
        [HttpGet]
        public IHttpActionResult WriteOutboundDataToFile(int fileId)
        {
            this._log.Debug("---Executing WriteOutboundDataToFile() in OutboundFilesController----");
            var record = this._outboundFilesService.GetOutboundFileById(fileId).Result;
            StreamWriter log;
            byte[] content = null;
            MemoryStream ms = new MemoryStream();
            using (log = new StreamWriter(ms))
            {
                log.WriteLine(record.File_Data);
            }
            content = ms.ToArray();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(content)
            };

            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = record.File_Name + DateTime.Now.ToString("yyyy/MM/dd")
            };
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
            this._log.Debug("---Executed Successfully WriteOutboundDataToFile() in OutboundFilesController----");
            return responseMessageResult;
        }
        [Route("GetOutboundFilesStatus/{CompanyId}/{fileStatus}/{fromDate}/{toDate}/{patientName?}")]
        [HttpGet]
        public JsonResult<List<OutBoundFileInformationEntity>> GetOutboundFilesStatus(int companyId, int fileStatus, string fromDate, string toDate, string patientName = "")
        {
            this._log.Debug("---Executing GetOutboundFilesStatus() in OutboundFilesController----");
            var data = this._outboundFilesService.GetOutboundFilesStatus(companyId, fileStatus, fromDate, toDate, patientName).Result;
            this._log.Debug("---Executed Successfully GetOutboundFilesStatus() in OutboundFilesController----");
            return Json<List<OutBoundFileInformationEntity>>(data);
        }
        [Route("GetOutboundResidentsDrop/{userId}/{CompanyId}")]
        [HttpGet]
        public JsonResult<List<ResidentDropEntity>> GetOutboundResidentsDrop(int userId, int companyId)
        {
            this._log.Debug("---Executing GetOutboundResidentsDrop() in InboundFilesController----");
            var data = this._outboundFilesService.GetOutboundResidentsDrop(userId, companyId).Result;
            this._log.Debug("---Executed Successfully GetOutboundResidentsDrop() in InboundFilesController----");
            return Json<List<ResidentDropEntity>>(data);
        }
    }
}
