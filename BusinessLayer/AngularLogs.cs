using log4net;
using System;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Web;

namespace LTCPro.ServiceLayer               
{
    public class AngularLogs : IAngularLog
    {
        public AngularLogs()
        {

        }

        public int ErrorLogging(string errorMessage)
        {
            string folderPath = ConfigurationManager.AppSettings.GetValues("angularlog")[0].ToString();
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";

            string filePath = HttpContext.Current.Server.MapPath("~/" + folderPath + fileName);
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Error Message: " + errorMessage);
                //sw.WriteLine("Stack Trace: " + ex.StackTrace);
                sw.WriteLine("===========End============= " + DateTime.Now);

            }
            return 1;
        }
    }
}
