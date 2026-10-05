using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using LTCPro.Entities;
using LTCPro.ServiceLayer;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [Authorize]
    [RoutePrefix("UserMaster")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ICommonService _commonservice;
        private readonly ILogger _log;
        public UserController(IUserService userService, ILogger log, ICommonService commonservice)
        {
            this._userService = userService;
            this._commonservice = commonservice;
            this._log = log;
        }
        [Route("GetUsersMasterGrid/{UserId}")]
        [HttpGet]
        public JsonResult<IList> GetUsersMasterGrid(int UserId)
        {
            try
            {
                this._log.Debug("---Executing GetUsersMasterGrid() in UserController----");
                var users = this._userService.GetUsersMasterGrid(UserId).Result;
                this._log.Debug("---Executed Successfully GetUsersMasterGrid() in UserController----");
                return Json<IList>(users);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("GetUserDropData")]
        [HttpGet]
        public JsonResult<List<UserDropEntity>> GetUserDropData()
        {
            this._log.Debug("---Executing GetUserDropData() in UserController----");
            var users = this._userService.GetUserDropData().Result;
            this._log.Debug("---Executed Successfully GetUserDropData() in UserController----");
            return Json<List<UserDropEntity>>(users);
        }
        [Route("GetUserDetailsByID/{UserId}")]
        [HttpGet]
        public JsonResult<UserEntity> GetUserDetailsByID(int userId)
        {
            this._log.Debug("---Executing GetUserDetailsByID() in UserController----");
            var user = this._userService.GetUserDetailsByID(userId).Result;
            this._log.Debug("---Executed Successfully GetUserDetailsByID() in UserController----");
            return Json<UserEntity>(user);
        }

        [Route("InsertUser")]
        [HttpPost]
        public int InsertUser(UserEntity entity)
        {
            this._log.Debug("---Executing Insert/Update() in UserController----");
            int result = this._userService.InsertUpdateUserMaster(entity).Result;
            this._log.Debug("---Executed Successfully Insert/Update() in UserController----");
            return result;
        }

        [Route("LogoutUser/{UserId}")]
        [HttpGet]
        public int LogoutUser(int userId)
        {
            this._log.Debug("---Executing LogoutUser() in UserController----");
            int result = this._userService.LogoutUser(userId).Result;
            this._log.Debug("---Executed Successfully LogoutUser() in UserController----");
            return result;
        }
        [Route("InsertUpdateUserRecentFacNs/{userId}/{NurseStation_Id}")]
        [HttpGet]
        public int InsertUpdateUserRecentFacNs(int userId, string NurseStation_Id)
        {
            this._log.Debug("---Executing InsertUpdateUserRecentFacNs() in UserController----");
            var users = this._commonservice.GetUserRecentFacNs(userId).Result;

            //  RecentFacEntity Obj;
            if (users != null)
            {


                users.NurseStation_Id = NurseStation_Id;
                users.Facility_Id = 0;
                int result = this._commonservice.InsertUpdateUserRecentFacNs(users).Result;
                this._log.Debug("---Executed Successfully InsertUpdateUserRecentFacNs() in UserController----");
                return result;
            }
            else
            {
                RecentFacEntity obj = new RecentFacEntity();
                obj.NurseStation_Id = NurseStation_Id;
                obj.Facility_Id = 0;
                int result = this._commonservice.InsertUpdateUserRecentFacNs(obj).Result;
                this._log.Debug("---Executed Successfully InsertUpdateUserRecentFacNs() in UserController----");
                return result;
                // return result;
            }



        }

        //[Route("InsertUserToken")]
        //[HttpPost]
        //public int InsertUserToken(string body)
        //{
        //    UserEntity entity = Newtonsoft.Json.JsonConvert.DeserializeObject<UserEntity>(body);
        //    this._log.Debug("---Executing InsertUserToken in UserController----");
        //    int result = this._userService.InsertUserToken(entity.User_Id, entity.User_Token).Result;
        //    this._log.Debug("---Executed Successfully Insert/Update() in UserController----");
        //    return result;
        //}
        [Route("GetStockReportUserDropData")]
        [HttpGet]
        public JsonResult<List<UserEntity>> GetStockReportUserDropData()
        {
            this._log.Debug("---Executing GetStockReportUserDropData() in UserController----");
            var users = this._userService.GetStockReportUserDropData().Result;
            this._log.Debug("---Executed Successfully GetStockReportUserDropData() in UserController----");
            return Json<List<UserEntity>>(users);
        }
        [Route("UpdateUsersStatus")]
        [HttpPost]
        public int UpdateUsersStatus(List<UserEntity> data)
        {
            this._log.Debug("---Executing UpdateUsersStatus() in UserController----");
            var result = this._userService.UpdateUsersStatus(data).Result;
            this._log.Debug("---Executed Successfully UpdateUsersStatus() in UserController----");
            return result;
        }
        [Route("ResetUserPwdByAdmin")]
        [HttpPost]
        public int ResetUserPwdByAdmin(UserEntity Obj)
        {
            this._log.Debug("---Executing ResetUserPwdByAdmin() in UserController----");
            var result = this._userService.ResetUserPwdByAdmin(Obj).Result;
            this._log.Debug("---Executed Successfully ResetUserPwdByAdmin() in UserController----");
            return result;
        }

        [Route("GetUsersByNSID/{nS_Id}")]
        [HttpGet]
        public JsonResult<List<UserDropEntity>> GetUsersByNSID(int nS_Id)
        {
            this._log.Debug("---Executing GetUsersByNSID() in UserController----");
            var users = this._userService.GetUsersByNSID(nS_Id).Result;
            this._log.Debug("---Executed Successfully GetUsersByNSID() in UserController----");
            return Json<List<UserDropEntity>>(users);
        }
    }
}
