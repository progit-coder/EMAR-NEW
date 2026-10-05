using LTCPro.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity.WebApi;
using WebApi.Providers;
using System.Web.Cors;
using System.Threading.Tasks;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(WebApi.Startup))]
namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.DependencyResolver = new UnityDependencyResolver(UnityConfig.Container);
            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                SupportsCredentials = true,

            };

            var appSettings = WebConfigurationManager.AppSettings;

            // If CORS settings are present in Web.config
            if (!string.IsNullOrWhiteSpace(appSettings["cors:Origins"]))
            {

                //policy.Origins.Add("http://localhost:4200"); //be sure to include the port:
                //example: "http://localhost:8081"

                var origins = appSettings["cors:Origins"].Split(',');
                foreach (var item in origins)
                {
                    policy.Origins.Add(item);
                }

                app.UseCors(new CorsOptions
                {
                    PolicyProvider = new CorsPolicyProvider
                    {
                        PolicyResolver = context => Task.FromResult(policy)
                    }
                });

            }
            //app.UseCors(CorsOptions.AllowAll);
            ConfigureOAuth(app);
            app.UseWebApi(config);
            SwaggerConfig.Register(config);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),//.FromMinutes(60),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}