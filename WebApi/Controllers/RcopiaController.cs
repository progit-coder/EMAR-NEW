using LTCPro.ServiceLayer;
using LTCPro.WebApi.Controllers;
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

namespace LTCPro.WebApi
{
   // [CustomApiExceptionFilter]
    [RoutePrefix("Rcopia")]
    //[CustomAuthorizationFilter]
    public class RcopiaController : ApiController
    {

        private readonly IRcopiaService _IRcopiaService;
        private readonly ILogger _log;
        private readonly CommonRcopiaController _CR;

        public RcopiaController(IRcopiaService IRcopiaService, ILogger log, CommonRcopiaController CR)
        {
            this._IRcopiaService = IRcopiaService;
            this._log = log;
            this._CR = CR;
        }


        [Route("PostDrFirstTest")]
        [HttpGet]
        public async Task<string> PostDrFirstTest()
        {
            try
            {

                HttpClient httpClient = new HttpClient();

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                this._log.Debug("---Executing GetBedById() in FacilityController----");
                var data = this._IRcopiaService.GetXMLData().Result;

                string Response = this._CR.PostDrFirstTest(data.XMLRequest,Convert.ToInt32(data.RequestID)).Result;

                return Response;


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

            //return null;
        }

        [Route("GetRedirectURL/{PatientId}/{LoginId}")]
        [HttpGet]
        public async Task<string> GetRedirectURL(string PatientId,string LoginId)
        {
            try
            {

                

                string Response = this._CR.RedirectURL();

                return Response;


            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

            //return null;
        }

        //RedirectURL()



    }
}
