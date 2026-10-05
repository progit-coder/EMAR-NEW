using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Filters;
using LTCPro.ServiceLayer;
using LTCPro.Entities;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("UserActivity")]
    [CustomAuthorizationFilter]
    public class UserActivityController : ApiController
    {
        private readonly IUserActivityService _userActivityService;
        private readonly ILogger _log;
        public UserActivityController(IUserActivityService userActivityService, ILogger log)
        {
            this._userActivityService = userActivityService;
            this._log = log;
        }
        //[Route("InsertUserSession/{userId}/{systemIp}/{browserName}")]
        //[HttpGet]
        //public Int64 InsertUserSession(int userId,string systemIp, string browserName)
        //{
        //    this._log.Debug("---Executing InsertUserSession() in CompanyController----");
        //    var companies = this._userActivityService.InsertUserSession(userId, systemIp, browserName).Result;
        //    this._log.Debug("---Executed Successfully InsertUserSession() in CompanyController----");
        //    return companies;
        //}

        [Route("InsertUserActivityDetails")]
        [HttpPost]
        public int InsertUserActivityDetails(UserActivityDetailEntity entity)
        {
            entity.Time = DateTime.Now;
            this._log.Debug("---Executing InsertUserActivityDetails() in CompanyController----");
            var result = this._userActivityService.InsertUserActivityDetails(entity).Result;
            this._log.Debug("---Executed Successfully InsertUserActivityDetails() in CompanyController----");
            return result;
        }
        //[Route("GetIP")]
        //[HttpGet]
        //public string GetIP()
        //{
        //    string strHostName = "";
        //    strHostName = System.Net.Dns.GetHostName();

        //    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

        //    IPAddress[] addr = ipEntry.AddressList;

        //    return addr[addr.Length - 1].ToString();

        //}
    }
}