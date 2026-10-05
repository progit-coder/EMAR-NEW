using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace WebApi.Filters
{
    public class CustomApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {

            var exceptionMessage = string.Empty;
            var innerExceptionMessage = string.Empty;
            HttpResponseMessage message = null;
            if (filterContext.Exception.InnerException != null)
                innerExceptionMessage = filterContext.Exception.InnerException.Message;
            exceptionMessage = filterContext.Exception.Message;
            //Logging Exception UnComment when deploying in live
            WriteLog(filterContext);
            if (filterContext.Exception is UnauthorizedAccessException)
            {
                message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(exceptionMessage + innerExceptionMessage),
                    ReasonPhrase = exceptionMessage + innerExceptionMessage

                };
            }
            else if (filterContext.Exception is NullReferenceException)
            {
                message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(exceptionMessage + innerExceptionMessage),
                    ReasonPhrase = exceptionMessage + innerExceptionMessage

                };
            }
            else
            {
                exceptionMessage = "An unhandled exception was thrown.Please try again...";
                message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(exceptionMessage + innerExceptionMessage),
                    ReasonPhrase = exceptionMessage + innerExceptionMessage

                };
            }


            throw new HttpResponseException(message);
        }
        private void WriteLog(HttpActionExecutedContext filterContext)
        {


            try
            {
                string fromMail = WebConfigurationManager.AppSettings["EmarFromMail"];
                string toMail = WebConfigurationManager.AppSettings["EmarTeamMail"];


                var requestScope = filterContext.Request.GetDependencyScope();
                var _log = requestScope.GetService(typeof(ILogger)) as ILogger;
                _log.Error("===================================");

                _log.Error(" Source : = " + filterContext.Exception.Source);
                _log.Error(" Stack Trace : = " + filterContext.Exception.StackTrace);
                _log.Error(" Exception Message : = " + filterContext.Exception.Message);
                _log.Error("============= Inner Exception  Stack Trace  ===========");
                _log.Error(" Inner Exception  Stack Trace : = " + filterContext.Exception.StackTrace);
                _log.Error("=============  Exception  Stack Trace End  ===========");

                if (filterContext.Exception.InnerException != null)
                {
                    _log.Error("============= Inner Exception  Message  ===========");
                    _log.Error(" Inner Exception Message : = " + filterContext.Exception.InnerException.Message);
                    _log.Error("============= Inner Exception  Message End  ===========");
                }

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromMail);

                string[] ToList = toMail.Split(',');

                foreach (var a in ToList)
                {
                    mail.To.Add(a);
                }

                //using (MailMessage mail = new MailMessage(fromMail, toMail))
                //  {
                string Body = "";
                    if (filterContext.Exception.InnerException != null)
                    {
                        string mes = "";
                        if (filterContext.Exception.Message == null)
                            mes = "";
                        else
                            mes = filterContext.Exception.Message.ToString();

                        mail.Subject = "Emar Application Issues :  " + mes + " , " + filterContext.Exception.InnerException.Message;
                        Body = " Message : " + filterContext.Request;
                    }
                    else
                    {
                        mail.Subject = "Emar Application Issues :  " + filterContext.Exception.Message;
                        Body = " Message : " + filterContext.Request;
                    }
                    mail.Body = Body;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp1-mke.securence.com"; //Or Your SMTP Server Address


                    smtp.EnableSsl = false;
                    smtp.Credentials = new System.Net.NetworkCredential("", "");
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = 587;
                    smtp.Send(mail);
                    if (filterContext.Exception.InnerException != null)
                        _log.Error(" InnerException Message : = " + filterContext.Exception.InnerException.Message);
                    else
                        _log.Error(" InnerException Message : = " + filterContext.Exception.Message);
                    _log.Error("===================================");
              //  }
            }
            catch(Exception ex)
            {

            }



        }
      
        }
    }

