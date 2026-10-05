using LTCPro.DAL;
using Microsoft.Owin.Security.OAuth;
using LTCPro.Repositories;
using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;

namespace WebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            Microsoft.Owin.IFormCollection parameters = await context.Request.ReadFormAsync();
            var loggedin = parameters.Get("loggedin");
            var userIp = parameters.Get("ip");
            var browser = parameters.Get("browser");
            //var role = parameters.Get("role");
            if (loggedin == null || userIp == null || browser == null)// || role == null)
            {
                context.SetError("InvalidRequest", "Invalid Request");
                return;
            }
            //int roleId = Convert.ToInt32(role);
            //context.OwinContext.Response.Headers.SetValues("Access-Control-Allow-Origin", "http://localhost:51810/");
            var mapper = new AutoMapping();
            SQLHelper _dbContext = new SQLHelper();
            UserService userService = new UserService(mapper, new UserRepository(mapper, new EMAREntities(), new UserActivityRepository(mapper, new EMAREntities()), _dbContext), new Log4net());
            int status = await userService.CheckUser(context.UserName, context.Password);
            switch (status)
            {
                case 1:

                    await GenerateToken(context, userService, userIp, browser);
                    break;
                case 2:
                    context.SetError("User Locked", "The user locked, to Unlock ");
                    //context.Rejected();
                    break;
                case 3:
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    //context.Rejected();
                    break;
                case 4:
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    //context.Rejected();
                    break;
                case 5:
                    context.SetError("invalid_Role", "Role or Nursestation not mapped.");
                    //context.Rejected();
                    break;
                case 6:
                    context.SetError("UserInactive", "The user is Inactive. Contact Admin.");
                    //context.Rejected();
                    break;
                case 7:
                    context.SetError("NewUser", "New User Change Password");
                    //context.Rejected();
                    break;
                case 8:
                    var isLoggedIn = Convert.ToInt32(loggedin);
                    if (isLoggedIn == 0)
                        context.SetError("userLoggedIn", "User already logged in.");
                    // context.Rejected();
                    else if (isLoggedIn == 1)
                    {
                        var user = await userService.FindUser(context.UserName, context.Password);
                        await userService.LogoutUser(user.User_Id);
                        await GenerateToken(context, userService, userIp, browser);
                    }
                    break;
            }
        }

        private static async Task GenerateToken(OAuthGrantResourceOwnerCredentialsContext context, UserService userService, string userIp, string browser)
        {
            var user = await userService.FindUser(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("UnAuthorized", "Contact Admin.");
                context.Rejected();
            }
            Int64 sessionId = await userService.InsertUserSession(user.User_Id, user.RoleId, userIp, browser);

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("Username", user.UserName));
            identity.AddClaim(new Claim("Email", user.User_Email));
            identity.AddClaim(new Claim("UserId", user.User_Id.ToString()));
            identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
            identity.AddClaim(new Claim("SessionId", sessionId.ToString()));
            identity.AddClaim(new Claim("RoleId", user.RoleId.ToString()));

            identity.AddClaim(new Claim(ClaimTypes.Role, user.RoleName));

            var additionalData = new AuthenticationProperties(new Dictionary<string, string>{
                    {
                    "displayname", Newtonsoft.Json.JsonConvert.SerializeObject(user.User_DisplayName)
                    },
                    {
                    "userid", Newtonsoft.Json.JsonConvert.SerializeObject(user.User_Id)
                    },
                    {
                    "role", Newtonsoft.Json.JsonConvert.SerializeObject(user.RoleName)
                    },
                    {
                    "roleid", Newtonsoft.Json.JsonConvert.SerializeObject(user.RoleId)
                    },
                    {
                    "screen", Newtonsoft.Json.JsonConvert.SerializeObject(user.DefaultScreenId)
                    }
                    });
            var token = new AuthenticationTicket(identity, additionalData);
            //context.Ticket.Identity.
            if (context.Validated(token))
            {
                //read token
                // inserttoken into DB
                // insert usersession
                // store sessionid in claim
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            var token = context.AccessToken;
            var mapper = new AutoMapping();
            SQLHelper _dbContext = new SQLHelper();
            UserService userService = new UserService(mapper, new UserRepository(mapper, new EMAREntities(), new UserActivityRepository(mapper, new EMAREntities()), _dbContext), new Log4net());
            int userId = Convert.ToInt32(context.Identity.FindFirst("UserId").Value);
            Task<int> result = userService.InsertUserToken(userId, token);

            return base.TokenEndpointResponse(context);
        }
    }
}