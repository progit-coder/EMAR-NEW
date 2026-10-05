using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using LTCPro.Entities;
using System.Web.Http.Results;
using WebApi.Filters;
using System.IO;
using System.Web;
using System.Xml;
using LTCPro.DAL;
using System.Text;
using System.Net.Http.Headers;
using System.Configuration;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("DrFirstIntegration")]
   [CustomAuthorizationFilter]
    public class DrFirstIntegrationController : ApiController
    {
        private readonly IDrFirstIntegrationService _drFirstIntegrationService;
        private readonly ILogger _log;
        public DrFirstIntegrationController(IDrFirstIntegrationService drFirstIntegrationService, ILogger log)
        {
            this._drFirstIntegrationService = drFirstIntegrationService;
            this._log = log;
        }
        [Route("InsertUpdateApiIntegration")]
        [HttpPost]
        public int InsertUpdateApiIntegration(ApiIntegrationEntity integrationdata)
        {
            this._log.Debug("---Executing Insert/Update() in DrFirstIntegrationController----");
            int result = this._drFirstIntegrationService.InsertUpdateApiIntegration(integrationdata).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in DrFirstIntegrationController----");
            return result;
        }
        [Route("GetApiIntegrationAllData/{companyID}")]
        [HttpGet]
        public JsonResult<List<ApiIntegrationEntity>> GetApiIntegrationAllData(int companyID)
        {
            this._log.Debug("---Executing GetApiIntegrationAllData() in DrFirstIntegrationController----");
            var integrationdata = this._drFirstIntegrationService.GetApiIntegrationAllData(companyID).Result;
            this._log.Debug("---Executed Successfully GetApiIntegrationAllData() in DrFirstIntegrationController----");
            return Json<List<ApiIntegrationEntity>>(integrationdata);
        }
        [Route("GetApiIntegrationDetails/{integrationID}")]
        [HttpGet]
        public JsonResult<ApiIntegrationEntity> GetApiIntegrationDetails(int integrationID)
        {
            this._log.Debug("---Executing GetApiIntegrationDetails() in DrFirstIntegrationController----");
            var integrationdata = this._drFirstIntegrationService.GetApiIntegrationDetails(integrationID).Result;
            this._log.Debug("---Executed Successfully GetApiIntegrationDetails() in DrFirstIntegrationController----");
            return Json<ApiIntegrationEntity>(integrationdata);
        }
        [Route("GetDrFirst")]
        [HttpGet]
        public string GetDrFirst()
        {
            this._log.Debug("---Executing GetDrFirst() in DrFirstIntegrationController----");

            string xmlData = @"<RCExtRequest version = '4'>
	<Caller>
		<VendorName>pavendor2991</VendorName>
		<VendorPassword>zrpxtgch</VendorPassword>
	</Caller>
	<SystemName>pavendor2991</SystemName>
	<RcopiaPracticeUsername>ph98003</RcopiaPracticeUsername>
	<Request>
		<Command>update_prescription</Command>
		<LastUpdateDate>12/01/2016 00:00:00</LastUpdateDate>
		<Patient>
			<RcopiaID></RcopiaID>
			<ExternalID>PALTROWBRUCE</ExternalID>
		</Patient>
		<Status>all</Status>
	</Request>
</RCExtRequest>";

            string url = "https://update201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes("xml="+xmlData);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "text/xml";
            req.ContentLength = data.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string returnXML = sr.ReadToEnd();
            var result = this._drFirstIntegrationService.DrFirstXMLSave(returnXML).Result;
            this._log.Debug("---Executed Successfully GetDrFirst() in DrFirstIntegrationController----");
            return result;
        }
        public static string XmlHttpRequest(string urlString, string xmlContent)
        {

            string response = null;
            HttpWebRequest httpWebRequest = null;//Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse httpWebResponse = null;//Declare an HTTP-specific implementation of the WebResponse class

            //Creates an HttpWebRequest for the specified URL.
            httpWebRequest = (HttpWebRequest)WebRequest.Create(urlString);

            try
            {
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlContent);
                //Set HttpWebRequest properties
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    //Writes a sequence of bytes to the current stream 
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();//Close stream
                }

                //Sends the HttpWebRequest, and waits for a response.
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream into StreamReader
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                            response = reader.ReadToEnd();
                    }
                }
                httpWebResponse.Close();//Close HttpWebResponse
            }
            catch (WebException we)
            {   //TODO: Add custom exception handling
                throw new Exception(we.Message);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                httpWebResponse.Close();
                //Release objects
                httpWebResponse = null;
                httpWebRequest = null;
            }
            return response;
        }
        [Route("GetDrFirstOrderInfo/{orderId}")]
        [HttpGet]
        public JsonResult<List<DrFirstOrderCheck_ResultEntity>> GetDrFirstOrderInfo(int orderId)
        {
            this._log.Debug("---Executing GetApiIntegrationDetails() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.GetDrFirstOrderInfo(orderId).Result;
            this._log.Debug("---Executed Successfully GetApiIntegrationDetails() in DrFirstIntegrationController----");

            return Json<List<DrFirstOrderCheck_ResultEntity>>(record);
        }
        [Route("UpdateApprovalStatus/{PrescriptionID}/{Username}/{Password}")]
        [HttpGet]
        public int UpdateApprovalStatus(int PrescriptionID, string Username, string Password)
        {
            this._log.Debug("---Executing Update() in DrFirstIntegrationController----");
            int result = this._drFirstIntegrationService.UpdateApprovalStatus(PrescriptionID, Username, Password).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in DrFirstIntegrationController----");
            return result;
        }
        [Route("GetDrFirstOrderCheck/{patientId}")]
        [HttpGet]
        public JsonResult<List<DrFirstOrderCheck_ResultEntity>> GetDrFirstOrderCheck(int patientId)
        {
            this._log.Debug("---Executing GetDrFirstOrderCheck() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.GetDrFirstOrderCheck(patientId).Result;
            this._log.Debug("---Executed Successfully GetDrFirstOrderCheck() in DrFirstIntegrationController----");

            return Json<List<DrFirstOrderCheck_ResultEntity>>(record);
        }
        [Route("GetHL7DrFirstOrderCheck/{patientId}")]
        [HttpGet]
        public JsonResult<List<PrcDrFirstOrderHL7_ResultEntity>> GetHL7DrFirstOrderCheck(int patientId)
        {
            this._log.Debug("---Executing GetHL7DrFirstOrderCheck() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.GetHL7DrFirstOrderCheck(patientId).Result;
            this._log.Debug("---Executed Successfully GetHL7DrFirstOrderCheck() in DrFirstIntegrationController----");

            return Json<List<PrcDrFirstOrderHL7_ResultEntity>>(record);
        }
        [Route("InsertDrFirstMap")]
        [HttpPost]
        public int InsertDrFirstMap(List<DrFirstEntity> entity)
        {
            this._log.Debug("---Executing InsertDrFirstMap() in DrFirstIntegrationController----");
            int result = this._drFirstIntegrationService.InsertDrFirstMap(entity).Result;
            this._log.Debug("---Executed Successfully InsertDrFirstMap() in DrFirstIntegrationController----");
            return result;
        }
        [Route("GetNurseComments")]
        [HttpGet]
        public JsonResult<List<NurseCommentTypeDropEntity>> GetNurseComments()
        {
            this._log.Debug("---Executing GetNurseComments() in DrFirstIntegrationController----");
            var nurseComments = this._drFirstIntegrationService.GetNurseComments().Result;
            this._log.Debug("---Executed Successfully GetNurseComments() in DrFirstIntegrationController----");
            return Json<List<NurseCommentTypeDropEntity>>(nurseComments);
        }

        [Route("GetNoteComments")]
        [HttpGet]
        public JsonResult<List<MedicationReasonEntity>> GetNoteComments()
        {
            this._log.Debug("---Executing GetNurseComments() in DrFirstIntegrationController----");
            var nurseComments = this._drFirstIntegrationService.GetNoteComments().Result;
            this._log.Debug("---Executed Successfully GetNurseComments() in DrFirstIntegrationController----");
            return Json<List<MedicationReasonEntity>>(nurseComments);
        }

        [Route("PrcGetReportDrFirst/{patientId}")]
        [HttpGet]
        public JsonResult<List<PrcGetReportDrFirst_ResultEntity>> PrcGetReportDrFirst(int patientId)
        {
            this._log.Debug("---Executing PrcGetReportDrFirst() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.PrcGetReportDrFirst(patientId).Result;
            this._log.Debug("---Executed Successfully PrcGetReportDrFirst() in DrFirstIntegrationController----");

            return Json<List<PrcGetReportDrFirst_ResultEntity>>(record);
        }
        [Route("GetDrFirstResidentDrop/{userId}")]
        [HttpGet]
        public JsonResult<List<DemographicResidentDropEnity>> GetDrFirstResidentDrop(int userId)
        {
            this._log.Debug("---Executing GetDrFirstResidentDrop() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.GetDrFirstResidentDrop(userId).Result;
            this._log.Debug("---Executed Successfully GetDrFirstResidentDrop() in DrFirstIntegrationController----");
            return Json<List<DemographicResidentDropEnity>>(record);
        }
        [Route("InsertHlSevenApprove")]
        [HttpPost]
        public JsonResult<int> InsertHlSevenApprove(HlSevenApproveEntity entity)
        {
            this._log.Debug("---Executing InsertHlSevenApprove() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.InsertHlSevenApprove(entity).Result;
            this._log.Debug("---Executed Successfully InsertHlSevenApprove() in DrFirstIntegrationController----");
            return Json<int>(record);
        }
        [Route("InsertDrFirstApprove/{usetId}/{DrFirstOrderId}")]
        [HttpGet]
        public JsonResult<int> InsertDrFirstApprove(int usetId, int DrFirstOrderId)
        {
            this._log.Debug("---Executing InsertDrFirstApprove() in DrFirstIntegrationController----");
            var record = this._drFirstIntegrationService.InsertDrFirstApprove(usetId, DrFirstOrderId).Result;
            this._log.Debug("---Executed Successfully InsertDrFirstApprove() in DrFirstIntegrationController----");
            return Json<int>(record);
        }
        [Route("DownloadXML/{fileId}")]
        [HttpGet]
        public IHttpActionResult DownloadXML(int fileId)
        {
            this._log.Debug("---Executing DownloadXML() in DrFirstIntegrationController----");
            var filePath = this._drFirstIntegrationService.DownloadXML(fileId).Result;

            if (filePath != null && File.Exists(filePath)) {
                byte[] content = File.ReadAllBytes(filePath);
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new ByteArrayContent(content)
                };

                httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
                    FileName = "DrFistPrescription" + fileId
                };
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
                ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);
                this._log.Debug("---Executed Successfully DownloadXML() in DrFirstIntegrationController----");
                return responseMessageResult;
            }
            return null;


        }
    }
}