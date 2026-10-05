using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using System.Threading;

namespace HL7FilesImportService
{
    public partial class Scheduler : ServiceBase
    {
        private Thread _thread;
        //private Timer timer1 = null;
        BusinessLayer biz = new BusinessLayer();
        string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
        string CompletedPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
        string ErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
        string OutboundPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
        int ProcessCount = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("ProcessCount")[0]);
        int CompanyID = 1;
        List<string> inProgressFiles = new List<string>();
        List<string> processedList = new List<string>();
        public Scheduler()
        {
            InitializeComponent();
        }
        public void Start()
        {
            Library.ErrorLog("Import Service Started.....");
            _thread = new Thread(WorkerThreadFunc);
            _thread.Start();
        }
        protected override void OnStart(string[] args)
        {
            //timer1 = new Timer();
            //int Processtimer = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("Processtime")[0]);
            //timer1.Interval = Processtimer;
            //timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            //timer1.Enabled = true;
            //Library.ErrorLog("Import Service Started.....");

            Library.ErrorLog("Import Service Started.....");
            _thread = new Thread(WorkerThreadFunc);
            _thread.Start();
        }

       
        //private void timer1_Tick(object sender, ElapsedEventArgs e)
        public void WorkerThreadFunc()
        {
            try
            {
                //Library.ErrorLog("InProgress File Count: " + inProgressFiles.Count());
                //if (inProgressFiles.Count() == 0)
                while(true)
                {
                    int userId = biz.GetUserID();
                    int result = 0;
                    int arcount = 0;

                    //inProgressFiles = Directory.EnumerateFiles(ReceivePath, "*.hl7").ToList();
                    DirectoryInfo dir = new DirectoryInfo(ReceivePath);
                    inProgressFiles = dir.GetFiles().OrderBy(p => p.CreationTime).Select(fi=>fi.FullName).ToList();
                    //Library.ErrorLog("Read File Count from Pending: " + inProgressFiles.Count());
                    if (inProgressFiles.Count() > 0)
                    {
                        if (inProgressFiles.Count() > ProcessCount)
                            arcount = ProcessCount;
                        else
                            arcount = inProgressFiles.Count();
                        System.Threading.Thread.Sleep(2000);
                        //foreach (string file in Directory.EnumerateFiles(ReceivePath, "*.hl7"))
                        foreach (string file in inProgressFiles.Take(arcount))
                        {
                            processedList.Add(file);
                            //string file = item;
                            string fileName = Path.GetFileName(file);
                            Int64 FileExist = biz.GetFileInfoCheck(fileName);
                            if (FileExist == 0)
                            {
                                string contents = string.Empty;
                                using (StreamReader sr = new StreamReader(file))
                                {
                                    contents = sr.ReadToEnd();
                                }
                                byte[] buff = Encoding.UTF8.GetBytes(contents);
                                Library.ErrorLog("Inserting into File Information Table - " + fileName);
                                Int64 fileID = biz.InsertFileInformation(CompanyID, fileName, buff, userId);
                                Library.ErrorLog("File Information Done - " + fileName);
                                if (fileID > 0)
                                {
                                    Library.ErrorLog("Inserting Data After Upload - " + fileName);
                                    result = biz.InsertDataAfterUpload(fileID.ToString(), userId);
                                    Library.ErrorLog("Inserting Data After Upload Done - " + fileName);
                                    if (result > 0)
                                    {
                                        if ((File.Exists(file)))
                                        {
                                            //In realtime shouldn't overwrite. This case will not happen.
                                            File.Copy(file.ToString(), CompletedPath + fileName, true);
                                            File.Delete(file);
                                        }
                                    }
                                    else
                                    {
                                        if ((File.Exists(file)))
                                        {
                                            //In realtime shouldn't overwrite. This case will not happen.
                                            File.Copy(file, ErrorPath + fileName, true);
                                            File.Delete(file);
                                        }
                                    }
                                }
                                else
                                {
                                    if ((File.Exists(file)))
                                    {
                                        //In realtime shouldn't overwrite. This case will not happen.
                                        File.Copy(file, ErrorPath + fileName, true);
                                        File.Delete(file);
                                    }
                                }
                            }
                            if ((File.Exists(file)))
                            {
                                //In realtime shouldn't overwrite. This case will not happen.
                                File.Copy(file, ErrorPath + fileName, true);
                                File.Delete(file);
                                Library.ErrorLog("File already processed, hence delete: " + fileName);
                            }
                            //DataTable dtAwatingrep = new DataTable();
                            //dtAwatingrep = biz.GetOutBoundAwatingFileData();
                            //foreach (DataRow dr in dtAwatingrep.Rows)
                            //{
                            //    if (dr["File_Name"].ToString() != "" && dr["File_Data"].ToString()!="")
                            //    {
                            //        string fullPath = OutboundPath + dr["File_Name"].ToString();
                            //        string fileData = dr["File_Data"].ToString();
                            //        System.IO.File.WriteAllText(fullPath, fileData);
                            //        int i = biz.UpdateOutBoundAwatingFile(dr["File_Name"].ToString());
                            //        Library.ErrorLog("Outbound File Downlod Completed " + fullPath);
                            //    }
                            //}
                        }
                        //var temp = processedList;
                        //inProgressFiles.RemoveAll(r => temp.Contains(r));
                        //processedList.RemoveAll(r => temp.Contains(r));
                        inProgressFiles.Clear();
                        processedList.Clear();
                    }
                    else
                    {
                        //var temp = processedList;
                        //inProgressFiles.RemoveAll(r => temp.Contains(r));
                        //processedList.RemoveAll(r => temp.Contains(r));
                        inProgressFiles.Clear();
                        processedList.Clear();
                    }
                    DataTable dtAwating = new DataTable();
                    dtAwating = biz.GetOutBoundAwatingFileData();
                    foreach (DataRow dr in dtAwating.Rows)
                    {
                        if (dr["File_Name"].ToString() != "" && dr["File_Data"].ToString() != "")
                        {
                            string fullPath = OutboundPath + dr["File_Name"].ToString();
                            string fileData = dr["File_Data"].ToString();
                            System.IO.File.WriteAllText(fullPath, fileData);
                            int i = biz.UpdateOutBoundAwatingFile(dr["File_Name"].ToString());
                            Library.ErrorLog("Outbound File Downlod Completed " + fullPath);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Library.ExceptionLog(ex);

                inProgressFiles.Clear();
                processedList.Clear();

                //timer1 = new Timer();
                //int Processtimer = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("Processtime")[0]);
                //timer1.Interval = Processtimer;
                //timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
                //timer1.Enabled = true;
                //Library.ErrorLog("Import Service Re-Started on Exception.....");
                //timer1_Tick(sender, e);

                Library.ErrorLog("Import Service Started.....");
                _thread = new Thread(WorkerThreadFunc);
                _thread.Start();
            }
            
            //Library.ErrorLog("Timer has done some job successfully");
        }
        protected override void OnStop()
        {
            _thread.Abort();
            Library.ErrorLog("Windows Service Stopped");
        }
        public static void RestartService(string serviceName, int timeoutInterval)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int time1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutInterval);
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                int time2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutInterval - (time1 - time2)); // Count the rest of the timeout 
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                Library.ExceptionLog(ex);
            }
        }
    }
}
