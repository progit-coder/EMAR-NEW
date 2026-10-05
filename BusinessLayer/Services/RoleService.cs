using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;
using System.Net.Mail;
using System.IO;
using System.Web;
using System.Configuration;

namespace LTCPro.ServiceLayer
{
    public class RoleService : IRoleService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger _log;
        private readonly IUserRepository _userRepository;
        private readonly IOrdersRepository _ordersRepository;
        public RoleService(IAutoMapper autoMapper, IRoleRepository roleRepository, ILogger log, IUserRepository userRepository, IOrdersRepository ordersRepository)
        {
            this._autoMapper = autoMapper;
            this._roleRepository = roleRepository;
            this._log = log;
            this._userRepository = userRepository;
            this._ordersRepository = ordersRepository;
        }
        public async Task<int> InsertRole(RoleEntity Role)
        {
            this._log.Debug("---Executing InsertRole() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.InsertRole(Role));
        }
        public async Task<RoleEntity> GetRoleDetailsByID(int RoleID)
        {
            this._log.Debug("---Executing GetRoleDetailsByID() in RoleService----");
            return await Task.FromResult<RoleEntity>(this._roleRepository.GetRoleDetailsByID(RoleID));
        }
        public async Task<List<RoleGridData>> GetRoleDetailsAll()
        {
            this._log.Debug("---Executing GetRoleDetailsAll() in RoleService----");
            return await Task.FromResult<List<RoleGridData>>(this._roleRepository.GetRoleDetailsAll());
        }
        public async Task<List<RoleEntity>> GetRoleDropData()
        {
            this._log.Debug("---Executing GetRoleDropData() in RoleService----");
            return await Task.FromResult<List<RoleEntity>>(this._roleRepository.GetRoleDropData());
        }
        public async Task<int> InsertRoleConfigMaster(RoleConfigEntity roleConfig)
        {
            this._log.Debug("---Executing InsertRoleConfigMaster() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.InsertRoleConfigMaster(roleConfig));
        }
        public async Task<List<RoleConfigGridEntity>> GetRoleConfigsGrid(int userId,int roleId)
        {
            this._log.Debug("---Executing GetRoleConfigsGrid() in RoleService----");
            return await Task.FromResult<List<RoleConfigGridEntity>>(this._roleRepository.GetRoleConfigsGrid(userId,roleId));
        }
        public async Task<List<RoleConfigEntity>> GetRoleConfigDetailsAll()
        {
            this._log.Debug("---Executing GetRoleConfigDetailsAll() in RoleService----");
            return await Task.FromResult<List<RoleConfigEntity>>(this._roleRepository.GetRoleConfigDetailsAll());
        }
        public async Task<int> InsertUserFacilityRoleConfig(UserRoleFacilityConfigCustomEntity userConfig)
        {
            this._log.Debug("---Executing InsertUserFacilityRoleConfig() in UserService----");
            return await Task.FromResult<int>(this._roleRepository.InsertUserFacilityRoleConfig(userConfig));
        }
        public async Task<List<UserRoleFacilityConfigEntity>> GetUserRoleFacilityConfigByID(int UserRole_Id)
        {
            this._log.Debug("---Executing GetUserRoleFacilityConfigByID() in RoleService----");
            return await Task.FromResult<List<UserRoleFacilityConfigEntity>>(this._roleRepository.GetUserRoleFacilityConfigByID(UserRole_Id));
        }
        public async Task<List<UserRoleFacilityConfigGridEntity>> UserRoleFacilityConfigGrid(UserRoleConfigIds Ids)
        {
            this._log.Debug("---Executing UserRoleFacilityConfigGrid(Ids) in RoleService----");
            return await Task.FromResult<List<UserRoleFacilityConfigGridEntity>>(this._roleRepository.UserRoleFacilityConfigGrid(Ids));
        }
        public async Task<UserRoleFacilityConfigCustomEntity> GetUserRoleFacilityConfigByRoleId(int roleId, int userId, int facilityId)
        {
            this._log.Debug("---Executing GetUserRoleFacilityConfigByRoleID() in RoleService----");
            return await Task.FromResult<UserRoleFacilityConfigCustomEntity>(this._roleRepository.GetUserRoleFacilityConfigByRoleId(roleId, userId, facilityId));
        }
        public async Task<List<ScreenPermissionsCustomEntity>> GetScreenPermissionsForLoggedInUser(int userId, int roleId)
        {
            this._log.Debug("---Executing GetScreenPermissionsForLoggedInUser() in RoleService----");
            return await Task.FromResult<List<ScreenPermissionsCustomEntity>>(this._roleRepository.GetScreenPermissionsForLoggedInUser(userId, roleId));
        }
        public async Task<List<ScreenEntity>> GetAllScreens(int screen)
        {
            this._log.Debug("---Executing GetAllScreens(int screen) in RoleService----");
            return await Task.FromResult<List<ScreenEntity>>(this._roleRepository.GetAllScreens(screen));
        }
        public async Task<string> GetRoleByUserId(int userId, int facilityId)
        {
            this._log.Debug("---Executing GetRoleByUserId() in RoleService----");
            return await Task.FromResult<string>(this._roleRepository.GetRoleByUserId(userId, facilityId));
        }
        public async Task<List<RoleConfigEntity>> GetRoleConfigDetailsByScreenID(RoleScreensEntity Ids)
        {
            this._log.Debug("---Executing GetRoleConfigDetailsByScreenID() in RoleService----");
            return await Task.FromResult<List<RoleConfigEntity>>(this._roleRepository.GetRoleConfigDetailsByScreenID(Ids));

        }
        public async Task<UserIdCustomEntity> GetUserEmailByUserName(string userName)
        {
            this._log.Debug("---Executing GetUserEmailByUserName() in UserService----");
            return await Task.FromResult<UserIdCustomEntity>(this._roleRepository.GetUserEmailByUserName(userName));
        }
        public int SendForgotPasswordMail(string userName, int? userId)
        {
            string FromMail= ConfigurationManager.AppSettings.GetValues("FromMail")[0].ToString();
            string mailPass = string.Empty;
            string otp = GenerateOTP("1");

            var userDetails = userId != 0 ? this._userRepository.GetUserDetailsByID((int)userId) : this._roleRepository.GetUserDetails(userName);

            var mailConfigData = this._roleRepository.GetMailconfigDetails(1);
            int result = this._roleRepository.InsertUserOTP(userDetails.User_Id, otp, 1);
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Controllers/HtmlTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName=="null"? userDetails.UserName : userName);
            body = body.Replace("{Text}", "Your Verification OTP is ");
            body = body.Replace("{OTP}", otp);
            if (userDetails != null && mailConfigData != null && result == 1)
            {

                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = "smtp1-mke.securence.com";
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(FromMail);
                 mail.To.Add(userDetails.User_Email);
                //mail.To.Add("bolten@augusta.edu");
                mail.Subject = userName == "null" ? "Change Password Request" :"Forgot Password Request";
                //mail.Body = "Your Password is '" + userDetails.Password + "'";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.EnableSsl = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Port = 587;
              
                SmtpServer.Send(mail);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public async Task<RoleConfigEntity> GetRoleConfigDetailsByID(int RoleConfig_Id)
        {
            this._log.Debug("---Executing GetRoleConfigDetailsByID() in RoleService----");
            return await Task.FromResult<RoleConfigEntity>(this._roleRepository.GetRoleConfigDetailsByID(RoleConfig_Id));
        }
        public async Task<List<UserRolesEntity>> GetRolesByUser(string userName, string password)
        {
            this._log.Debug("---Executing GetRolesByUser() in RoleService----");
            int status = this._userRepository.ValidateUser(userName, password);
            List<UserRolesEntity> litsobj = new List<UserRolesEntity>();
            UserRolesEntity role = new UserRolesEntity();
            int userId = 0;
            switch (status)
            {
                case 1:
                    userId = this._userRepository.GetUserId(userName, password);
                    if (userId == 0)
                        return null;
                    else
                        litsobj = await Task.FromResult<List<UserRolesEntity>>(this._roleRepository.GetRolesByUser(userId));
                    break;
                //case 6:
                //    userId = this._userRepository.GetUserId(userName, password);
                //    if (userId == 0)
                //        return null;
                //    else
                //        litsobj = await Task.FromResult<List<UserRolesEntity>>(this._roleRepository.GetRolesByUser(userId));
                //    break;
                case 2:
                    role.RoleId = -1;
                    role.RoleDesc = "The user locked, to Unlock ";
                    litsobj.Add(role);
                    break;
                case 3:
                    role.RoleId = -1;
                    role.RoleDesc = "The user name or password is incorrect";
                    litsobj.Add(role);
                    break;
                case 4:
                    role.RoleId = -1;
                    role.RoleDesc = "The user name or password is incorrect";
                    litsobj.Add(role);
                    break;
                case 5:
                    role.RoleId = -1;
                    role.RoleDesc = "No Role or Nursestation mapped";
                    litsobj.Add(role);
                    break;
                case 7:
                    role.RoleId = -1;
                    role.RoleDesc = "The user InActive. Contact Admin.";
                    litsobj.Add(role);
                    break;
            }
            return litsobj;
        }
        public string GenerateOTP(string type)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            if (type == "1")
            {
                characters += alphabets + small_alphabets + numbers;
            }
            int length = 6;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        public async Task<string> GetUserOTPCheckStatus(int UserId, string OTP)
        {
            this._log.Debug("---Executing GetUserOTPCheckStatus() in RoleService----");
            return await Task.FromResult<string>(this._roleRepository.GetUserOTPCheckStatus(UserId, OTP));
        }
        public async Task<string> GetLockOTPCheckStatus(int UserId, string OTP)
        {
            this._log.Debug("---Executing GetLockOTPCheckStatus() in RoleService----");
            return await Task.FromResult<string>(this._roleRepository.GetLockOTPCheckStatus(UserId, OTP));
        }
        public async Task<int> ResetPassword(int UserId, string PWD)
        {
            this._log.Debug("---Executing GetUserOTPCheckStatus() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.ResetPassword(UserId, PWD));
        }
        public async Task<int> UpdateNewUserpassword(string userName, string password, string newPassword)
        {
            this._log.Debug("---Executing UpdateNewUserpassword() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.UpdateNewUserpassword(userName, password, newPassword));
        }
        public async Task<int> UpdateRoleConfigsStatus(List<RoleConfigEntity> data)
        {
            this._log.Debug("---Executing UpdateRoleConfigsStatus() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.UpdateRoleConfigsStatus(data));
        }
        public async Task<int> UpdateRolesStatus(List<RoleEntity> data)
        {
            this._log.Debug("---Executing UpdateRolesStatus() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.UpdateRolesStatus(data));
        }
        public async Task<List<ScreenEntity>> GetDefaultScreenDropData()
        {
            this._log.Debug("---Executing GetDefaultScreenDropData() in RoleService----");
            return await Task.FromResult <List<ScreenEntity>> (this._roleRepository.GetDefaultScreenDropData());
        }
        public async Task<int> InsertDefaultScreen(DefaultScreenEntity data)
        {
            this._log.Debug("---Executing InsertDefaultScreen() in RoleService----");
            return await Task.FromResult<int>(this._roleRepository.InsertDefaultScreen(data));
        }

        public async Task<List<RoleDropdownEntity>> GetSubRolesList()
        {
            this._log.Debug("---Executing GetSubRolesList() in RoleService----");
            return await Task.FromResult <List<RoleDropdownEntity>> (this._roleRepository.GetSubRolesList());
        }

        public async Task<List<ScreenEntity>> GetSubRolesScreenList(int roleID,string status)
        {
            this._log.Debug("---Executing GetSubRolesScreenList() in RoleService----");
            return await Task.FromResult<List<ScreenEntity>>(this._roleRepository.GetSubRolesScreenList(roleID,status));
        }
        public async Task<MailConfigEntity> GetMailconfigDetails(int id)
        {
            this._log.Debug("---Executing GetMailconfigDetails() in RoleService----");
            return await Task.FromResult<MailConfigEntity>(this._roleRepository.GetMailconfigDetails(id));
        }
    }
}