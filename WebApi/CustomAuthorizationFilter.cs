using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using LTCPro.Repositories;
using LTCPro.ServiceLayer;
using LTCPro.DAL;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuthorizationFilter : AuthorizationFilterAttribute
    {
        private const string _authorizedToken = "Authorization";
        private const string _userAgent = "User-Agent";

        //private UserAuthorizations objAuth = null;

        public override async void OnAuthorization(HttpActionContext filterContext)
        {
            string authorizedToken = string.Empty;
            string userAgent = string.Empty;

            try
            {
                var headerToken = filterContext.Request.Headers.SingleOrDefault
                                      (x => x.Key == _authorizedToken);
                if (headerToken.Key != null)
                {
                    authorizedToken = Convert.ToString(headerToken.Value.SingleOrDefault());
                    userAgent = Convert.ToString(filterContext.Request.Headers.UserAgent);
                    var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
                    if (claimsIdentity.Claims.Count() > 0)
                    {
                        string userId = claimsIdentity.FindFirst("UserId").Value;
                        if (userId != "")
                        {
                            bool isAuthorize = await IsAuthorize(authorizedToken, Convert.ToInt32(userId));
                            if (!isAuthorize)
                            {
                                var mapper = new AutoMapping();
                                SQLHelper _dbContext = new SQLHelper();
                                UserService userService = new UserService(mapper, new UserRepository(mapper, new EMAREntities(), new UserActivityRepository(mapper, new EMAREntities()), _dbContext), new Log4net());
                                int res = await userService.AutoLogoutUser(Convert.ToInt32(userId));
                                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                                return;
                            }
                        }
                        else
                        {
                            filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                            return;
                        }
                    }
                    else
                    {
                        filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        return;
                    }
                }
                else
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }
            }
            catch (Exception ex)
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }

            base.OnAuthorization(filterContext);
        }

        private async Task<bool> IsAuthorize(string authorizedToken, int userId)
        {
            //objAuth = new UserAuthorizations();
            var mapper = new AutoMapping();
           SQLHelper _dbContext = new SQLHelper();
            UserService userService = new UserService(mapper, new UserRepository(mapper, new EMAREntities(),new UserActivityRepository(mapper, new EMAREntities()), _dbContext), new Log4net());
            string token = await userService.GetUserToken(userId);
            bool result = false;
            try
            {
                if (authorizedToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    string authToken = authorizedToken.Substring("Bearer ".Length).Trim();
                    if (authToken == token)
                        result = true;
                }
                // objAuth.ValidateToken(authorizedToken, userAgent);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

    }
}