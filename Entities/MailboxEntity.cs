using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTCPro.Entities
{
    public class MailEntity
    {
        public int MailBoxId { get; set; }
        public string Subject { get; set; }
        public Nullable<int> AttachmentFlag { get; set; }
        public Nullable<System.DateTime> MailBoxDate { get; set; }
        public string DisplayDate { get; set; }
        public Nullable<int> FavouriteFlag { get; set; }
        public Nullable<int> ReadFlag { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> MailBox_Status { get; set; }
        public string UserName { get; set; }
        public int? ToUserId { get; set; }
        public string InboxFlag { get; set; }
    }
    public class MailBodyEntity
    {
        public int MailBoxId { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public Nullable<int> AttachmentFlag { get; set; }
        public string AttachmentPath { get; set; }
        public Nullable<System.DateTime> MailBoxDate { get; set; }
        public Nullable<int> FavouriteFlag { get; set; }
        public Nullable<int> ReadFlag { get; set; }
        public Nullable<int> FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public string CcUserId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string CcUserName { get; set; }
        public Nullable<int> MailBox_Status { get; set; }
        public string InboxFlag { get; set; }
    }
    public class MailComposeEntity
    {
        public int MailBox_Id { get; set; }
        public Nullable<int> Fromuser_Id { get; set; }
        public string Touser_Id { get; set; }
        public string ToMalId { get; set; }
        public string CcUser_Id { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public string AttachmentPath { get; set; }
        public Nullable<int> MailBox_Status { get; set; }
        public Nullable<System.DateTime> MailBox_Date { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
    public class MailFavouritesEntity
    {
        public int MailFavourite_Id { get; set; }
        public Nullable<int> MailBox_Id { get; set; }
        public Nullable<int> user_Id { get; set; }
        public Nullable<int> MailFavourite_Status { get; set; }
        public Nullable<System.DateTime> MailFavourite_date { get; set; }
    }
   
}
