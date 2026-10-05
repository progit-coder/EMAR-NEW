using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.DAL;
using System.Collections;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Data.Entity.Core.Objects;
using System.Security.Claims;

namespace LTCPro.Repositories
{
    public class MailboxRepository : IMailboxRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        public MailboxRepository(IAutoMapper autoMapper, IDbContextEmar dbContext, IRoleRepository roleRepository, IUserActivityRepository userActivityRepository)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
            this._roleRepository = roleRepository;
            _userActivityRepository = userActivityRepository;
        }

        public List<AlertNotificationEntity> GetAlertsData(int userId, int typeId, int favstatus, int trashStatus)
        {
            if (favstatus == 1)
            {
                var data = (from i in this.dbContext.AlertTexts
                            join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                            from x in fav.DefaultIfEmpty()
                            join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                            join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                            join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                            where s.User_Id == userId && i.AlertText_Status == 1 && x.Favourite_Flag == 1
                            select new AlertNotificationEntity
                            {
                                ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                TypeName = t.Type_Desc,
                                TypeId = t.Type_Id,
                                AlertStatus = i.AlertText_Status,
                                AlertTextId = i.AlertText_Id,
                                DateTime = i.AlertText_CreatedOn,
                                FavouriteFlag = x.Favourite_Flag == null ? 0 : x.Favourite_Flag,
                                ReadFlag = x.Read_Flag == null ? 0 : x.Read_Flag,
                            }).Distinct().ToList();

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var record = this.dbContext.AlertTexts.Where(e => e.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        var result = this.dbContext.AlertStatus.Where(s => s.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        if (record != null && result == null)
                        {
                            AlertStatu ss = new AlertStatu();
                            ss.AlertText_Id = item.AlertTextId;
                            ss.user_Id = userId;
                            ss.Favourite_Flag = 0;
                            ss.Read_Flag = 1;
                            ss.Alert_CreatedDate = DateTime.Now;
                            this.dbContext.AlertStatus.Add(ss);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                return data;
            }
            if (trashStatus == 1)
            {
                var data = (from i in this.dbContext.AlertTexts
                            join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                            from x in fav.DefaultIfEmpty()
                            join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                            join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                            join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                            where s.User_Id == userId && i.AlertText_Status == 2
                            select new AlertNotificationEntity
                            {
                                ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                TypeName = t.Type_Desc,
                                TypeId = t.Type_Id,
                                AlertStatus = i.AlertText_Status,
                                AlertTextId = i.AlertText_Id,
                                DateTime = i.AlertText_CreatedOn,
                                FavouriteFlag = x.Favourite_Flag == null ? 0 : x.Favourite_Flag,
                                ReadFlag = x.Read_Flag == null ? 0 : x.Read_Flag,
                            }).Distinct().ToList();

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var record = this.dbContext.AlertTexts.Where(e => e.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        var result = this.dbContext.AlertStatus.Where(s => s.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        if (record != null && result == null)
                        {
                            AlertStatu ss = new AlertStatu();
                            ss.AlertText_Id = item.AlertTextId;
                            ss.user_Id = userId;
                            ss.Favourite_Flag = 0;
                            ss.Read_Flag = 1;
                            ss.Alert_CreatedDate = DateTime.Now;
                            this.dbContext.AlertStatus.Add(ss);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                return data;
            }
            else if (typeId == 0)
            {
                var data = (from i in this.dbContext.AlertTexts
                            join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                            from x in fav.DefaultIfEmpty()
                            join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                            join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                            join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                            where s.User_Id == userId && i.AlertText_Status == 1
                            select new AlertNotificationEntity
                            {
                                ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                TypeName = t.Type_Desc,
                                TypeId = t.Type_Id,
                                AlertStatus = i.AlertText_Status,
                                AlertTextId = i.AlertText_Id,
                                DateTime = i.AlertText_CreatedOn,
                                FavouriteFlag = x.Favourite_Flag == null ? 0 : x.Favourite_Flag,
                                ReadFlag = x.Read_Flag == null ? 0 : x.Read_Flag,
                            }).Distinct().ToList();

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var record = this.dbContext.AlertTexts.Where(e => e.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        var result = this.dbContext.AlertStatus.Where(s => s.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        if (record != null && result == null)
                        {
                            AlertStatu ss = new AlertStatu();
                            ss.AlertText_Id = item.AlertTextId;
                            ss.user_Id = userId;
                            ss.Favourite_Flag = 0;
                            ss.Read_Flag = 1;
                            ss.Alert_CreatedDate = DateTime.Now;
                            this.dbContext.AlertStatus.Add(ss);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                return data;
            }
            else
            {
                var data = (from i in this.dbContext.AlertTexts
                            join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                            from x in fav.DefaultIfEmpty()
                            join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                            join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                            join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                            where s.User_Id == userId && i.AlertText_Status == 1 && i.Type_Id == typeId
                            select new AlertNotificationEntity
                            {
                                ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                TypeName = t.Type_Desc,
                                AlertStatus = i.AlertText_Status,
                                AlertTextId = i.AlertText_Id,
                                DateTime = i.AlertText_CreatedOn,
                                // FavouriteFlag = this.dbContext.AlertStatus.Where(e => e.AlertText_Id == i.AlertText_Id && e.user_Id == userId).Select(e => e.Favourite_Flag).FirstOrDefault() == null ? 0 : 1,
                                FavouriteFlag = x.Favourite_Flag == null ? 0 : x.Favourite_Flag,
                                ReadFlag = x.Read_Flag == null ? 0 : x.Read_Flag,
                            }).Distinct().ToList();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        var record = this.dbContext.AlertTexts.Where(e => e.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        var result = this.dbContext.AlertStatus.Where(s => s.AlertText_Id == item.AlertTextId).FirstOrDefault();
                        if (record != null && result == null)
                        {
                            AlertStatu ss = new AlertStatu();
                            ss.AlertText_Id = item.AlertTextId;
                            ss.user_Id = userId;
                            ss.Favourite_Flag = 0;
                            ss.Read_Flag = 1;
                            ss.Alert_CreatedDate = DateTime.Now;
                            this.dbContext.AlertStatus.Add(ss);
                            this.dbContext.SaveChanges();
                        }
                    }
                }
                return data;
            }
        }

        public MailBodyEntity GetMailBodyDetails(int mailBoxId,int userId)
        {
            var record = this.dbContext.MailReads.Where(m => m.MailBox_Id == mailBoxId && m.user_Id == userId).FirstOrDefault();
            if (record != null)
            {
                return (from mr in this.dbContext.MailReads
                        join m in this.dbContext.MailBoxes on mr.MailBox_Id equals m.MailBox_Id
                        join u in this.dbContext.Users on m.Fromuser_Id equals u.User_Id
                        where m.MailBox_Id == mailBoxId && mr.user_Id==userId
                        select new MailBodyEntity
                        {
                            Subject = m.Subject,
                            MailBody = m.MailBody,
                            AttachmentPath = m.AttachmentPath,
                            CcUserId = m.CcUser_Id,
                            CcUserName = m.CcUser_Id,
                            AttachmentFlag = 0,
                            ReadFlag = 1,
                            FavouriteFlag = 0,
                            FromUserId = m.Fromuser_Id,
                            FromUserName = u.User_DisplayName,
                            MailBoxDate = m.MailBox_Date,
                            MailBoxId = m.MailBox_Id,
                            ToUserId = mr.user_Id,
                            ToUserName = m.Touser_Id,
                            MailBox_Status = mr.Mail_Status,
                        }).FirstOrDefault();
            }
            else
            {
                var data = this.dbContext.MailBoxes.Where(m => m.MailBox_Id == mailBoxId && m.Fromuser_Id == userId).FirstOrDefault();
                if (data != null)
                {
                    return (from m in this.dbContext.MailBoxes
                            join u in this.dbContext.Users on m.Fromuser_Id equals u.User_Id
                            where m.MailBox_Id == mailBoxId && m.Fromuser_Id== userId
                            select new MailBodyEntity
                            {
                                Subject = m.Subject,
                                MailBody = m.MailBody,
                                AttachmentPath = m.AttachmentPath,
                                CcUserId = m.CcUser_Id,
                                CcUserName = m.CcUser_Id,
                                AttachmentFlag = 0,
                                ReadFlag = 1,
                                FavouriteFlag = 0,
                                FromUserId = m.Fromuser_Id,
                                FromUserName = u.User_DisplayName,
                                MailBoxDate = m.MailBox_Date,
                                MailBoxId = m.MailBox_Id,
                                ToUserId = m.Fromuser_Id,
                                ToUserName = m.Touser_Id,
                                MailBox_Status = m.MailBox_Status,
                            }).FirstOrDefault();
                }
            }
            return null;
        }

        public List<MailEntity> GetMailboxDetails(int userId, string folderName)
        {
            if (folderName == "Inbox")
            {   
                var data = (from mr in this.dbContext.MailReads
                            join m in this.dbContext.MailBoxes on mr.MailBox_Id equals m.MailBox_Id
                            join u in this.dbContext.Users on m.Fromuser_Id equals u.User_Id
                            where mr.user_Id == userId && mr.Mail_Status == 1
                            select new MailEntity
                            {
                                MailBoxId = m.MailBox_Id,
                                Subject = m.Subject,
                                AttachmentFlag = m.AttachmentPath == null ? 0 : 1,
                                MailBoxDate = m.MailBox_Date,
                                DisplayDate = string.Empty,
                                UserId = mr.user_Id,
                                UserName = u.User_Fname +" "+u.User_Lname,
                                MailBox_Status = m.MailBox_Status,
                                ReadFlag = mr.MailRead_Status,
                                FavouriteFlag = mr.MailFav_Status,
                                ToUserId = mr.user_Id
                            }).OrderByDescending(item => item.MailBoxId).ToList();
                return data;
            }   
            else if (folderName == "Sent")
            {
                var data = (from mr in this.dbContext.MailReads
                            join m in this.dbContext.MailBoxes on mr.MailBox_Id equals m.MailBox_Id
                            join u in this.dbContext.Users on mr.user_Id equals u.User_Id
                            where m.Fromuser_Id == userId && mr.FromMail_Status ==1
                            select new MailEntity
                            {
                                MailBoxId = m.MailBox_Id,
                                Subject = m.Subject,
                                AttachmentFlag = m.AttachmentPath == null ? 0 : 1,
                                MailBoxDate = m.MailBox_Date,
                                DisplayDate = string.Empty,
                                UserId = mr.user_Id,
                                UserName = u.User_Fname + " " + u.User_Lname,
                                MailBox_Status = m.MailBox_Status,
                                ReadFlag = 1,//MailRead_Status,
                                FavouriteFlag = mr.MailFav_Status,
                                ToUserId = mr.user_Id
                            }).OrderByDescending(item => item.MailBoxId).ToList();
                return data;
            }
            else if (folderName == "Drafts")
            {
                var data = (from m in this.dbContext.MailBoxes
                            join u in this.dbContext.Users on m.Fromuser_Id equals u.User_Id
                            where (m.Fromuser_Id == userId) && m.MailBox_Status == 0
                            select new MailEntity
                            {
                                MailBoxId = m.MailBox_Id,
                                Subject = m.Subject,
                                AttachmentFlag = m.AttachmentPath == null ? 0 : 1,
                                MailBoxDate = m.MailBox_Date,
                                DisplayDate = string.Empty,
                                UserId = m.Fromuser_Id,
                                ToUserId = m.Fromuser_Id,
                                UserName = u.User_Fname + " " + u.User_Lname,
                                MailBox_Status = m.MailBox_Status,
                                ReadFlag = 1,
                                FavouriteFlag = 0,
                            }).OrderByDescending(item => item.MailBoxId).ToList();
                return data;
            }
            else if (folderName == "Fav")
            {
                var data = (from mr in this.dbContext.MailReads
                            join m in this.dbContext.MailBoxes on mr.MailBox_Id equals m.MailBox_Id
                            join u in this.dbContext.Users on m.Fromuser_Id equals u.User_Id
                            //join f in this.dbContext.MailFavourites on m.MailBox_Id equals f.MailBox_Id
                            where (mr.user_Id == userId ) && (mr.Mail_Status == 1 || m.MailBox_Status == 1) && mr.MailFav_Status == 1
                            select new MailEntity
                            {
                                MailBoxId = m.MailBox_Id,
                                Subject = m.Subject,
                                AttachmentFlag = m.AttachmentPath == null ? 0 : 1,
                                MailBoxDate = m.MailBox_Date,
                                DisplayDate = string.Empty,
                                UserId = mr.user_Id,
                                UserName = u.User_Fname + " " + u.User_Lname,
                                MailBox_Status = m.MailBox_Status,
                                ReadFlag = mr.MailRead_Status,
                                FavouriteFlag = mr.MailFav_Status,
                                ToUserId = mr.user_Id
                            }).OrderByDescending(item => item.MailBoxId).ToList();
                return data;
            }
            else if (folderName == "Trash")
            {
                var data = (from mr in this.dbContext.MailReads
                            join m in this.dbContext.MailBoxes on mr.MailBox_Id equals m.MailBox_Id
                            join u in this.dbContext.Users on mr.user_Id equals u.User_Id
                            //where (mr.user_Id == userId || m.Fromuser_Id == userId) && (mr.Mail_Status == 2 || mr.FromMail_Status ==2) 
                            where (mr.user_Id==userId && mr.Mail_Status==2) || (m.Fromuser_Id==userId && mr.FromMail_Status==2)
                            select new MailEntity
                            {
                                MailBoxId = m.MailBox_Id,
                                Subject = m.Subject,
                                AttachmentFlag = m.AttachmentPath == null ? 0 : 1,
                                MailBoxDate = m.MailBox_Date,
                                DisplayDate = string.Empty,
                                UserId = mr.user_Id,
                                UserName = u.User_Fname + " " + u.User_Lname,
                                MailBox_Status = m.MailBox_Status,
                                ReadFlag = mr.MailRead_Status,
                                FavouriteFlag = mr.MailFav_Status,
                                ToUserId = mr.user_Id
                            }).OrderByDescending(item => item.MailBoxId).ToList();
                return data;
            }
            return null;
        }

        public int InsertComposeMail(MailComposeEntity mailBodyObj)
        {
            var record = this.autoMapper.Map<MailComposeEntity, MailBox>(mailBodyObj);
            var data = this.dbContext.MailBoxes.Where(pt => pt.MailBox_Id == record.MailBox_Id).FirstOrDefault();
            if (data == null || (data!=null && mailBodyObj.MailBox_Status == 1))
            {
                
                this.dbContext.MailBoxes.Add(record);
                this.dbContext.SaveChanges();
                if (mailBodyObj.MailBox_Status == 1)
                {
                    //var userId = record.Touser_Id == null ? record.CcUser_Id : record.Touser_Id;
                    if (record.Touser_Id != "null" || record.CcUser_Id != "null")
                    {
                        string[] ToUsers = record.Touser_Id != null ? record.Touser_Id.Split(',') : new string[] { };
                        string[] CCUsers = record.CcUser_Id != null ? record.CcUser_Id.Split(',') : new string[] { };
                        var users = ToUsers.Concat(CCUsers).ToArray().Distinct();
                        if (users.Count() > 0)
                        {
                            foreach (var item in users)
                            {
                                int userId = Convert.ToInt32(item);
                                InsertReadMail(record.MailBox_Id, Convert.ToInt32(item));
                                var ToUserDetails = this.dbContext.Users.Where(u => u.User_Id == userId).FirstOrDefault();
                                var fromUserDetails = this.dbContext.Users.Where(u => u.User_Id == record.Fromuser_Id).FirstOrDefault();
                                string fromUersMail = fromUserDetails.User_Email;
                                var mailConfigData = this._roleRepository.GetMailconfigDetails(1);
                                string body = string.Empty;
                                //new impliemnted on 04/15/2026 start
                                int sid = 0;
                                if (ToUserDetails.User_Suffix != "" && ToUserDetails.User_Suffix != null)
                                {
                                    sid = Convert.ToInt16(ToUserDetails.User_Suffix);
                                }
                                string suffixName = this.dbContext.Suffixes.Where(s => s.Suffix_Id == sid).Select(s => s.Suffix_Desc).FirstOrDefault();

                                if (suffixName == "" || suffixName == null)
                                    suffixName = "";

                                //end
                                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Controllers/HtmlTemplate.html")))
                                {
                                    body = reader.ReadToEnd();
                                }
                                //body = body.Replace("{UserName}", ToUserDetails.User_Fname + " " + ToUserDetails.User_Lname);
                                body = body.Replace("{UserName}", suffixName + " " + ToUserDetails.User_Fname + " " + ToUserDetails.User_Lname);

                                body = body.Replace("{Text}", record.MailBody);
                                body = body.Replace("{OTP}", "");
                                if (ToUserDetails != null && mailConfigData != null)
                                {
                                    SmtpClient SmtpServer = new SmtpClient("smtp1-mke.securence.com");
                                    MailMessage mail = new MailMessage();
                                    mail.From = new MailAddress(fromUersMail);
                                    mail.To.Add(ToUserDetails.User_Email);
                                    mail.Subject = record.Subject;
                                    mail.Body = body;
                                    mail.IsBodyHtml = true;
                                    SmtpServer.Port = Convert.ToInt16(587);
                                    SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
                                    SmtpServer.UseDefaultCredentials = false;
                                    SmtpServer.EnableSsl = false;
                                    SmtpServer.Send(mail);
                                }
                            }
                        }
                        UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                        {
                            Screen_Id = (int)ScreenEntity.Screens.Mailbox,
                            Activity_Id = (int)ActivityEntity.ActivityMaster.Save,
                            Comments = record.MailBox_Id.ToString(),
                            Session_Id = 0,
                            Time = DateTime.Now,
                            UserActivity_Id = 0,

                        };

                        _userActivityRepository.InsertUserActivityDetails(activityEntity);
                        return 1;
                    }
                }
            }
            else if(data!=null && mailBodyObj.MailBox_Status == 0)
            {
                data.Touser_Id = mailBodyObj.Touser_Id;
                data.CcUser_Id = mailBodyObj.CcUser_Id;
                data.Subject = mailBodyObj.Subject;
                data.MailBody = mailBodyObj.MailBody;
                data.Fromuser_Id = mailBodyObj.Fromuser_Id;
                data.ToMalId = mailBodyObj.ToMalId;
                data.AttachmentPath = mailBodyObj.AttachmentPath;
                data.MailBox_Status = mailBodyObj.MailBox_Status;
                data.MailBox_Date = mailBodyObj.MailBox_Date;
                this.dbContext.SaveChanges();
                UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
                {
                    Screen_Id = (int)ScreenEntity.Screens.Mailbox,
                    Activity_Id = (int)ActivityEntity.ActivityMaster.Edit,
                    Comments = record.MailBox_Id.ToString(),
                    Session_Id = 0,
                    Time = DateTime.Now,
                    UserActivity_Id = 0,

                };

                _userActivityRepository.InsertUserActivityDetails(activityEntity);
            }
            return 1;
        }

        public int InsertUpdateFavouritesMail(MailFavouritesEntity mailFavObj)
        {
            var record = this.autoMapper.Map<MailFavouritesEntity, MailFavourite>(mailFavObj);
            var data = this.dbContext.MailReads.Where(e => e.MailBox_Id == mailFavObj.MailBox_Id && e.user_Id == mailFavObj.user_Id).FirstOrDefault();
            if (data == null)
            {
                MailRead obj = new MailRead();
                obj.MailBox_Id = mailFavObj.MailBox_Id;
                obj.user_Id = mailFavObj.user_Id;
                obj.MailFav_Status = mailFavObj.MailFavourite_Status;
                this.dbContext.MailReads.Add(obj);
                this.dbContext.SaveChanges();
            }
            else
            {

                data.MailFav_Status = mailFavObj.MailFavourite_Status;
                this.dbContext.SaveChanges();
            }

            return 1;
        }
        public List<UserDropEntity> GetToUsers(int userId)
        {
            List<UserDropEntity> toUsers = new List<UserDropEntity>();
            List<int> nsIds = this.dbContext.UserRoleFacilityConfigs.Where(i => i.User_Id == userId).Select(i => (int)i.NurseStation_Id).ToList();
            if (nsIds.Count() > 0)
            {
                //List<int> companyIds = this.dbContext.Facilities.Where(p => facilityIds.Contains(p.Facility_Id)).Select(p => p.Company_Id).ToList();

                //var toFacilityIds = this.dbContext.Facilities.Where(f => companyIds.Contains(f.Company_Id)).Select(f => f.Facility_Id).ToList();
                var userIds = this.dbContext.UserRoleFacilityConfigs.Where(f => nsIds.Contains((int)f.NurseStation_Id)).Select(u => u.User_Id).Distinct();
                toUsers = (from u in this.dbContext.Users
                           where u.User_Status == 1 && userIds.Contains(u.User_Id)
                           select new UserDropEntity
                           {
                               User_Id = u.User_Id,
                               User_DisplayName = u.User_DisplayName
                           }).OrderBy(item => item.User_DisplayName).ToList();

            }
            return toUsers;
        }
        public int TrashMailRecord(List<MailEntity> entity)
        {
            foreach (var item in entity)
            {
                MailRead list = this.dbContext.MailReads.Where(c => c.MailBox_Id == item.MailBoxId && c.user_Id == item.ToUserId).FirstOrDefault();
                if (list != null && (list.Mail_Status == 1))
                {
                    if(item.InboxFlag == "Sent")
                    {
                        list.FromMail_Status = 2;
                    }
                    else if(list.FromMail_Status == 2)
                    {
                        list.FromMail_Status = 3;
                    }
                    else
                    {
                        list.Mail_Status = 2;
                    }
                  
                    list.MailRead_Status = 1;
                    list.MailRead_Date = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
                else if (list != null && list.Mail_Status == 2)
                {
                    if (item.InboxFlag == "Sent")
                    {
                        list.FromMail_Status = 2;
                    }
                    else if (list.FromMail_Status == 2)
                    {
                        list.FromMail_Status = 3;
                    }
                    else
                    {
                        list.Mail_Status = 3;
                    }
                    list.MailRead_Date = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
                else if (list != null && list.FromMail_Status == 2)
                {
                    list.FromMail_Status = 3;
                    list.MailRead_Date = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
                    if (list == null)
                {
                    MailBox record = this.dbContext.MailBoxes.Where(m => m.MailBox_Id == item.MailBoxId && (item.MailBox_Status == 0 || item.MailBox_Status == 1 || item.MailBox_Status == 2)).FirstOrDefault();
                    if (record != null && (record.MailBox_Status == 0 || record.MailBox_Status == 1))
                    {
                        record.MailBox_Status = 2;
                        record.MailBox_Date = DateTime.Now;
                        MailRead obj = new MailRead();
                        obj.MailBox_Id = item.MailBoxId;
                        obj.user_Id = this.dbContext.MailBoxes.Where(m => m.MailBox_Id == item.MailBoxId).Select(m => m.Fromuser_Id).FirstOrDefault();
                        obj.MailFav_Status = 0;
                        obj.MailRead_Status = 1;
                        obj.Mail_Status = 2;
                        obj.MailRead_Date = DateTime.Now;
                        this.dbContext.MailReads.Add(obj);
                        this.dbContext.SaveChanges();
                    }
                    else if (record != null && record.MailBox_Status == 2)
                    {
                        
                        MailRead check = this.dbContext.MailReads.Where(c => c.MailBox_Id == item.MailBoxId && c.user_Id == item.ToUserId).FirstOrDefault();
                        if (check != null)
                        {
                            check.Mail_Status = 3;
                            check.MailRead_Date = DateTime.Now;
                        }
                        else
                        {
                            MailBox data = this.dbContext.MailBoxes.Where(c => c.MailBox_Id == item.MailBoxId).FirstOrDefault();
                            if (data != null)
                            {
                                data.MailBox_Status = 3;
                                data.MailBox_Date = DateTime.Now;
                                this.dbContext.SaveChanges();
                            }
                        }
                        this.dbContext.SaveChanges();
                    }
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Mailbox,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Delete,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int TrashReadMailRecord(MailBodyEntity entity)
        {
            MailRead list = this.dbContext.MailReads.Where(c => c.MailBox_Id == entity.MailBoxId && c.user_Id == entity.ToUserId).FirstOrDefault();
            if (list != null && (list.Mail_Status == 1))
            {
                if (entity.InboxFlag == "Sent")
                {
                    list.FromMail_Status = 2;
                }
                else if (list.FromMail_Status == 2)
                {
                    list.FromMail_Status = 3;
                }
                else
                {
                    list.Mail_Status = 2;
                }
                //list.Mail_Status = 2;
                list.MailRead_Date = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            else if (list != null && list.Mail_Status == 2)
            {
                if (entity.InboxFlag == "Sent")
                {
                    list.FromMail_Status = 2;
                }
                else if (list.FromMail_Status == 2)
                {
                    list.FromMail_Status = 3;
                }
                else
                {
                    list.Mail_Status = 3;
                }
                list.MailRead_Date = DateTime.Now;
                this.dbContext.SaveChanges();
                //this.dbContext.MailReads.Remove(list);
                //this.dbContext.SaveChanges();
            }
            else if (list != null && list.FromMail_Status == 2)
            {
                list.FromMail_Status = 3;
                list.MailRead_Date = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            if (list == null)
            {
                MailBox record = this.dbContext.MailBoxes.Where(m => m.MailBox_Id == entity.MailBoxId && (entity.MailBox_Status == 0 || entity.MailBox_Status == 1)).FirstOrDefault();
                if (record != null && (record.MailBox_Status == 0 || record.MailBox_Status == 1))
                {
                    record.MailBox_Status = 2;
                    record.MailBox_Date = DateTime.Now;
                    MailRead obj = new MailRead();
                    obj.MailBox_Id = entity.MailBoxId;
                    obj.user_Id = this.dbContext.MailBoxes.Where(m => m.MailBox_Id == entity.MailBoxId).Select(m => m.Fromuser_Id).FirstOrDefault();
                    obj.MailFav_Status = 0;
                    obj.MailRead_Status = 1;
                    obj.Mail_Status = 2;
                    obj.MailRead_Date = DateTime.Now;
                    this.dbContext.MailReads.Add(obj);
                    this.dbContext.SaveChanges();
                }
                else if (record != null && record.MailBox_Status == 2)
                {
                    MailRead check = this.dbContext.MailReads.Where(c => c.MailBox_Id == entity.MailBoxId && c.user_Id == entity.ToUserId).FirstOrDefault();
                    if (check != null)
                    {
                        if (entity.InboxFlag == "Sent")
                        {
                            check.FromMail_Status = 2;
                        }
                        else if (list.FromMail_Status == 2)
                        {
                            check.FromMail_Status = 3;
                        }
                        else
                        {
                            check.Mail_Status = 3;
                        }
                        //check.Mail_Status = 3;
                        check.MailRead_Date = DateTime.Now;
                    }
                    else
                    {
                        MailBox data = this.dbContext.MailBoxes.Where(c => c.MailBox_Id == entity.MailBoxId).FirstOrDefault();
                        if (data != null)
                        {
                            data.MailBox_Status = 3;
                            data.MailBox_Date = DateTime.Now;
                            this.dbContext.SaveChanges();
                        }
                    }
                    this.dbContext.SaveChanges();
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Mailbox,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Delete,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }

        public IList GetAlertsDetails(int userId)
        {
            var data = this.dbContext.AlertTypeDataCount(userId).ToList();
            return data;
        }

        public int InsertAlertFavStatus(AlertStatusEntity entityObj)
        {
            var record = this.autoMapper.Map<AlertStatusEntity, AlertStatu>(entityObj);
            var data = this.dbContext.AlertStatus.Where(e => e.AlertText_Id == entityObj.AlertText_Id).FirstOrDefault();
            if(data==null)
            {
                AlertStatu obj = new AlertStatu();
                obj.AlertText_Id = record.AlertText_Id;
                obj.user_Id = record.user_Id;
                obj.Favourite_Flag = record.Favourite_Flag;
                obj.Alert_CreatedDate = DateTime.Now;
                this.dbContext.AlertStatus.Add(obj);
                this.dbContext.SaveChanges();
            }
            else
            {
                data.Favourite_Flag = record.Favourite_Flag;
                data.Alert_CreatedDate = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            //if (entityObj.Favourite_Flag == 1)
            //{
            //    data.Favourite_Flag = 1;
            //    data.Alert_CreatedDate = DateTime.Now;
            //    this.dbContext.SaveChanges();
            //}
            //else
            //{
            //    data.Favourite_Flag = 0;
            //    data.Alert_CreatedDate = DateTime.Now;
            //    this.dbContext.SaveChanges();
            //}
            return 1;
        }
        public int TrashAlertRecord(List<AlertNotificationEntity> alerts)
        {
            foreach (var item in alerts)
            {
                AlertText list = this.dbContext.AlertTexts.Where(c => c.AlertText_Id == item.AlertTextId).FirstOrDefault();
                if (list != null && (list.AlertText_Status == 1))
                {
                    list.AlertText_Status = 2;
                    list.AlertText_CreatedOn = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
                else if (list != null && list.AlertText_Status == 2)
                {
                    list.AlertText_Status = 3;
                    list.AlertText_CreatedOn = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
            }
            UserActivityDetailEntity activityEntity = new UserActivityDetailEntity()
            {
                Screen_Id = (int)ScreenEntity.Screens.Alerts,
                Activity_Id = (int)ActivityEntity.ActivityMaster.Delete,
                Comments = "",
                Session_Id = 0,
                Time = DateTime.Now,
                UserActivity_Id = 0,

            };

            _userActivityRepository.InsertUserActivityDetails(activityEntity);
            return 1;
        }
        public int InsertReadMail(int mailboxId, int userId)
        {
            // var record = this.autoMapper.Map<AlertStatusEntity, AlertStatu>(entityObj);
            var record = this.dbContext.MailBoxes.Where(m => m.MailBox_Status == 1 && m.MailBox_Id == mailboxId).FirstOrDefault();
            if (record != null)
            {
                var data = this.dbContext.MailReads.Where(e => e.MailBox_Id == mailboxId && e.user_Id == userId).FirstOrDefault();
                if (data == null)
                {
                    MailRead obj = new MailRead();
                    obj.MailBox_Id = mailboxId;
                    obj.user_Id = userId;
                    obj.MailRead_Status = 0;
                    obj.MailFav_Status = 0;
                    obj.Mail_Status = 1;
                    obj.FromMail_Status = 1;
                    obj.MailRead_Date = DateTime.Now;
                    this.dbContext.MailReads.Add(obj);
                    this.dbContext.SaveChanges();
                }
                else
                {
                    data.MailRead_Status = 1;
                    data.MailRead_Date = DateTime.Now;
                    this.dbContext.SaveChanges();
                }
            }
            return 1;
        }
        public int GetReadMailsCount(int userId)
        {
            return this.dbContext.MailReads.Where(m => m.MailRead_Status == 0 && m.Mail_Status!=3 && m.user_Id == userId).ToList().Count();
        }
        public List<ResidentDropEntity> GetAlertsResDrop(int userId, int companyId)
        {
            List<int> companyFacId = this.dbContext.Facilities.Where(f => f.Company_Id == companyId).Select(f => f.Facility_Id).Distinct().ToList();
            List<int?> nsIds = this.dbContext.UserRoleFacilityConfigs.Where(ur => ur.User_Id == userId && companyFacId.Contains(ur.Facility_id)).Select(ur => ur.NurseStation_Id).Distinct().ToList();
            var records = (from at in this.dbContext.AlertTexts
                           join s in this.dbContext.UserRoleFacilityConfigs on at.NurseStation_Id equals s.NurseStation_Id
                           //join vi in this.dbContext.VisitInfoes on at.NurseStation_Id equals vi.NursingStationId
                           join de in this.dbContext.Demographics on at.Patient_Id equals de.Patient_Id
                           where s.User_Id==userId && nsIds.Contains(s.NurseStation_Id)
                           select new ResidentDropEntity
                           {
                               Patient_Id = de.Patient_Id,
                               PatientName = de.PatientLastName + ", " + de.PatientFirstName + " " + (de.PatientMiddleInitial == null ? "" : de.PatientMiddleInitial),
                           }).OrderBy(item => item.PatientName).ToList();
            List<ResidentDropEntity> resList = records.GroupBy(m => new { m.Patient_Id }).Select(group => group.FirstOrDefault()).ToList();
            return resList;
        }
        public List<AlertNotificationEntity> GetAllAlertsData(AlertsCustomEntity entity)
        {
            List<string> resIds = entity.Residents.Split(',').ToList();
            int userId = 0;
            List<int> alertType=new List<int>();
            if (entity.AlertTypeId == 0)
            {
                alertType = this.dbContext.AlertTypes.Select(a => a.Type_Id).ToList();
            }
            else
            {
                alertType.Add(entity.AlertTypeId);
            }
            int count = 0;
            entity.SearchText = entity.SearchText != "null" && entity.SearchText != null && entity.SearchText != string.Empty ? entity.SearchText.ToLower() : string.Empty;
            int skipRows = (entity.CurrentPage - 1) * entity.PageSize;
            if (entity.SearchText == string.Empty)
            {
                if (entity.TypeId == 1)
                {
                    count =     (from i in this.dbContext.AlertTexts
                                 join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                                 from x in fav.DefaultIfEmpty()
                                 //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                 join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                 join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                 join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                 where  alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1
                                 select i).Distinct().Count();

                    var data = (from i in this.dbContext.AlertTexts
                                join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                                from x in fav.DefaultIfEmpty()
                                join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1
                                select new AlertNotificationEntity
                                {
                                    ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                    TypeName = t.Type_Desc,
                                    TypeId = t.Type_Id,
                                    AlertStatus = i.AlertText_Status,
                                    AlertTextId = i.AlertText_Id,
                                    DateTime = i.AlertText_CreatedOn,
                                    FavouriteFlag = x.Favourite_Flag == null ? 0 : x.Favourite_Flag,
                                    ReadFlag = x.Read_Flag == null ? 0 : x.Read_Flag,
                                    File_Id = i.File_Id,
                                    TotalRecords=count,
                                    NurseStationName=ns.NurseStation_Name
                                }).Distinct().OrderByDescending(item => item.DateTime).Skip(skipRows).Take(entity.PageSize).ToList();
                    return data;
                }
                if (entity.TypeId == 2)
                {
                    count = (from a in this.dbContext.AlertStatus
                             join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                             join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                             //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                             join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                             join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                             where  alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1 && a.Favourite_Flag == 1
                             select i).Distinct().Count();

                    var data = (from a in this.dbContext.AlertStatus
                                join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                                join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1 && a.Favourite_Flag == 1
                                select new AlertNotificationEntity
                                {
                                    ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                    TypeName = t.Type_Desc,
                                    TypeId = t.Type_Id,
                                    AlertStatus = i.AlertText_Status,
                                    AlertTextId = i.AlertText_Id,
                                    DateTime = i.AlertText_CreatedOn,
                                    FavouriteFlag = a.Favourite_Flag == null ? 0 : a.Favourite_Flag,
                                    ReadFlag = a.Read_Flag == null ? 0 : a.Read_Flag,
                                    File_Id = i.File_Id,
                                    TotalRecords = count,
                                    NurseStationName = ns.NurseStation_Name
                                }).Distinct().OrderByDescending(item => item.DateTime).Skip(skipRows).Take(entity.PageSize).ToList();
                    return data;
                }
                if (entity.TypeId == 3)
                {
                    count = (from a in this.dbContext.AlertStatus
                             join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                             join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                             //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                             join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                             join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                             where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && (i.AlertText_Status == 2)
                             select i).Distinct().Count();

                    var data = (from a in this.dbContext.AlertStatus
                                join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                                join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && (i.AlertText_Status == 2)
                                select new AlertNotificationEntity
                                {
                                    ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                    TypeName = t.Type_Desc,
                                    TypeId = t.Type_Id,
                                    AlertStatus = i.AlertText_Status,
                                    AlertTextId = i.AlertText_Id,
                                    DateTime = i.AlertText_CreatedOn,
                                    FavouriteFlag = a.Favourite_Flag == null ? 0 : a.Favourite_Flag,
                                    ReadFlag = a.Read_Flag == null ? 0 : a.Read_Flag,
                                    File_Id = i.File_Id,
                                    TotalRecords=count,
                                    NurseStationName = ns.NurseStation_Name
                                }).Distinct().OrderByDescending(item => item.DateTime).Skip(skipRows).Take(entity.PageSize).ToList();
                    return data;
                }
            }
            else if (entity.SearchText != string.Empty)
            {
                if (entity.TypeId == 1)
                {
                    count = (from i in this.dbContext.AlertTexts
                             join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                             from x in fav.DefaultIfEmpty()
                             join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                             //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                             join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                             join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                             where  alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1
                             &&((ns.NurseStation_Name.ToLower().Contains(entity.SearchText.ToLower()))||(r.PatientLastName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientFirstName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientMiddleInitial.ToLower().Contains(entity.SearchText.ToLower())) ||(t.Type_Desc.ToLower().Contains(entity.SearchText.ToLower())) 
                             ||(EntityFunctions.TruncateTime(i.AlertText_CreatedOn).ToString()).Contains(entity.SearchText))
                             select i).Distinct().Count();

                    var data = (from i in this.dbContext.AlertTexts
                                join a in this.dbContext.AlertStatus on i.AlertText_Id equals a.AlertText_Id into fav
                                from x in fav.DefaultIfEmpty()
                                join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1
                                && ((ns.NurseStation_Name.ToLower().Contains(entity.SearchText.ToLower()))|| (r.PatientLastName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientFirstName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientMiddleInitial.ToLower().Contains(entity.SearchText.ToLower())) || (t.Type_Desc.ToLower().Contains(entity.SearchText.ToLower()))
                                || (EntityFunctions.TruncateTime(i.AlertText_CreatedOn).ToString()).Contains(entity.SearchText))
                                select new AlertNotificationEntity
                                {
                                    ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                    TypeName = t.Type_Desc,
                                    TypeId = t.Type_Id,
                                    AlertStatus = i.AlertText_Status,
                                    AlertTextId = i.AlertText_Id,
                                    DateTime = i.AlertText_CreatedOn,
                                    FavouriteFlag = x.Favourite_Flag == null ? 0 : x.Favourite_Flag,
                                    ReadFlag = x.Read_Flag == null ? 0 : x.Read_Flag,
                                    File_Id = i.File_Id,
                                    TotalRecords = count,
                                    NurseStationName = ns.NurseStation_Name
                                }).Distinct().OrderByDescending(item => item.DateTime).Skip(skipRows).Take(entity.PageSize).ToList();
                    return data;
                }
                if (entity.TypeId == 2)
                {
                    count = (from a in this.dbContext.AlertStatus
                             join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                             join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                             //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                             join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                             join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                             where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1 && a.Favourite_Flag == 1
                             && ((ns.NurseStation_Name.ToLower().Contains(entity.SearchText.ToLower()))|| (r.PatientLastName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientFirstName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientMiddleInitial.ToLower().Contains(entity.SearchText.ToLower())) || (t.Type_Desc.ToLower().Contains(entity.SearchText.ToLower()))
                             || (EntityFunctions.TruncateTime(i.AlertText_CreatedOn).ToString()).Contains(entity.SearchText))
                             select i).Distinct().Count();

                    var data = (from a in this.dbContext.AlertStatus
                                join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                                join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && i.AlertText_Status == 1 && a.Favourite_Flag == 1
                                && ((ns.NurseStation_Name.ToLower().Contains(entity.SearchText.ToLower()))||(r.PatientLastName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientFirstName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientMiddleInitial.ToLower().Contains(entity.SearchText.ToLower())) || (t.Type_Desc.ToLower().Contains(entity.SearchText.ToLower()))
                                || (EntityFunctions.TruncateTime(i.AlertText_CreatedOn).ToString()).Contains(entity.SearchText))
                                select new AlertNotificationEntity
                                {
                                    ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                    TypeName = t.Type_Desc,
                                    TypeId = t.Type_Id,
                                    AlertStatus = i.AlertText_Status,
                                    AlertTextId = i.AlertText_Id,
                                    DateTime = i.AlertText_CreatedOn,
                                    FavouriteFlag = a.Favourite_Flag == null ? 0 : a.Favourite_Flag,
                                    ReadFlag = a.Read_Flag == null ? 0 : a.Read_Flag,
                                    File_Id = i.File_Id,
                                    TotalRecords = count,
                                    NurseStationName = ns.NurseStation_Name
                                }).Distinct().OrderByDescending(item => item.DateTime).Skip(skipRows).Take(entity.PageSize).ToList();
                    return data;
                }
                if (entity.TypeId == 3)
                {
                    count = (from a in this.dbContext.AlertStatus
                             join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                             join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                             //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                             join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                             join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                             where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && (i.AlertText_Status == 2)
                             && ((ns.NurseStation_Name.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientLastName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientFirstName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientMiddleInitial.ToLower().Contains(entity.SearchText.ToLower())) || (t.Type_Desc.ToLower().Contains(entity.SearchText.ToLower()))
                             || (EntityFunctions.TruncateTime(i.AlertText_CreatedOn).ToString()).Contains(entity.SearchText))
                             select i).Distinct().Count();

                    var data = (from a in this.dbContext.AlertStatus
                                join i in this.dbContext.AlertTexts on a.AlertText_Id equals i.AlertText_Id
                                join ns in this.dbContext.NursingStations on i.NurseStation_Id equals ns.NurseStation_Id
                                //join s in this.dbContext.UserRoleFacilityConfigs on i.NurseStation_Id equals s.NurseStation_Id
                                join r in this.dbContext.Demographics on i.Patient_Id equals r.Patient_Id
                                join t in this.dbContext.AlertTypes on i.Type_Id equals t.Type_Id
                                where alertType.Contains((int)i.Type_Id) && resIds.Contains(i.Patient_Id.ToString()) && (i.AlertText_Status == 2)
                                && ((r.PatientLastName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientFirstName.ToLower().Contains(entity.SearchText.ToLower())) || (r.PatientMiddleInitial.ToLower().Contains(entity.SearchText.ToLower())) || (t.Type_Desc.ToLower().Contains(entity.SearchText.ToLower()))
                                || (EntityFunctions.TruncateTime(i.AlertText_CreatedOn).ToString()).Contains(entity.SearchText))
                                select new AlertNotificationEntity
                                {
                                    ResidentName = r.PatientLastName + ", " + r.PatientFirstName + " " + (r.PatientMiddleInitial != null ? r.PatientMiddleInitial : ""),
                                    TypeName = t.Type_Desc,
                                    TypeId = t.Type_Id,
                                    AlertStatus = i.AlertText_Status,
                                    AlertTextId = i.AlertText_Id,
                                    DateTime = i.AlertText_CreatedOn,
                                    FavouriteFlag = a.Favourite_Flag == null ? 0 : a.Favourite_Flag,
                                    ReadFlag = a.Read_Flag == null ? 0 : a.Read_Flag,
                                    File_Id = i.File_Id,
                                    TotalRecords = count,
                                    NurseStationName = ns.NurseStation_Name
                                }).Distinct().OrderByDescending(item => item.DateTime).Skip(skipRows).Take(entity.PageSize).ToList();
                    return data;
                }
            }
            return null;
        }
        public int InsertReadAlert(long alertTextId,int userId)
        {
            var record = this.dbContext.AlertStatus.Where(a => a.AlertText_Id == alertTextId).FirstOrDefault();
            if(record==null)
            {
                AlertStatu obj = new AlertStatu();
                obj.AlertText_Id = alertTextId;
                obj.user_Id = userId;
                obj.Read_Flag = 1;
                obj.Favourite_Flag = 0;
                obj.Alert_CreatedDate = DateTime.Now;
                this.dbContext.AlertStatus.Add(obj);
                this.dbContext.SaveChanges();
            }
            else
            {
                record.Read_Flag = 1;
                record.Alert_CreatedDate = DateTime.Now;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
    }
}
