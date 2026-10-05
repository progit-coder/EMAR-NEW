using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using LTCPro.Entities;
using System.Web;
using System.IO;
using LTCPro.ServiceLayer;
using Newtonsoft.Json;
using WebApi.Filters;
using System.Collections;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Mailbox")]
   	[CustomAuthorizationFilter]
    public class MailboxController : ApiController
    {
        private readonly IMailboxService _mailboxService;
        private readonly ILogger _log;
        public MailboxController(IMailboxService mailboxService, ILogger log)
        {
            this._mailboxService = mailboxService;
            this._log = log;
        }
        [Route("GetMailboxDetails/{userId}/{folderName}")]
        [HttpGet]
        public JsonResult<List<MailEntity>> GetMailboxDetails(int userId, string folderName)
        {
            this._log.Debug("---Executing GetMailboxDetails() in MailboxController----");
            var mailbox = this._mailboxService.GetMailboxDetails(userId, folderName).Result;
            this._log.Debug("---Executed successfully GetMailboxDetails() in MailboxController----");
            return Json<List<MailEntity>>(mailbox);
        }
        [Route("InsertComposeMail")]
        [HttpPost]
        public int InsertComposeMail(MailComposeEntity mailBodyObj)
        {
            this._log.Debug("---Executing InsertComposeMail() in MailboxController----");
            var mailbox = this._mailboxService.InsertComposeMail(mailBodyObj).Result;
            this._log.Debug("---Executed successfully InsertComposeMail() in MailboxController----");
            return mailbox;
        }
        [Route("InsertUpdateFavouritesMail")]
        [HttpPost]
        public int InsertUpdateFavouritesMail(MailFavouritesEntity mailFavObj)
        {
            this._log.Debug("---Executing InsertUpdateFavouritesMail() in MailboxController----");
            var fav = this._mailboxService.InsertUpdateFavouritesMail(mailFavObj).Result;
            this._log.Debug("---Executed successfully InsertUpdateFavouritesMail() in MailboxController----");
            return fav;
        }
        [Route("GetMailBodyDetails/{mailBoxId}/{userId}")]
        [HttpGet]
        public JsonResult<MailBodyEntity> GetMailBodyDetails(int mailBoxId, int userId)
        {
            this._log.Debug("---Executing GetMailBodyDetails() in MailboxController----");
            var mailbox = this._mailboxService.GetMailBodyDetails(mailBoxId,userId).Result;
            this._log.Debug("---Executed successfully GetMailBodyDetails() in MailboxController----");
            return Json<MailBodyEntity>(mailbox);
        }
        [Route("GetAlertsData/{userId}/{typeId}/{favstatus?}/{trashStatus?}")]
        [HttpGet]
        public JsonResult<List<AlertNotificationEntity>> GetAlertsData(int userId ,int typeId, int favstatus = 0,int trashStatus=0)
        {
            this._log.Debug("---Executing GetAlertsData() in MailboxController----");
            var data = this._mailboxService.GetAlertsData(userId, typeId, favstatus,trashStatus).Result;
            this._log.Debug("---Executed successfully GetAlertsData() in MailboxController----");
            return Json<List<AlertNotificationEntity>>(data);
        }

        [Route("GetToUsers/{userId}")]
        [HttpGet]
        public JsonResult<List<UserDropEntity>> GetToUsers(int userId)
        {
            this._log.Debug("---Executing GetToUsers() in MailboxController----");
            var data = this._mailboxService.GetToUsers(userId).Result;
            this._log.Debug("---Executed successfully GetToUsers() in MailboxController----");
            return Json<List<UserDropEntity>>(data);
        }
        [Route("TrashMailRecord")]
        [HttpPost]
        public JsonResult<int> TrashMailRecord(List<MailEntity> entity)
        {
            this._log.Debug("---Executing TrashMailRecord() in MailboxController----");
            var data = this._mailboxService.TrashMailRecord(entity).Result;
            this._log.Debug("---Executed successfully TrashMailRecord() in MailboxController----");
            return Json<int>(data);
        }
        [Route("TrashReadMailRecord")]
        [HttpPost]
        public JsonResult<int> TrashReadMailRecord(MailBodyEntity entity)
        {
            this._log.Debug("---Executing TrashReadMailRecord() in MailboxController----");
            var data = this._mailboxService.TrashReadMailRecord(entity).Result;
            this._log.Debug("---Executed successfully TrashReadMailRecord() in MailboxController----");
            return Json<int>(data);
        }
        [Route("GetAlertsDetailsCount/{userId}")]
        [HttpGet]
        public IHttpActionResult GetAlertsDetailsCount(int userId)
        {
            this._log.Debug("---Executing GetAlertsDetails() in MailboxController----");
            var data = this._mailboxService.GetAlertsDetails(userId).Result;
            this._log.Debug("---Executed successfully GetAlertsDetails() in MailboxController----");
            return Json<IList>(data);
        }
        [Route("InsertAlertFavStatus")]
        [HttpPost]
        public int InsertAlertFavStatus(AlertStatusEntity entityObj)
        {
            this._log.Debug("---Executing InsertAlertFavStatus() in MailboxController----");
            var data = this._mailboxService.InsertAlertFavStatus(entityObj).Result;
            this._log.Debug("---Executed successfully InsertAlertFavStatus() in MailboxController----");
            return data;
        }
        [Route("TrashAlertRecord")]
        [HttpPost]
        public int TrashAlertRecord(List<AlertNotificationEntity> alerts)
        {
            this._log.Debug("---Executing TrashAlertRecord() in MailboxController----");
            var data = this._mailboxService.TrashAlertRecord(alerts).Result;
            this._log.Debug("---Executed successfully TrashAlertRecord() in MailboxController----");
            return data;
        }
        [Route("InsertReadMail/{mailboxId}/{userId}")]
        [HttpGet]
        public int InsertReadMail(int mailboxId, int userId)
        {
            this._log.Debug("---Executing InsertReadMail() in MailboxController----");
            var data = this._mailboxService.InsertReadMail(mailboxId,userId).Result;
            this._log.Debug("---Executed successfully InsertReadMail() in MailboxController----");
            return data;
        }
        [Route("GetReadMailsCount/{userId}")]
        [HttpGet]
        public int GetReadMailsCount(int userId)
        {
            this._log.Debug("---Executing GetReadMailsCount() in MailboxController----");
            var data = this._mailboxService.GetReadMailsCount( userId).Result;
            this._log.Debug("---Executed successfully GetReadMailsCount() in MailboxController----");
            return data;
        }
        [Route("GetAlertsResDrop/{userId}/{companyId}")]
        [HttpGet]
        public List<ResidentDropEntity> GetAlertsResDrop(int userId, int companyId)
        {
            this._log.Debug("---Executing GetAlertsResDrop() in MailboxController----");
            var data = this._mailboxService.GetAlertsResDrop(userId, companyId).Result;
            this._log.Debug("---Executed successfully GetAlertsResDrop() in MailboxController----");
            return data;
        }
        [Route("GetAllAlertsData")]
        [HttpPost]
        public List<AlertNotificationEntity> GetAllAlertsData(AlertsCustomEntity entity)
        {
            this._log.Debug("---Executing GetAllAlertsData() in MailboxController----");
            var data = this._mailboxService.GetAllAlertsData(entity).Result;
            this._log.Debug("---Executed successfully GetAllAlertsData() in MailboxController----");
            return data;
        }
        [Route("InsertReadAlert/{alertTextId}/{userId}")]
        [HttpGet]
        public int InsertReadAlert(long alertTextId, int userId)
        {
            this._log.Debug("---Executing InsertReadAlert() in MailboxController----");
            var data = this._mailboxService.InsertReadAlert(alertTextId, userId).Result;
            this._log.Debug("---Executed successfully InsertReadAlert() in MailboxController----");
            return data;
        }
        
    }
   
    
}