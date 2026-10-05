using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using LTCPro.Entities;
using System.Web.Http.Results;
using WebApi.Filters;
namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Role")]
    //[CustomAuthorizationFilter]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;
        private readonly IFacilityService _facilityService;
        private readonly ILogger _log;

        public RoleController(IRoleService roleService, ILogger log, IFacilityService facilityService)
        {
            this._roleService = roleService;
            this._facilityService = facilityService;
            this._log = log;
        }
        [Route("InsertRole")]
        [HttpPost]
        public int InsertRole(RoleEntity Role)
        {
            this._log.Debug("---Executing Insert/Update() in RoleController----");
            int result = this._roleService.InsertRole(Role).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in RoleController----");
            return result;
        }
        [Route("GetRoleDetailsByID/{RoleID}")]
        [HttpGet]
        public JsonResult<RoleEntity> GetRoleDetailsByID(int RoleID)
        {
            this._log.Debug("---Executing GetRoleDetailsByID() in RoleController----");
            var role = this._roleService.GetRoleDetailsByID(RoleID).Result;
            this._log.Debug("---Executed Successfully GetRoleDetailsByID() in RoleController----");
            return Json<RoleEntity>(role);
        }
        [Route("GetRoleDetailsAll")]
        [HttpGet]
        public JsonResult<List<RoleGridData>> GetRoleDetailsAll()
        {
            this._log.Debug("---Executing GetRoleDetailsAll() in RoleController----");
            var role = this._roleService.GetRoleDetailsAll().Result;
            this._log.Debug("---Executed Successfully GetRoleDetailsAll() in RoleController----");
            return Json<List<RoleGridData>>(role);
        }
        [Route("GetRoleDropData")]
        [HttpGet]
        public JsonResult<List<RoleEntity>> GetRoleDropData()
        {
            this._log.Debug("---Executing GetRoleDropData() in RoleController----");
            var role = this._roleService.GetRoleDropData().Result;
            this._log.Debug("---Executed Successfully GetRoleDropData() in RoleController----");
            return Json<List<RoleEntity>>(role);
        }

        [Route("InsertRoleConfigMaster")]
        [HttpPost]
        public int InsertRoleConfigMaster(RoleConfigEntity roleConfig)
        {
            this._log.Debug("---Executing Insert/Update() in RoleController----");
            int result = this._roleService.InsertRoleConfigMaster(roleConfig).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in RoleController----");
            return result;
        }

        [Route("GetRoleConfigsGrid/{userId}/{roleId}")]
        [HttpGet]
        public JsonResult<List<RoleConfigGridEntity>> GetRoleConfigsGrid(int userId,int roleId)
        {
            this._log.Debug("---Executing GetRoleConfigsGrid() in RoleController----");
            var role = this._roleService.GetRoleConfigsGrid(userId,roleId).Result;
            this._log.Debug("---Executed Successfully GetRoleConfigsGrid() in RoleController----");
            return Json<List<RoleConfigGridEntity>>(role);
        }
        [Route("GetRoleConfigDetailsAll")]
        [HttpGet]
        public JsonResult<List<RoleConfigEntity>> GetRoleConfigDetailsAll()
        {
            this._log.Debug("---Executing GetRoleConfigDetailsAll() in RoleController----");
            var role = this._roleService.GetRoleConfigDetailsAll().Result;
            this._log.Debug("---Executed Successfully GetRoleConfigDetailsAll() in RoleController----");
            return Json<List<RoleConfigEntity>>(role);
        }
        [Route("InsertUserFacilityRoleConfig")]
        [HttpPost]
        public int InsertUserFacilityRoleConfig(UserRoleFacilityConfigCustomEntity userConfig)
        {
            this._log.Debug("---Executing InsertUserFacilityRoleConfig() in UserController----");
            int userrolecnf = this._roleService.InsertUserFacilityRoleConfig(userConfig).Result;
            this._log.Debug("---Executed Successfully InsertUserFacilityRoleConfig() in UserController----");
            return userrolecnf;
        }
        [Route("GetUserRoleFacilityConfigByID/{UserRole_Id}")]
        [HttpGet]
        public JsonResult<List<UserRoleFacilityConfigEntity>> GetUserRoleFacilityConfigByID(int UserRole_Id)
        {
            this._log.Debug("---Executing GetUserRoleFacilityConfigByID() in RoleController----");
            var userRole = this._roleService.GetUserRoleFacilityConfigByID(UserRole_Id).Result;
            this._log.Debug("---Executed Successfully GetUserRoleFacilityConfigByID() in RoleController----");
            return Json<List<UserRoleFacilityConfigEntity>>(userRole);
        }      
        [Route("UserRoleFacilityConfigGrid")]
        [HttpPost]
        public JsonResult<List<UserRoleFacilityConfigGridEntity>> UserRoleFacilityConfigGrid(UserRoleConfigIds Ids)
        
