using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
//using System.mail
using System.Net.Mail;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ClosedXML.Excel;
using System.Globalization;
//using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer;
        private System.Timers.Timer timerRej;

        DataAccess dataAccess = new DataAccess();
        private Thread _thread;
        public Service1()
        {
            //getdata();
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {

            //this.WriteToFile("Simple Service started {0}");

            Logger.ErrorLog("Service started");
            //  this.ScheduleService();
            _thread = new Thread(CallMethod);
            _thread.Start();

            //this.timer = new System.Timers.Timer(120000D);  // 30000 milliseconds = 30 seconds
            //this.timer = new System.Timers.Timer();
            //this.timer.Interval = TimeSpan.FromMinutes(4).TotalMilliseconds;
            //this.timer.AutoReset = true;
            //this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            //this.timer.Start();


            //For order Change
            this.timer = new System.Timers.Timer();
            this.timer.Interval = TimeSpan.FromMinutes(4).TotalMilliseconds;
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            // _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            this.timer.Start();


            //For Rejection
            this.timerRej = new System.Timers.Timer();
            this.timerRej.Interval = TimeSpan.FromMinutes(60).TotalMilliseconds;
            this.timerRej.AutoReset = true;
            this.timerRej.Elapsed += new System.Timers.ElapsedEventHandler(timer_ElapsedRej);
            // _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            this.timerRej.Start();

            
        }
        public void CallMethod()
        {

           // RefilDisContinueMailfiles();
            orderChangeReportMail();
            NewMailMethod("Service Started", "Service Started");


        }


        //For Rejection

        private void timer_ElapsedRej(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Logger.ErrorLog("timer_ElapsedRej entered");
                RefilDisContinueMailfiles();

            }
            catch (Exception ex)
            {
                Logger.ErrorLog("Exception Message in  timer_ElapsedRej" + ex.Message.ToString());
                Logger.ErrorLog("IOnner Exception Message in  timer_ElapsedRej" + ex.InnerException.Message.ToString());
                NewMailMethod(ex.Message.ToString(), ex.InnerException.Message.ToString());
            }



        }

        //For order Change
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Logger.ErrorLog("timer_Elapsed entered");
                DateTime dt = DateTime.Now;
                string Time = dt.ToString("hh:mm tt");
                string date = DateTime.Now.ToString("MM/dd/yyyy");

                string Timings = ConfigurationManager.AppSettings["Timings"];

                string[] timeArray = Timings.Split(',');

                Logger.ErrorLog("Timings " + Timings);

                foreach (var item in timeArray)
                {
                    string dtime = date + " " + item;
                    DateTime From = Convert.ToDateTime(dtime);
                    DateTime To = Convert.ToDateTime(dtime).AddMinutes(5);

                    if (dt > From && dt < To)
                    {
                        //'11:23' > '11:30' && '11:05' < '11:05'

                        //'01/17/2025 3:00:59 AM' > '01/17/2025 03:00 AM' && '01/17/2025 3:00:59 AM' < '01/17/2025 03:05 AM'

                        Logger.ErrorLog("Calling Methods");


                        orderChangeReportMail();

                    }


                }

               
            }
            catch (Exception ex)
            {
                Logger.ErrorLog("Exception Message in  timer_Elapsed" + ex.Message.ToString());
                Logger.ErrorLog("IOnner Exception Message in  timer_Elapsed" + ex.InnerException.Message.ToString());
                NewMailMethod(ex.Message.ToString(), ex.InnerException.Message.ToString());
            }

            
        }
        protected override void OnStop()
        {
            // this.WriteToFile("Simple Service stopped {0}");
            Logger.ErrorLog("Service Stoped");
            this.Schedular.Dispose();
        }
        public void RefilDisContinueMailfiles()
        {
            try
            {
                DateTime dtnew = new DateTime();

                //getting User Mail Detail
                DataTable dt = new DataTable();
                dt = dataAccess.GetRefilMailDetailsByTimme(System.DateTime.Now);

                if (dt.Rows.Count > 0)
                {
                    //Get Mailconfig Credentials
                    Logger.ErrorLog("RefilDisContinueMailfiles Records found to send mail at this time");
                    DataTable dtMail = dataAccess.GetMailCredentials();
                    string networkUserName = dtMail.Rows[0]["UserName"].ToString();
                    string networkPassword = dtMail.Rows[0]["Pwd"].ToString();
                    int networkPort = Convert.ToInt32(dtMail.Rows[0]["Port"].ToString());
                    string networkHost = dtMail.Rows[0]["Host"].ToString();

                    foreach (DataRow dtrow in dt.Rows)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dataAccess.GetRefilMailConfigInfo(Convert.ToInt32(dtrow["NurseStation_Id"].ToString()), Convert.ToInt32(dtrow["Rdc_Id"].ToString()), Convert.ToInt32(dtrow["Hour_Id"].ToString()));
                        if (dt1.Rows.Count > 0)
                        {
                            int fileId = Convert.ToInt32(dtrow["File_Id"]);
                            Logger.ErrorLog("RefilDisContinueMailfiles Records found for this nursing station " + dtrow["NurseStation_Id"].ToString() + ", Rdc_Id " + dtrow["Rdc_Id"].ToString() + ", Hour_Id " + dtrow["Hour_Id"].ToString());
                            string records = string.Join(",", dt1.AsEnumerable().Select(r => r.Field<int>("File_Id")).ToArray());
                            int t = dataAccess.RefilStatusUpdate(records);
                            dt1.Columns.Remove("File_Id");
                            dt1.TableName = "Data";

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                //Add the DataTable as Excel Worksheet.
                                wb.Worksheets.Add(dt1);

                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    //Save the Excel Workbook to MemoryStream.
                                    wb.SaveAs(memoryStream);

                                    //Convert MemoryStream to Byte array.
                                    byte[] bytes = memoryStream.ToArray();
                                    memoryStream.Close();

                                    //Send Email with Excel attachment.
                                    try
                                    {
                                        Logger.ErrorLog("RefilDisContinueMailfiles Mail Sent Started");
                                        using (MailMessage mail = new MailMessage())
                                        {
                                            //var date = DateTime.Today.ToString("MM-dd-yyyy").Replace("-", "");
                                            //var time = string.Format("{0:hh:mm:ss tt}", DateTime.Now).Replace(":", "");
                                            //Add Byte array as Attachment.
                                            mail.Attachments.Add(new Attachment(new MemoryStream(bytes), "RejectionfilesList.xlsx"));
                                            mail.IsBodyHtml = true;
                                            mail.From = new MailAddress("emarsupport@pharmalife.com");


                                            mail.To.Add(dtrow["MailTo"].ToString());
                                            if (dtrow["MailCC"].ToString() != "" && dtrow["MailCC"].ToString() != null)
                                            {
                                                mail.CC.Add(dtrow["MailCC"].ToString());
                                            }
                                            //mail.Bcc.Add("tajuddeenh@promantra.us");
                                            //mail.Bcc.Add("srikarv@RevvPro.com");
                                            mail.Bcc.Add("rightmarsupport@promantra.us");
                                            mail.Subject = "RejectionfilesList.xlsx";
                                            string Body = "The Following Attachment Includes Refill and Discontinue Orders Rejection files List.";
                                            mail.Body = Body;
                                            //SmtpClient smtp = new SmtpClient();
                                            //smtp.Host = "mail.revvpro.com"; //Or Your SMTP Server Address
                                            //smtp.Port = 587;
                                            //smtp.UseDefaultCredentials = false;
                                            //smtp.Credentials = new System.Net.NetworkCredential
                                            //("support@revvpro.com", "]rRx&C%k!*y8Bp#@)(w");

                                            ////Or your Smtp Email ID and Password
                                            //smtp.EnableSsl = true;
                                            SmtpClient smtp = new SmtpClient();
                                            smtp.Host = "smtp1-mke.securence.com";
                                           // smtp.Host = networkHost;
                                            smtp.EnableSsl = false;
                                            smtp.Credentials = new System.Net.NetworkCredential("", "");
                                            smtp.UseDefaultCredentials = false;
                                            smtp.Port = 587;
                                            int res = dataAccess.PrcUpdateMailsendStatus(fileId);
                                            smtp.Send(mail);
                                            //int res = dataAccess.PrcUpdateMailsendStatus(fileId);
                                            Logger.ErrorLog("RefilDisContinueMailfiles Mail Sent successfully " + res);
                                        }
                                    }
                                    catch (Exception exe)
                                    {
                                        Logger.ErrorLog(exe.Message);
                                        Logger.ErrorLog(exe.InnerException.Message);
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Logger.ErrorLog("RefilDisContinueMailfiles No Records found for this nursing station");
                        }
                    }
                }
                else
                {
                    Logger.ErrorLog("RefilDisContinueMailfiles No Records found to sent mail at this time");
                }
            }

            catch (Exception ex)
            {
                Logger.ErrorLog(ex.Message);
                Logger.ErrorLog(ex.InnerException.Message);
            }
        }
        public void orderChangeReportMail()
        {
            try
            {

                //getting User Mail Detail
                DataTable dt = new DataTable();
                DateTime currentDate = System.DateTime.Now;
                dt = dataAccess.GetOrderChangeReportMailDetailsByTimme(System.DateTime.Now);

                if (dt.Rows.Count > 0)
                {
                    //Get Mailconfig Credentials
                    Logger.ErrorLog("Order change records found to send mail at this time");
                    DataTable dtMail = dataAccess.GetMailCredentials();
                    string networkUserName = dtMail.Rows[0]["UserName"].ToString();
                    string networkPassword = dtMail.Rows[0]["Pwd"].ToString();
                    int networkPort = Convert.ToInt32(dtMail.Rows[0]["Port"].ToString());
                    string networkHost = dtMail.Rows[0]["Host"].ToString();

                    foreach (DataRow dtrow in dt.Rows)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dataAccess.GetOrderChangeReportRecords(Convert.ToInt32(dtrow["NurseStation_Id"].ToString()), Convert.ToInt32(dtrow["Rdc_Id"].ToString()), Convert.ToInt32(dtrow["Hour_Id"].ToString()), currentDate);
                        if (dt1.Rows.Count > 0)
                        {
                            Logger.ErrorLog("Order Change records found for this nursing station" + dtrow["NurseStation_Id"].ToString());
                            //getting Header Logo
                            DataTable facility = dataAccess.GetFacilityIdByNsId(Convert.ToInt32(dtrow["NurseStation_Id"].ToString()));
                            int facilityId = Convert.ToInt32(facility.Rows[0]["Facility_Id"]);
                            DataTable dtFac = new DataTable();
                            dtFac = dataAccess.GetHeaderLogo(facilityId);

                            //getting Footer Logo
                            DataTable dtEmar = new DataTable();
                            dtEmar = dataAccess.GetFooterLogo();

                            var records = dt1;
                            var emarlogo = dtEmar;
                            var facilitylogo = dtFac;
                            // Delete previous pdf file
                            string reportsFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Reports";
                            string orderChangePDF = "OrderChangeReport.pdf";
                            if (File.Exists(Path.Combine(reportsFolder, orderChangePDF)))
                            {
                                // If file found, delete it    
                                File.Delete(Path.Combine(reportsFolder, orderChangePDF));
                                Logger.ErrorLog("File deleted.");
                            }
                            ReportDocument rd = new ReportDocument();
                            string reportsPath = AppDomain.CurrentDomain.BaseDirectory + "crptOrderChangeDetailsReport.rpt";
                            rd.Load(reportsPath);
                            rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(dtEmar);
                            System.Drawing.Image Footerimage = System.Drawing.Image.FromStream(new System.IO.MemoryStream((byte[])dtEmar.Rows[0]["FooterLogo"]));
                            if (Footerimage.Size.Width >= Footerimage.Size.Height + 150)
                            {
                                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 2500;
                            }
                            else
                            {
                                rd.Subreports[0].ReportDefinition.ReportObjects[0].Height = 900;
                                rd.Subreports[0].ReportDefinition.ReportObjects[0].Width = 1000;
                            }
                            if (dtFac.Rows.Count > 0 && dtFac.Rows[0]["ReportHeaderLogo"].ToString().Length > 0)
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream((byte[])dtFac.Rows[0]["ReportHeaderLogo"]));
                                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                                if (image.Size.Width >= image.Size.Height + 120)
                                {
                                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 2500;
                                }
                                else
                                {
                                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Height = 900;
                                    rd.Subreports[1].ReportDefinition.ReportObjects[0].Width = 1000;
                                }
                            }
                            else
                            {
                                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                            }
                            rd.SetDataSource(records);
                            rd.SetParameterValue("ReportName", "Order Change  Report");
                            rd.SetParameterValue("DateTime", System.DateTime.Now);
                            rd.SetParameterValue("FromDate", "");
                            rd.SetParameterValue("ToDate", "");
                            rd.SetParameterValue("residentStatus", "");
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, AppDomain.CurrentDomain.BaseDirectory + "\\Reports\\OrderChangeReport.pdf");

                            rd.Close();
                            rd.Dispose();

                            //Send Email with PDF attachment.
                            try
                            {
                                Logger.ErrorLog("Order change report Mail Sent Started");
                                using (MailMessage mail = new MailMessage())
                                {
                                    //mail.Attachments.Add(new Attachment(rd.ExportToStream(ExportFormatType.PortableDocFormat), "OrderChange_Report.pdf"));
                                    mail.Attachments.Add(new Attachment(AppDomain.CurrentDomain.BaseDirectory + "\\Reports\\OrderChangeReport.pdf"));
                                    mail.IsBodyHtml = true;
                                    mail.From = new MailAddress("emarsupport@pharmalife.com");


                                    mail.To.Add(dtrow["MailTo"].ToString());
                                    if (dtrow["MailCC"].ToString() != "" && dtrow["MailCC"].ToString() != null)
                                    {
                                        mail.CC.Add(dtrow["MailCC"].ToString());
                                    }

                                    // mail.To.Add("srikarv@RevvPro.com");
                                    // mail.Bcc.Add("srikarv@RevvPro.com");
                                    //mail.Bcc.Add("tajuddeenh@revvpro.com");

                                    //mail.To.Add("kdivya@revvpro.com");
                                    //mail.Bcc.Add("kdivya@revvpro.com");
                                    //mail.To.Add("dbalaji@promantra.net");

                                    mail.Bcc.Add("rightmarsupport@promantra.us");

                                    mail.Subject = "Order Change Report";
                                    string Body = "The Following Attachment Includes Order Change List.";
                                    mail.Body = Body;
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = "smtp1-mke.securence.com";
                                    // smtp.Host = networkHost;
                                    smtp.EnableSsl = false;
                                    smtp.Credentials = new System.Net.NetworkCredential("", "");
                                    smtp.UseDefaultCredentials = false;
                                    smtp.Port = 587;
                                    int re = this.dataAccess.insertUpdateOrderChangeMailSent(Convert.ToInt32(dtrow["NurseStation_Id"].ToString()), currentDate);
                                    smtp.Send(mail);
                                    Logger.ErrorLog("mail Sent successfully");
                                    Logger.ErrorLog("Order change report mail Sent successfully");
                                }
                            }
                            catch (Exception exe)
                            {
                                Logger.ErrorLog(exe.InnerException.Message);
                                continue;
                            }
                        }
                        else
                        {
                            Logger.ErrorLog("Order change report no Records found for this nursing station");
                        }
                    }
                }
                else
                {
                    Logger.ErrorLog("Order change report no Records found to sent mail at this time");
                }
            }

            catch (Exception ex)
            {
                Logger.ErrorLog(ex.Message);
                Logger.ErrorLog(ex.InnerException.Message);
            }
        }

        public void NewMailMethod(string exception, string innerException)
        {
            DataTable dt = new DataTable();
            try
            {
                Logger.ErrorLog("Order change report Mail Sent Started");
                using (MailMessage mail = new MailMessage())
                {
                    //mail.Attachments.Add(new Attachment(rd.ExportToStream(ExportFormatType.PortableDocFormat), "OrderChange_Report.pdf"));
                    // mail.Attachments.Add(new Attachment(AppDomain.CurrentDomain.BaseDirectory + "\\Reports\\OrderChangeReport.pdf"));
                    // mail.IsBodyHtml = true;

                    DataTable dtMail = dataAccess.GetMailCredentials();
                    string networkUserName = dtMail.Rows[0]["UserName"].ToString();
                    string networkPassword = dtMail.Rows[0]["Pwd"].ToString();
                    int networkPort = Convert.ToInt32(dtMail.Rows[0]["Port"].ToString());
                    string networkHost = dtMail.Rows[0]["Host"].ToString();


                    mail.From = new MailAddress("emarsupport@pharmalife.com");


                    // mail.To.Add(dtrow["MailTo"].ToString());
                    // if (dtrow["MailCC"].ToString() != "" && dtrow["MailCC"].ToString() != null)
                    // {
                    //     mail.CC.Add(dtrow["MailCC"].ToString());
                    //}
                    mail.Bcc.Add("rightmarsupport@promantra.us");
                    // mail.Bcc.Add("tajuddeenh@revvpro.com");
                    //mail.Bcc.Add("firdosei@revvpro.com");



                    mail.Subject = "Excption in Order Change Service";
                    string Body = "Exception : " + exception + "</br>" + "Inner Exception :" + innerException;
                    mail.Body = Body;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp1-mke.securence.com";
                    // smtp.Host = networkHost;
                    smtp.EnableSsl = false;
                    smtp.Credentials = new System.Net.NetworkCredential("", "");
                    smtp.UseDefaultCredentials = false;
                    smtp.Port = 587;
                    smtp.Send(mail);
                    Logger.ErrorLog("mail Sent successfully");
                    //Logger.ErrorLog("Order change report mail Sent successfully");
                }
            }
            catch (Exception exe)
            {
                Logger.ErrorLog(exe.InnerException.Message);
                // continue;
            }
        }

        private Timer Schedular;
        public void ScheduleService()
        {
            try
            {
                Schedular = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                //this.WriteToFile("Simple Service Mode: " + mode + " {0}");

                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;

                if (mode.ToUpper() == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                //this.WriteToFile("Simple Service scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                Schedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                //WriteToFile("Simple Service Error on: {0} " + ex.Message + ex.StackTrace);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }

        private void SchedularCallback(object e)
        {

            this.ScheduleService();
            try
            {

                //getting User Mail Detail
                DataTable dt = new DataTable();
                dt = dataAccess.GetUserAndEmail();

                //getting Network Credentials
                DataTable dtMail = dataAccess.GetMailCredentials();


                //string networkUserName = dtMail.Rows[0]["UserName"].ToString();
                //string networkPassword = dtMail.Rows[0]["Pwd"].ToString();
                //int networkPort = Convert.ToInt32(dtMail.Rows[0]["Port"].ToString());
                //string networkHost = dtMail.Rows[0]["Host"].ToString();

                string networkUserName = "";
                string networkPassword = "";
                int networkPort = 587;
                string networkHost = "smtp1-mke.securence.com";


                foreach (DataRow dtrow in dt.Rows)
                {
                    DataTable dtCompanyId = new DataTable();

                    dtCompanyId = dataAccess.GetStockReportsCompanyId(Convert.ToInt32(dtrow["User_Id"].ToString()));

                    foreach (DataRow drRowCompany in dtCompanyId.Rows)
                    {

                        DataTable dtFac = new DataTable();
                        dtFac = dataAccess.GetHeaderLogo(Convert.ToInt32(drRowCompany["Company_Id"].ToString()));

                        //getting Footer Logo
                        DataTable dtEmar = new DataTable();
                        dtEmar = dataAccess.GetFooterLogo();


                        try
                        {
                            DataTable dt1 = new DataTable();
                            dt1 = dataAccess.GetStockReportsData(Convert.ToInt32(dtrow["User_Id"].ToString()), Convert.ToInt32(drRowCompany["Company_Id"].ToString()));

                            //byte[] bytes = (byte[])dtEmar.Rows[0]["FooterLogo"];


                            //List<emarlogo> emarLogoprop = new List<emarlogo>();
                            //emarlogo emarnew = new emarlogo();

                            //emarnew.FooterLogo = bytes;
                            //emarLogoprop.Add(emarnew);
                            if (dt1.Rows.Count > 0)
                            {
                                var records = dt1;
                                var emarlogo = dtEmar;
                                var facilitylogo = dtFac;

                                ReportDocument rd = new ReportDocument();
                                string reportsPath = AppDomain.CurrentDomain.BaseDirectory + "crptStockDetailsReport.rpt";
                                rd.Load(reportsPath);
                                rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(dtEmar);
                                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
                                rd.SetDataSource(records);
                                rd.SetParameterValue("ReportName", "Stock  Report");


                                string ToEmail = dtrow["User_Email"].ToString();
                                string FromEmail = ConfigurationManager.AppSettings["FromMailID"];

                                using (MailMessage mm = new MailMessage(FromEmail, ToEmail))
                                {
                                    mm.Subject = "Stock Report - " + drRowCompany["Company_Name"].ToString();
                                    mm.Body = string.Format("<b>Hi<br />");
                                    mm.Attachments.Add(new Attachment(rd.ExportToStream(ExportFormatType.PortableDocFormat), "Stock_Report.pdf"));
                                    mm.IsBodyHtml = true;
                                    SmtpClient smtp = new SmtpClient();
                                    smtp.Host = networkHost;
                                    smtp.EnableSsl = false;
                                    //// System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                                    // credentials.UserName = networkUserName;
                                    // credentials.Password = networkPassword;
                                    smtp.Credentials = new System.Net.NetworkCredential("", "");

                                    smtp.UseDefaultCredentials = false;
                                    // smtp.Credentials = credentials;
                                    smtp.Port = 587;
                                    smtp.Send(mm);
                                    rd.Close();
                                    rd.Dispose();
                                    Logger.ErrorLog("mail Sent successfully");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                }

                // }
                this.ScheduleService();
            }
            catch (Exception ex)
            {

                //using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                ////using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("StockReportService"))
                //{
                //    serviceController.Stop();
                //}
            }
        }

        //public void getdata()
        //{


        //    //getting User Mail Detail
        //    DataTable dt = new DataTable();
        //    dt = dataAccess.GetUserAndEmail();

        //    //getting Network Credentials
        //    DataTable dtMail = dataAccess.GetMailCredentials();
        //    string networkUserName = dtMail.Rows[0]["UserName"].ToString();
        //    string networkPassword = dtMail.Rows[0]["Pwd"].ToString();
        //    int networkPort = Convert.ToInt32(dtMail.Rows[0]["Port"].ToString());
        //    string networkHost = dtMail.Rows[0]["Host"].ToString();

        //    foreach (DataRow dtrow in dt.Rows)
        //    {
        //        DataTable dtCompanyId = new DataTable();

        //        dtCompanyId = dataAccess.GetStockReportsCompanyId(Convert.ToInt32(dtrow["User_Id"].ToString()));

        //        foreach (DataRow drRowCompany in dtCompanyId.Rows)
        //        {

        //            DataTable dtFac = new DataTable();
        //            dtFac = dataAccess.GetHeaderLogo(Convert.ToInt32(drRowCompany["Company_Id"].ToString()));

        //            //getting Footer Logo
        //            DataTable dtEmar = new DataTable();
        //            dtEmar = dataAccess.GetFooterLogo();


        //            try
        //            {
        //                DataTable dt1 = new DataTable();
        //                dt1 = dataAccess.GetStockReportsData(Convert.ToInt32(dtrow["User_Id"].ToString()), Convert.ToInt32(drRowCompany["Company_Id"].ToString()));

        //                //byte[] bytes = (byte[])dtEmar.Rows[0]["FooterLogo"];


        //                //List<emarlogo> emarLogoprop = new List<emarlogo>();
        //                //emarlogo emarnew = new emarlogo();

        //                //emarnew.FooterLogo = bytes;
        //                //emarLogoprop.Add(emarnew);

        //                var records = dt1;
        //                var emarlogo = dtEmar;
        //                var facilitylogo = dtFac;

        //                ReportDocument rd = new ReportDocument();
        //                string reportsPath = AppDomain.CurrentDomain.BaseDirectory + "crptStockDetailsReport.rpt";
        //                rd.Load(reportsPath);
        //                rd.OpenSubreport("crptEmarlogoReport.rpt").SetDataSource(dtEmar);
        //                rd.OpenSubreport("crptFacilitylogoReport.rpt").SetDataSource(facilitylogo);
        //                rd.SetDataSource(records);
        //                rd.SetParameterValue("ReportName", "Stock  Report");


        //                string ToEmail = dtrow["User_Email"].ToString();
        //                string FromEmail = ConfigurationManager.AppSettings["FromMailID"];

        //                using (MailMessage mm = new MailMessage(FromEmail, ToEmail))
        //                {
        //                    mm.Subject = "Stock Report - "+ drRowCompany["Company_Name"].ToString();
        //                    mm.Body = string.Format("<b>Hi<br />");
        //                    mm.Attachments.Add(new Attachment(rd.ExportToStream(ExportFormatType.PortableDocFormat), "Stock_Report.pdf"));
        //                    mm.IsBodyHtml = true;
        //                    SmtpClient smtp = new SmtpClient();
        //                    smtp.Host = networkHost;
        //                    smtp.EnableSsl = true;
        //                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
        //                    credentials.UserName = networkUserName;
        //                    credentials.Password = networkPassword;
        //                    smtp.UseDefaultCredentials = true;
        //                    smtp.Credentials = credentials;
        //                    smtp.Port = networkPort;
        //                    smtp.Send(mm);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                continue;
        //            }
        //        }

        //    }
        //}



        //public class emarlogo
        //{
        //    public byte[] FooterLogo { get; set; }
        //}
        //public class FacilityLogoMain
        //{
        //    public string FacilityName { get; set; }
        //    public byte[] FacilityLogo { get; set; }
        //}


        //private void WriteToFile(string text)
        //{
        //    string path = "C:\\ServiceLog.txt";
        //    using (StreamWriter writer = new StreamWriter(path, true))
        //    {
        //        writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
        //        writer.Close();
        //    }
        //}        

    }
}
