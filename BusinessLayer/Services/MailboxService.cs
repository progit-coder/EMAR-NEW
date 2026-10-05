using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class MailboxService : IMailboxService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ILogger _log;
        private MailboxRepository _mailboxRepository;
        public MailboxService(IAutoMapper autoMapper, MailboxRepository mailboxRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._mailboxRepository = mailboxRepository;
            this._log = log;
        }

        public async Task<List<AlertNotificationEntity>> GetAlertsData(int userId, int typeId, int favstatus, int trashStatus)
        {
            this._log.Debug("---Executing GetAlertsData() in MailboxService----");
            return await Task.FromResult<List<AlertNotificationEntity>>(this._mailboxRepository.GetAlertsData(userId, typeId, favstatus,trashStatus));
        }

        public async Task<IList> GetAlertsDetails(int userId)
        {
            this._log.Debug("---Executing GetAlertsDetails() in MailboxService----");
            return await Task.FromResult<IList>(this._mailboxRepository.GetAlertsDetails(userId));
        }

        public async Task<MailBodyEntity> GetMailBodyDetails(int mailBoxId, int userId)
        {
            this._log.Debug("---Executing GetMailBodyDetails() in MailboxService----");
            return await Task.FromResult<MailBodyEntity>(this._mailboxRepository.GetMailBodyDetails(mailBoxId, userId));
        }

        public async Task<List<MailEntity>> GetMailboxDetails(int userId, string folderName)
        {
            this._log.Debug("---Executing GetMailboxDetails() in MailboxService----");
            return await Task.FromResult<List<MailEntity>>(this._mailboxRepository.GetMailboxDetails(userId, folderName));
        }

        public async Task<List<UserDropEntity>> GetToUsers(int userId)
        {
            this._log.Debug("---Executing GetToUsers() in MailboxService----");
            return await Task.FromResult<List<UserDropEntity>>(this._mailboxRepository.GetToUsers(userId));
        }

        public async Task<int> InsertAlertFavStatus(AlertStatusEntity entityObj)
        {
            this._log.Debug("---Executing InsertAlertFavStatus() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.InsertAlertFavStatus(entityObj));
        }

        public async Task<int> InsertComposeMail(MailComposeEntity mailBodyObj)
        {
            this._log.Debug("---Executing InsertComposeMail() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.InsertComposeMail(mailBodyObj));
        }

        public async Task<int> InsertUpdateFavouritesMail(MailFavouritesEntity mailFavObj)
        {
            this._log.Debug("---Executing InsertUpdateFavouritesMail() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.InsertUpdateFavouritesMail(mailFavObj));
        }
        public async Task<int> TrashMailRecord(List<MailEntity> entity)
        {
            this._log.Debug("---Executing TrashMailRecord() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.TrashMailRecord(entity));
        }
        public async Task<int> TrashReadMailRecord(MailBodyEntity entity)
        {
            this._log.Debug("---Executing TrashReadMailRecord() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.TrashReadMailRecord(entity));
        }
        public async Task<int> TrashAlertRecord(List<AlertNotificationEntity> alerts)
        {
            this._log.Debug("---Executing TrashAlertRecord() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.TrashAlertRecord(alerts));
        }
        public async Task<int> InsertReadMail(int mailboxId, int userId)
        {
            this._log.Debug("---Executing InsertReadMail() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.InsertReadMail(mailboxId, userId));
        }
        public async Task<int> GetReadMailsCount(int userId)
        {
            this._log.Debug("---Executing GetReadMailsCount() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.GetReadMailsCount(userId));
        }
        public async Task<List<ResidentDropEntity>> GetAlertsResDrop(int userId, int companyId)
        {
            this._log.Debug("---Executing GetAlertsResDrop() in MailboxService----");
            return await Task.FromResult<List<ResidentDropEntity>>(this._mailboxRepository.GetAlertsResDrop(userId, companyId));
        }
        public async Task<List<AlertNotificationEntity>> GetAllAlertsData(AlertsCustomEntity entity)
        {
            this._log.Debug("---Executing GetAllAlertsData() in MailboxService----");
            return await Task.FromResult<List<AlertNotificationEntity>>(this._mailboxRepository.GetAllAlertsData(entity));
        }
        public async Task<int> InsertReadAlert(long alertTextId, int userId)
        {
            this._log.Debug("---Executing InsertReadAlert() in MailboxService----");
            return await Task.FromResult<int>(this._mailboxRepository.InsertReadAlert(alertTextId, userId));
        }
    }
}