{
            this._log.Debug("---Executing UserRoleFacilityConfigGrid(Ids) in RoleController----");
            var details = this._roleService.UserRoleFacilityConfigGrid(Ids).Result;
            this._log.Debug("---Executing successfully UserRoleFacilityConfigGrid(Ids) in Role Controller----");
            return Json<List<UserRoleFacilityConfigGridEntity>>(details);
        }
        [Route("GetUserRoleFacilityConfigByRoleID/{roleId}/{userId}/{facilityId}")]
        [HttpGet]
        public JsonResult<UserRoleFacilityConfigCustomEntity> GetUserRoleFacilityConfigByRoleId(int roleId, int userId,int facilityId)
        {
            this._log.Debug("---Executing GetUserRoleFacilityConfigByRoleId() in RoleController----");
            var data = this._roleService.GetUserRoleFacilityConfigByRoleId(roleId, userId,facilityId).Result;
            this._log.Debug("---Executed Successfully GetUserRoleFacilityConfigByRoleID() in RoleController----");
            return Json<UserRoleFacilityConfigCustomEntity>(data);
        }
        [Route("GetScreenPermissions/{UserId}/{roleId}")]
        [HttpGet]
        public JsonResult<List<ScreenPermissionsCustomEntity>> GetScreenPermissionsForLoggedInUser(int userId, int roleId)
        {
            this._log.Debug("---Executing GetScreenPermissionsForLoggedInUser() in RoleController----");
            var screenPermissions = this._roleService.GetScreenPermissionsForLoggedInUser(userId, roleId).Result;
            this._log.Debug("---Executed Successfully GetScreenPermissionsForLoggedInUser() in RoleController----");
            return Json<List<ScreenPermissionsCustomEntity>>(screenPermissions);
        }
        [Route("GetScreens/{screen}")]
        [HttpGet]
        public JsonResult<List<ScreenEntity>> GetAllScreens(int screen)
        {
            this._log.Debug("---Executing GetAllScreens() in RoleController----");
            var screens = this._roleService.GetAllScreens(screen).Result;
            this._log.Debug("---Executed Successfully GetScreenPermissionsForLoggedInUser() in RoleController----");
            return Json<List<ScreenEntity>>(screens);
        }
        [Route("GetSubRolesScreenList/{roleID}/{status?}")]
        [HttpGet]
        public JsonResult<List<ScreenEntity>> GetSubRolesScreenList(int roleID,string status = "")
        {
            this._log.Debug("---Executing GetSubRolesScreenList() in RoleController----");
            var screens = this._roleService.GetSubRolesScreenList(roleID, status).Result;
            this._log.Debug("---Executed Successfully GetScreenPermissionsForLoggedInUser() in RoleController----");
            return Json<List<ScreenEntity>>(screens);
        }
        [Route("GetRoleConfigDetailsByScreenID")]
        [HttpPost]
        public JsonResult<List<RoleConfigEntity>> GetRoleConfigDetailsByScreenID(RoleScreensEntity Ids)
        {
            this._log.Debug("---Executing GetRoleConfigDetailsByScreenID() in RoleController----");
            var result = this._roleService.GetRoleConfigDetailsByScreenID(Ids).Result;
            this._log.Debug("---Executed Successfully GetRoleConfigDetailsByScreenID() in RoleController----");
            return Json<List<RoleConfigEntity>>(result);

        }
        [Route("GetUserEmailByUserName/{userName}")]
        [HttpGet]
        public JsonResult<UserIdCustomEntity> GetUserEmailByUserName(string userName)
        {
            this._log.Debug("---Executing GetUserEmailByUserName() in UserController----");
            var mailObj = this._roleService.GetUserEmailByUserName(userName).Result;
            this._log.Debug("---Executed Successfully GetUserEmailByUserName() in UserController----");
            return Json<UserIdCustomEntity>(mailObj);
        }

        [Route("SendForgotPasswordMail/{userName}/{userId}")]
        [HttpGet]
        public int SendForgotPasswordMail(string userName, int? userId)
        {
            try
            {
                this._log.Debug("---Executing SendForgotPasswordMail() in UserController----");
                var result = this._roleService.SendForgotPasswordMail(userName, userId);
                this._log.Debug("---Executed Successfully SendForgotPasswordMail() in UserController----");
                return result;
            }
            catch(Exception ex)
            {
                this._log.Debug("---Error Mail Successfully SendForgotPasswordMail() in UserController----"+ ex.Message.ToString());
                this._log.Debug("---Error Mail Successfully SendForgotPasswordMail() in UserController----" + ex.InnerException.ToString());
                return 0;
            }
          
        }
        [Route("GetRoleConfigDetailsByID/{RoleConfig_Id}")]
        [HttpGet]
        public JsonResult<RoleConfigEntity> GetRoleConfigDetailsByID(int RoleConfig_Id)
        {

            this._log.Debug("---Executing GetRoleConfigDetailsByID() in RoleController----");
            var roleconfigdata = this._roleService.GetRoleConfigDetailsByID(RoleConfig_Id).Result;
            this._log.Debug("---Executed Successfully GetRoleConfigDetailsByID() in RoleController----");
            return Json<RoleConfigEntity>(roleconfigdata);
        }
        [Route("GetRolesByUser/{userName}/{password}")]
        [HttpGet]
        public JsonResult<List<UserRolesEntity>> GetRolesByUser(string userName, string password)
        {
            this._log.Debug("---Executing GetRolesByUser() in RoleController----");
            var result = this._roleService.GetRolesByUser(userName, password).Result;
            this._log.Debug("---Executed Successfully GetRolesByUser() in RoleController----");
            return Json<List<UserRolesEntity>>(result);
        }
        [Route("GetUserOTPCheckStatus/{UserId}/{OTP}")]
        [HttpGet]
        public string GetUserOTPCheckStatus(int UserId,string OTP)
        {
            UserOTPCheckEntity userOTPCheck = new UserOTPCheckEntity();
            userOTPCheck.UserId = UserId;
            userOTPCheck.OTP = OTP;
            this._log.Debug("---Executing GetUserOTPCheckStatus() in UserController----");
            string result = this._roleService.GetUserOTPCheckStatus(UserId,OTP).Result;
            this._log.Debug("---Executed Successfully GetUserOTPCheckStatus() in UserController----");
            return result;
        }
        [Route("GetLockOTPCheckStatus/{UserId}/{OTP}")]
        [HttpGet]
        public string GetLockOTPCheckStatus(int UserId, string OTP)
        {
            UserOTPCheckEntity userOTPCheck = new UserOTPCheckEntity();
            userOTPCheck.UserId = UserId;
            userOTPCheck.OTP = OTP;
            this._log.Debug("---Executing GetLockOTPCheckStatus() in UserController----");
            string result = this._roleService.GetLockOTPCheckStatus(UserId, OTP).Result;
            this._log.Debug("---Executed Successfully GetLockOTPCheckStatus() in UserController----");
            return result;
        }
        [Route("ResetPassword/{UserId}/{PWD}")]
        [HttpGet]
        public int ResetPassword(int UserId, string PWD)
        {
            this._log.Debug("---Executing ResetPassword() in RoleController----");
            int result = this._roleService.ResetPassword(UserId, PWD).Result;
            this._log.Debug("---Executed Successfully ResetPassword() in RoleController----");
            return result;
        }
        [Route("UpdateNewUserpassword/{userName}/{password}/{newPassword}")]
        [HttpGet]
        public int UpdateNewUserpassword(string userName, string password, string newPassword)
        {
            this._log.Debug("---Executing UpdateNewUserpassword() in RoleController----");
            var result = this._roleService.UpdateNewUserpassword(userName, password, newPassword).Result;
            this._log.Debug("---Executed Successfully UpdateNewUserpassword() in RoleController----");
            return result;
        }
        [Route("UpdateRoleConfigsStatus")]
        [HttpPost]
        public int UpdateRoleConfigsStatus(List<RoleConfigEntity> data)
        {
            this._log.Debug("---Executing UpdateRoleConfigsStatus() in RoleController----");
            var result = this._roleService.UpdateRoleConfigsStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateRoleConfigsStatus() in RoleController----");
            return result;
        }
        [Route("UpdateRolesStatus")]
        [HttpPost]
        public int UpdateRolesStatus(List<RoleEntity> data)
        {
            this._log.Debug("---Executing UpdateRolesStatus() in RoleController----");
            var result = this._roleService.UpdateRolesStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateRolesStatus() in RoleController----");
            return result;
        }
        [Route("GetDefaultScreenDropData")]
        [HttpGet]
        public JsonResult<List<ScreenEntity>> GetDefaultScreenDropData()
        {
            this._log.Debug("---Executing GetDefaultScreenDropData() in RoleController----");
            var screens = this._roleService.GetDefaultScreenDropData().Result;
            this._log.Debug("---Executed Successfully GetDefaultScreenDropData() in RoleController----");
            return Json<List<ScreenEntity>>(screens);
        }
        [Route("InsertDefaultScreen")]
        [HttpPost]
        public int InsertDefaultScreen(DefaultScreenEntity data)
        {
            this._log.Debug("---Executing InsertDefaultScreen() in RoleController----");
            var result = this._roleService.InsertDefaultScreen(data).Result;
            this._log.Debug("---Executed Successfully InsertDefaultScreen() in RoleController----");
            return result;
        }
        [Route("GetSubRolesList")]
        [HttpGet]
        public JsonResult<List<RoleDropdownEntity>> GetSubRolesList()
        {
            this._log.Debug("---Executing GetSubRolesList() in RoleController----");
            var role = this._roleService.GetSubRolesList().Result;
            this._log.Debug("---Executed Successfully GetSubRolesList() in RoleController----");
            return Json<List<RoleDropdownEntity>>(role);
        }
        [Route("UsersConfigNurseStationDrop/{username}/{password}")]
        [HttpGet]
        public JsonResult<List<NurseStationDropEntity>> UsersConfigNurseStationDrop(string username, string password)
        {
            try
            {
                this._log.Debug("---Executing UsersConfigNurseStationDrop() in FacilityController----");
                var result = this._facilityService.UsersConfigNurseStationDrop(username, password).Result;
                this._log.Debug("---Executed Successfully UsersConfigNurseStationDrop() in FacilityController----");
                return Json<List<NurseStationDropEntity>>(result);
            }
            catch(Exception ex)
            {
                return null;
            }
          
        }
        [Route("GetProcessKeyMasterList/{nsId?}")]
        [HttpGet]
        public JsonResult<List<ProcessMasterEntity>> GetProcessKeyMasterList(int? nsId = null)
        {
            this._log.Debug("---Executing GetProcessKeyMasterList() in FacilityController----");
            var result = this._facilityService.GetProcessKeyMasterList(nsId).Result;
            this._log.Debug("---Executed Successfully GetProcessKeyMasterList() in FacilityController----");
            return Json<List<ProcessMasterEntity>>(result);
        }
        [Route("GetMailconfigDetails/{id}")]
        [HttpGet]
        public JsonResult<MailConfigEntity> GetMailconfigDetails(int id)
        {
            this._log.Debug("---Executing GetMailconfigDetails() in RoleController----");
            var result = this._roleService.GetMailconfigDetails(id).Result;
            this._log.Debug("---Executed Successfully GetMailconfigDetails() in RoleController----");
            return Json<MailConfigEntity>(result);
        }
    }
}
  