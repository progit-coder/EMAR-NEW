using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HL7FilesImportService
{
    public static class Library
    {
        public static void ExceptionLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                string logFile = "\\Logs\\LogFile_Exception_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        public static void ErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                string logFile = "\\Logs\\LogFile_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFile, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}
