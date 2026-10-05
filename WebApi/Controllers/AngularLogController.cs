using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using LTCPro.Entities;
using WebApi.Filters;
using System.Web.Http.Results;
using System.Net.Mail;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("AngularLog")]
   [CustomAuthorizationFilter]
    public class AngularLogController : ApiController
    {
        private readonly IAngularLog _angularLogService;
        public AngularLogController(IAngularLog angularLogService)
        {
            this._angularLogService = angularLogService;            
        }
        [Route("LogError/{errorMessage}")]
        [HttpGet]
        public JsonResult<int> LogError(string errorMessage)
        {
            int result = this._angularLogService.ErrorLogging(errorMessage);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("emarsupport@pharmalife.com");
            mail.To.Add("chrandheer@promantra.us");
           //  mail.CC.Add("jagadeeshk@revvpro.com");
            mail.Bcc.Add("tajuddeenh@promantra.us");
            mail.Subject = "Angular Error";
            string Body = "Message :" + errorMessage;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp1-mke.securence.com"; //Or Your SMTP Server Address
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            ("", "");
            smtp.EnableSsl = false;
            //smtp.Send(mail);
            return Json<int>(result);
        }
    }
}