using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using System.Collections;

namespace LTCPro.Repositories
{
    public interface IMailboxRepository
    {
        List<MailEntity> GetMailboxDetails(int userId,string folderName);
        int InsertComposeMail(MailComposeEntity mailBodyObj);
        int InsertUpdateFavouritesMail(MailFavouritesEntity mailFavObj);
        MailBodyEntity GetMailBodyDetails(int mailBoxId,int userId);
        List<AlertNotificationEntity> GetAlertsData(int userId,int typeId,int favstatus, int trashStatus);
        List<UserDropEntity> GetToUsers(int userId);
        int TrashMailRecord(List<MailEntity> entity);
        int TrashReadMailRecord(MailBodyEntity entity);
        IList GetAlertsDetails(int userId);
        int InsertAlertFavStatus(AlertStatusEntity entityObj);
        int TrashAlertRecord(List<AlertNotificationEntity> alerts);
        int InsertReadMail(int mailboxId, int userId);
        int GetReadMailsCount(int userId);
        List<ResidentDropEntity> GetAlertsResDrop(int userId, int companyId);
        List<AlertNotificationEntity> GetAllAlertsData(AlertsCustomEntity entity);
        int InsertReadAlert(long alertTextId,int userId);
    }
}
