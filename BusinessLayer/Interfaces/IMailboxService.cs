using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.ServiceLayer
{
    public interface IMailboxService
    {
        Task<List<MailEntity>> GetMailboxDetails(int userId, string folderName);
        Task<int> InsertComposeMail(MailComposeEntity mailBodyObj);
        Task<int> InsertUpdateFavouritesMail(MailFavouritesEntity mailFavObj);
        Task<MailBodyEntity> GetMailBodyDetails(int mailBoxId, int userId);
        Task<List<AlertNotificationEntity>> GetAlertsData(int userId, int typeId, int favstatus, int trashStatus);
        Task<List<UserDropEntity>> GetToUsers(int userId);
        Task<int> TrashMailRecord(List<MailEntity> entity);
        Task<int> TrashReadMailRecord(MailBodyEntity entity);
        Task<IList> GetAlertsDetails(int userId);
        Task<int> InsertAlertFavStatus(AlertStatusEntity entityObj);
        Task<int> TrashAlertRecord(List<AlertNotificationEntity> alerts);
        Task<int> InsertReadMail(int mailboxId, int userId);
        Task<int> GetReadMailsCount(int userId);
        Task<List<ResidentDropEntity>> GetAlertsResDrop(int userId, int companyId);
        Task<List<AlertNotificationEntity>> GetAllAlertsData(AlertsCustomEntity entity);
        Task<int> InsertReadAlert(long alertTextId,int userId);

    }
}
