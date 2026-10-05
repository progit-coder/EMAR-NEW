using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using System.Web.Http.Results;
using LTCPro.Entities;
using Newtonsoft.Json;
using System.Web;
using LTCPro.Repositories;
using WebApi.Filters;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [CustomAuthorizationFilter]
    [RoutePrefix("DocumentManager")]
    public class DocumentManagerController : ApiController
    {
        private readonly IDocumentManagerService _documentManagerService;
        private readonly IDocumentManagerRepository _documentRepositiory;
        private readonly ILogger _log;
        public DocumentManagerController(IDocumentManagerService documentManagerService, ILogger log, IDocumentManagerRepository documentManagerRepositiory)
        {
            this._documentManagerService = documentManagerService;
            this._documentRepositiory = documentManagerRepositiory;
            this._log = log;
        }

        [Route("UploadResidentDocument")]
        [HttpPost]
        public int UploadResidentDocument(string body)
        {
            this._log.Debug("---Executing UploadResidentDocument() in DocumentManagerController----");
            UploadedDocumentEntity record = JsonConvert.DeserializeObject<UploadedDocumentEntity>(body);
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];

                //var filelocation = this._documentRepositiory.GetFolderLocation((int)record.FolderID);
                //var patientname = this._documentRepositiory.GetPatientName((int)record.Patient_Id);
                //string filename = patientname + "_" + record.DocName;
                //var postedFile = httpRequest.Files[0];
                //var filePath = ConfigurationManager.AppSettings.GetValues("DocumentManger")[0].ToString() + filelocation + "/" + patientname + "_" + record.DocName;
                //postedFile.SaveAs(filePath);
                //UploadedDocumentEntity entity = new UploadedDocumentEntity();
                //entity.PatientDoc_Id = record.PatientDoc_Id;
                //entity.Patient_Id = record.Patient_Id;
                //entity.DocName = patientname + "_" + record.DocName;
                //entity.DocType = record.DocType;
                //entity.DocLocation = filelocation;
                //entity.DocDescription = record.DocDescription;
                //entity.FolderID = record.FolderID;
                //entity.UDocuments_CreatedBy = record.UDocuments_CreatedBy;
                //entity.UDocuments_CreatedDate = record.UDocuments_CreatedDate;

                var uploaded = this._documentManagerService.UploadResidentDocument(record, postedFile).Result;
                this._log.Debug("---Executed Successfully UploadResidentDocument() in DocumentManagerController----");
                return uploaded;
            }
            else
                return 0;
        }
        [Route("GetUploadedDocsByfolderid/{folderid}")]
        [HttpGet]
        public JsonResult<List<UploadedDocumentEntity>> GetUploadedDocsByfolderid(int folderid)
        {
            this._log.Debug("---Executing GetUploadedDocsByfolderid() in DocumentManagerController----");
            var documments = this._documentManagerService.GetUploadedDocsByfolderid(folderid).Result;
            this._log.Debug("---Executed Successfully GetUploadedDocsByfolderid() in DocumentManagerController----");
            return Json<List<UploadedDocumentEntity>>(documments);
        }

     
        [Route("GetDocFolders")]
        [HttpGet]
        public JsonResult<List<DocFloderCustomEntity>> GetDocFolders()
        {
            this._log.Debug("---Executing GetDocFolders() in DocumentManagerController----");
            var result = this._documentManagerService.GetDocFolders().Result;
            this._log.Debug("---Executed Successfully GetDocFolders() in DocumentManagerController----");
            return Json<List<DocFloderCustomEntity>>(result);
        }
        [Route("GetUploadedDocuments/{patientId}/{folderIds}")]
        [HttpGet]
        public JsonResult<List<UploadedDocumentEntity>> GetUploadedDocuments(int patientId, string folderIds)
        {
            this._log.Debug("---Executing GetUploadedDocuments() in DocumentManagerController----");
            var result = this._documentManagerService.GetUploadedDocuments(patientId, folderIds).Result;
            this._log.Debug("---Executed Successfully GetUploadedDocuments() in DocumentManagerController----");
            return Json<List<UploadedDocumentEntity>>(result);
        }
        [Route("DocumentsDownload/{patientdocId}")]
        [HttpGet]
        public IHttpActionResult DocumentsDownload(int patientdocId)
        {
            this._log.Debug("---Executing DocumentsDownload() in DocumentManagerController----");
            var result = this._documentManagerService.DocumentsDownload(patientdocId).Result;

            var filePath = ConfigurationManager.AppSettings.GetValues("DocumentManger")[0].ToString() + result.DocLocation + "/" + result.DocName;
            if (filePath != null && File.Exists(filePath))
            {
                byte[] content = File.ReadAllBytes(filePath);
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(content)
                };

                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = result.DocName
                };
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(result.DocType);
                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                this._log.Debug("---Executed Successfully DocumentsDownload() in DocumentManagerController----");
                return responseMessageResult;
            }
            return null;
            

        }
        
        [Route("DeleteDocument/{patientdocId}")]
        [HttpPost]
        public JsonResult<int> DeleteDocument(int patientdocId)
        {

            this._log.Debug("---Executing DeleteDocument() in DocumentManagerController----");
            var result = this._documentManagerService.DeleteDocument(patientdocId).Result;
            this._log.Debug("---Executed Successfully DeleteDocument() in DocumentManagerController----");
            return Json<int>(result);
        }
        [Route("GetFolderNames")]
        [HttpGet]
        public JsonResult<List<DocFolderEntity>> GetFolderNames()
        {
            this._log.Debug("---Executing GetFolderNames() in DocumentManagerController----");
            var result = this._documentManagerService.GetFolderNames().Result;
            this._log.Debug("---Executed Successfully GetFolderNames() in DocumentManagerController----");
            return Json<List<DocFolderEntity>>(result);
        }
        [Route("InsertFolders")]
        [HttpPost]
        public JsonResult<int> InsertFolders(DocFolderEntity entity)
        {
            this._log.Debug("---Executing InsertFolders() in DocumentManagerController----");
            var result = this._documentManagerService.InsertFolders(entity).Result;
            this._log.Debug("---Executed Successfully InsertFolders() in DocumentManagerController----");
            return Json<int>(result); 
        }
    }
}