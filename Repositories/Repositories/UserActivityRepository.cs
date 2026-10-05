using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using System.Web;
using System.Security.Claims;
using LTCPro.Entities;

namespace LTCPro.Repositories
{
    public class UserActivityRepository:IUserActivityRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        public UserActivityRepository(IAutoMapper autoMapper, IDbContextEmar dbContext)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
        }
        //public Int64 InsertUserSession(int userId, string systemIp, string browserName)
        //{
        //    UserSession userObj = new UserSession();
        //    userObj.user_Id = userId;
        //    userObj.LoginTime = DateTime.Now;
        //    userObj.Session_Status = 1;
        //    userObj.SystemIP = systemIp;
        //    userObj.BrowserName = browserName;
        //    this.dbContext.UserSessions.Add(userObj);
        //    this.dbContext.SaveChanges();
        //    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //    var identity = new ClaimsIdentity(claimsIdentity);
        //    identity.AddClaim(new Claim("SessionId", userObj.Session_Id.ToString()));

        //    ClaimsPrincipal principal = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
        //    principal.AddIdentity(identity);
        //    //var customClaimValue = principal.Claims.Where(c => c.Type == "CompanyID").Single().Value;

        //    //HttpContext.Current.Session["SessionId"] = userObj.Session_Id;
        //    return userObj.Session_Id;            
        //}
        public int InsertUserActivityDetails(UserActivityDetailEntity entity)
        {
            //ClaimsPrincipal principal = System.Threading.Thread.CurrentPrincipal as ClaimsPrincipal;
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            string sessionId = claimsIdentity.FindFirst("SessionId").Value;
            if (sessionId != "")
            {
                entity.Session_Id = Convert.ToInt32(sessionId);
                var record = this.autoMapper.Map<UserActivityDetailEntity, UserActivityDetail>(entity);                
                this.dbContext.UserActivityDetails.Add(record);
                this.dbContext.SaveChanges();
                return 1;
            }
            return 0;
        }
    }
}
