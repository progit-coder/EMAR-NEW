using LTCPro.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace LTCPro.WebApi.Controllers
{
    public class CommonRcopiaController : ApiController
    {
        private readonly IRcopiaService _IRcopiaService;
        private readonly ILogger _log;
    

        public CommonRcopiaController(IRcopiaService IRcopiaService, ILogger log)
        {
            this._IRcopiaService = IRcopiaService;
            this._log = log;
          
        }

        public async Task<string> PostDrFirstTest(string Request,Int32 Id)
        {

            try
            {

                HttpClient httpClient = new HttpClient();

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //  string sampledata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.19'><Caller><VendorName>avendor2728</VendorName><VendorPassword>mr41zw7k</VendorPassword></Caller><SystemName>avendor2728</SystemName><RcopiaPracticeUsername>ph6214</RcopiaPracticeUsername><Request><Command>update_allergy</Command><LastUpdateDate>05/10/2023 06:22:03</LastUpdateDate><ReturnAllNDCIDs>y</ReturnAllNDCIDs><Patient><RcopiaID>26155609419</RcopiaID><ExternalID>abingdonperseus</ExternalID></Patient></Request></RCExtRequest>";


                var content = new FormUrlEncodedContent(new[]
                {
    new KeyValuePair<string, string>("xml", Request)
});
                var response = await httpClient.PostAsync("https://update201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet", content).ConfigureAwait(false);

                string responseText = await response.Content.ReadAsStringAsync();

                var data = this._IRcopiaService.InsertXMLData(Id,responseText).Result;

                return responseText;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }

        [HttpGet]
        public async Task<JsonResult<string>> GetDrFirstTest()
        {

            try
            {
                HttpClient client = new HttpClient();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client.BaseAddress = new Uri("https://engine201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet");

                string sampledata = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.19'><Caller><VendorName>avendor2728</VendorName >< VendorPassword > mr41zw7k </ VendorPassword ></ Caller >< SystemName > avendor2728 </ SystemName >< RcopiaPracticeUsername > ph6214 </ RcopiaPracticeUsername >< Request > ";

                client.BaseAddress = new Uri("https://engine201.staging.drfirst.com/servlet/rcopia.servlet.EngineServlet?xml=" + sampledata);



                HttpResponseMessage Res = client.GetAsync("").Result;


                if (Res.IsSuccessStatusCode)
                {

                    var Response = Res.Content.ReadAsStringAsync().Result;
                    // BulkStatusRoot data = JsonConvert.DeserializeObject<BulkStatusRoot>(Response);


                }
                else
                {

                }
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }

        }


        public string CreateMD5(string input = "")
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // string Key =  Convert.ToString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to.NET 5
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                string Key = sb.ToString();
                return Key;


            }
        }

        public string gmtTime()
        {
            DateTime gmtTime = DateTime.UtcNow;
            string month = gmtTime.Month.ToString("00");
            string day = gmtTime.Day.ToString("00");
            string year = (gmtTime.Year % 100).ToString("00");
            string hour = gmtTime.Hour.ToString("00");
            string min = gmtTime.Minute.ToString("00");
            string sec = gmtTime.Second.ToString("00");

            string gmTime = month + day + year + hour + min + sec;
            return gmTime;
        }

        public string RedirectURL()
        {


            string URL = "rcopia_portal_system_name={RPSN}&rcopia_practice_user_name={RPUN}&rcopia_user_id={RUID}&rcopia_user_external_id={RUEI}&service={SER}&action={LGN}&startup_screen={SCN}&rcopia_patient_id={RPI}&rcopia_patient_system_name={RPTSN}&rcopia_patient_external_id={RPTEI}&time={STIME}";

            string rcopia_portal_system_name = ConfigurationManager.AppSettings["rcopia_portal_system_name"].ToString();
            string rcopia_practice_user_name = ConfigurationManager.AppSettings["rcopia_practice_user_name"].ToString();
            string rcopia_user_id = ConfigurationManager.AppSettings["rcopia_user_id"].ToString();
            string rcopia_user_external_id = ConfigurationManager.AppSettings["rcopia_user_external_id"].ToString();
            string service = ConfigurationManager.AppSettings["service"].ToString();
            string action = ConfigurationManager.AppSettings["action"].ToString();
            string startup_screen = ConfigurationManager.AppSettings["startup_screen"].ToString();
            string rcopia_patient_id = ConfigurationManager.AppSettings["rcopia_patient_id"].ToString();
            string rcopia_patient_system_name = ConfigurationManager.AppSettings["rcopia_patient_system_name"].ToString();
            string rcopia_patient_external_id = ConfigurationManager.AppSettings["rcopia_patient_external_id"].ToString();
            string secret_key = ConfigurationManager.AppSettings["secret_key"].ToString();
            //MMDDYYHHMMSS
            //   DateTime d = DateTime.Now;
            //string Time = d.ToString("MMddyyHHmmss");
            // System.Console.WriteLine(dateString);

            string Time = gmtTime();



            URL = URL.Replace("{RPSN}", rcopia_portal_system_name);
            URL = URL.Replace("{RPUN}", rcopia_practice_user_name);
            URL = URL.Replace("{RUID}", rcopia_user_id);
            URL = URL.Replace("{RUEI}", rcopia_user_external_id);
            URL = URL.Replace("{SER}", service);
            URL = URL.Replace("{LGN}", action);
            URL = URL.Replace("{SCN}", startup_screen);

            URL = URL.Replace("{RPI}", rcopia_patient_id);
            URL = URL.Replace("{RPTSN}", rcopia_patient_system_name);
            URL = URL.Replace("{RPTEI}", rcopia_patient_external_id);

            URL = URL.Replace("{STIME}", Time);

            string MAC = CreateMD5(URL+ secret_key);

            //&MAC={SMAC}

            URL = URL.Replace("{SMAC}", MAC);

            string FinalURL = "https://web.staging.drfirst.com/sso/portalServices?" + URL+ "&MAC="+MAC;
            return FinalURL;


        }

    }
}
