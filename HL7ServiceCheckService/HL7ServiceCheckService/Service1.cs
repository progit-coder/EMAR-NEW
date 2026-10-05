using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Configuration;
using EncDec;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace HL7ServiceCheckService
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer1 = null;
        BusinessLayer ObjBIZ = new BusinessLayer();
        clsEncDec encrypt = new clsEncDec();
        DataTable dtFTE;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            Int64 Processtimer = Convert.ToInt64(ConfigurationManager.AppSettings.GetValues("Processtime")[0]);
            timer1.Interval = Processtimer;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;
            Library.ErrorLog("HL7ServiceCheckService Service Started.....");
        }
        //public void Start()
        //{
        //    CHeck();
        //}

        //public void CHeck()
        //{
        //    Library.ErrorLog("Method Service Started.....");
        //    string FromMail = ConfigurationManager.AppSettings.GetValues("FromMail")[0].ToString();
        //    try
        //    {
        //        dtFTE = new DataTable();
        //        dtFTE = ObjBIZ.GetFTEConfigurationReport();
        //        DataTable dtError = new DataTable();
        //        string strError = string.Empty;
        //        string strErrorIDS = string.Empty;
        //        dtError = ObjBIZ.GetFileACKAEARData();
        //        //if (dtError.Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow dre in dtError.Rows)
        //        //    {
        //        //        strError = strError + "<br><b>File Data:</b><br>" + dre["File_Data"] + "<br>";
        //        //        strError = strError + "<b>File ACK Data:</b>" + dre["FileAck_Data"] + "<br>";
        //        //        strErrorIDS = strErrorIDS + dre["FileAckInformation_Id"] + ",";
        //        //    }
        //        //    if (strError != string.Empty)
        //        //    {
        //        //        DataTable dtemails = new DataTable();
        //        //        dtemails = ObjBIZ.GetSuperAdminUsersEmail();
        //        //        if (dtemails.Rows.Count > 0 && dtemails.Rows[0][0].ToString() != "")
        //        //        {
        //        //            SendEmail(dtemails.Rows[0][0].ToString(), "", "", "InBound Files Accepted With Errors and Rejections Alert", strError);
        //        //            ObjBIZ.UpdateAckSent(strErrorIDS.Substring(0, strErrorIDS.Length - 1));
        //        //        }
        //        //    }
        //        //}
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Library.ExceptionLog(ex);
        //        //timer1_Tick(sender, e);
        //    }
        //}

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            Library.ErrorLog("Method Service Started.....");
            try
            {
                //dtFTE = new DataTable();
                //dtFTE = ObjBIZ.GetFTEConfigurationReport();
                DataTable dtError = new DataTable();
                DataTable dtemails = new DataTable();
                dtemails = ObjBIZ.GetSuperAdminUsersEmail();
                string strError = string.Empty;
                string strErrorIDS = string.Empty;                
                dtError = ObjBIZ.GetFileACKAEARData();
                if (dtError.Rows.Count > 0)
                {
                    foreach (DataRow dre in dtError.Rows)
                    {
                        strError = strError + "<br><b>File Data:</b><br>" + dre["File_Data"] + "<br>";
                        strError = strError + "<b>File ACK Data:</b>" + dre["FileAck_Data"] + "<br>";
                        strErrorIDS = strErrorIDS + dre["FileAckInformation_Id"] + ",";
                    }
                    if (strError != string.Empty)
                    {
                        
                        if (dtemails.Rows.Count > 0 && dtemails.Rows[0][0].ToString() != "")
                        {
                            SendEmail(dtemails.Rows[0][0].ToString(), "", "", "InBound Files Accepted With Errors and Rejections Alert", strError);
                            ObjBIZ.UpdateAckSent(strErrorIDS.Substring(0, strErrorIDS.Length - 1));
                        }
                    }
                }
                //Library.ErrorLog("Service List Count....."+ dtFTE.Rows.Count);
                //foreach (DataRow item in dtFTE.Rows)
                //{
                //    string ServerIP = encrypt.psDecrypt(item["ServerIp"].ToString());
                //    string Status = "";
                //    WebReference.WebServiceSender importFiles = new WebReference.WebServiceSender();
                //    System.Net.ServicePointManager.Expect100Continue = false;
                //    //string Status = importFiles.RecievingMessageStatus(Port);
                //    Library.ErrorLog("Server IP is....."+ ServerIP);
                //    if (item["Port"] != null)
                //    {
                //        Library.ErrorLog("Port is....." + Convert.ToInt16(item["Port"]));
                //        Status = importFiles.RecievingMessageStatus(Convert.ToInt16(item["Port"]));
                //        Library.ErrorLog("Message Status is....." + Status);                        
                //        if(item["FteCategory_Desc"].ToString()== "Inbound" && Status== "Service Not Running")
                //        {
                //            string run = importFiles.ReceiveMessage();
                //            //int res = ObjBIZ.UpdateFTEConnectionStatus(Convert.ToInt16(item["FteConfig_Id"]), "Service Running");
                //            //DataTable dtemails = new DataTable();
                //            //dtemails = ObjBIZ.GetSuperAdminUsersEmail();
                //            //if(dtemails.Rows.Count>0 && dtemails.Rows[0][0].ToString()!="")
                //            //{
                //            //    SendEmail(dtemails.Rows[0][0].ToString(), "", "", "InBound Service Alert", Status);
                //            //}
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Library.ExceptionLog(ex);
                timer1_Tick(sender, e);
            }
        }
        public static void SendEmail(String ToEmail, string cc, string bcc, String Subj, string Message)
        {
            //Reading sender Email credential from web.config file  
            BusinessLayer ObjBIZ2 = new BusinessLayer();
            DataTable dtconfig = new DataTable();
            Service1 objSer = new Service1();
            string FromMail = ConfigurationManager.AppSettings.GetValues("FromMail")[0].ToString();
            dtconfig = ObjBIZ2.GetMailConfigData();
            if (dtconfig.Rows.Count > 0)
            {
                string HostAdd = dtconfig.Rows[0]["Host"].ToString();
                string FromEmailid = dtconfig.Rows[0]["UserName"].ToString();
                string Pass = dtconfig.Rows[0]["Pwd"].ToString();
                int port = Convert.ToInt16(dtconfig.Rows[0]["Port"]);
                string body = objSer.createEmailBody("", Message, "");
                //creating the object of MailMessage  
                MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress(FromEmailid); //From Email Id  
                mailMessage.From = new MailAddress(FromMail); //From Email Id  
                mailMessage.Subject = Subj; //Subject of Email  
                mailMessage.Body = body; //body or message of Email  
                mailMessage.IsBodyHtml = true;

                string[] ToMuliId = ToEmail.Split(',');
                foreach (string ToEMailId in ToMuliId)
                {
                    if (ToEMailId != "")
                        mailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                }


                string[] CCId = cc.Split(',');

                foreach (string CCEmail in CCId)
                {
                    if(CCEmail!="")
                    mailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id  
                }

                string[] bccid = bcc.Split(',');

                foreach (string bccEmailId in bccid)
                {
                    if (bccEmailId != "")
                        mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id  
                }
                SmtpClient smtp = new SmtpClient();  // creating object of smptpclient  
                smtp.Host = HostAdd;              //host of emailaddress for example smtp.gmail.com etc  

                //network and security related credentials  

                smtp.EnableSsl = false;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = FromEmailid;
                NetworkCred.Password = Pass;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = port;
                smtp.Send(mailMessage); //sending Email  
            }
            else
            {
                Library.ErrorLog("No Email Configuration Found...");
            }
        }
        private string createEmailBody(string userName, string title, string message)

        {

            string body = string.Empty;
            string mailBody = ConfigurationManager.AppSettings.GetValues("MailBody")[0].ToString();
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(mailBody))

            {

                body = reader.ReadToEnd();

            }

            //body = body.Replace("{UserName}", userName); //replacing the required things  

            body = body.Replace("{Text}", title);

            //body = body.Replace("{message}", message);

            return body;

        }
        protected override void OnStop()
        {
            timer1.Enabled = false;
            Library.ErrorLog("Windows Service Stopped");
        }
    }
}
