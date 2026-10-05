using EncDec;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hl7ListenerInboundService
{
    public partial class Service1 : ServiceBase
    {
        private Thread _thread;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        BusinessLayer biz = new BusinessLayer();
        clsEncDec encryptObj = new clsEncDec();
        byte[] Receivelocalhost = new byte[4];
        int Receiveport;
        private IPEndPoint endPoint;
        private System.Net.Sockets.Socket listener;
        static string response = string.Empty;
        static string PMNumber = string.Empty;
        string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
        string ErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
        public Service1()
        {
            InitializeComponent();
        }
        //Comment before deploye
        //public void Start()
        //{
        //    _thread = new Thread(WorkerThreadFunc);
        //    _thread.Start();

        //    // NewMailMethod("test mail", "Service Started");


        //}
        //private void Test()
        //{

        //    string path = ReceivePath + "/" + "Demographics_202209292118425125" + ".hl7";
        //    string fName = "Demographics_202209292118425125" + ".hl7";
        //    int count = 0;

        //    while (File.Exists(path))
        //    {
        //        File.Copy(path, ErrorPath + fName, true);
        //        File.Delete(path);
        //    }

        //}

        protected override void OnStart(string[] args)
        {
            Logger.ErrorLog("Service is started at " + DateTime.Now);
            NewMailMethod("Listener Service Started", "Service Started");
            NewMailMethod("test mail", "Service Started");
            _thread = new Thread(WorkerThreadFunc);
            _thread.Start();
        }
        private void WorkerThreadFunc()
        {
            try
            {
                Logger.ErrorLog("*************************************************************************");
                Logger.ErrorLog("entered AcceptCallback");

                DataTable dt = new DataTable();
                dt = biz.getServerDetailsByCompany(1, 3);
                string decrptIp = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    //for live 
                    decrptIp = encryptObj.psDecrypt(dt.Rows[0][0].ToString());
                    Receiveport = Convert.ToInt16(dt.Rows[0][1]);


                    //// for test vm
                    //decrptIp = "192.168.100.21";
                    //Receiveport = 5678;


                    string[] receiveIp = decrptIp.Split('.');
                    for (int i = 0; i < receiveIp.Length; i++)
                    {
                        Receivelocalhost[i] = Convert.ToByte(receiveIp[i]);
                    }




                    IPAddress address = new IPAddress(Receivelocalhost);
                    IPEndPoint endPoint = new IPEndPoint(address, Receiveport);
                    //IPAddress senderaddress = new IPAddress(localhost);
                    //IPEndPoint senderendPoint = new IPEndPoint(address, port);

                    this.endPoint = endPoint;
                    listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //listener.Disconnect(true);
                    listener.Blocking = false;
                    listener.Bind(endPoint);
                    listener.Listen(2000);

                    // Declare your variables.
                    // Do not declare variables inside loops like for, foreach, while etc.
                    // Because with every iteration, a new variable will be created.
                    // If your loop iterates 1000 times, you will end up creating 1000 variables instead of just one variable.

                    while (true)
                    {
                        // Set the event to nonsignaled state.
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        //Console.WriteLine("Waiting for a connection...");
                        listener.BeginAccept(
                          new AsyncCallback(AcceptCallback),
                          listener);

                        // Wait until a connection is made before continuing.
                        allDone.WaitOne();


                    }
                }
                else
                {
                    Logger.ErrorLog("Invalid Configuration");
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                if (ex.Message == "Only one usage of each socket address (protocol/network address/port) is normally permitted")
                {
                    Logger.ErrorLog("Service Already Running...");
                    //return "Service Already Running...";
                }
                else
                {
                    Logger.ErrorLog(ex);
                    _thread = new Thread(WorkerThreadFunc);
                    _thread.Start();
                }
                // Exception handling
            }
        }
        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Logger.ErrorLog("*************************************************************************");
                Logger.ErrorLog("entered AcceptCallback");
                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                  new AsyncCallback(ReadCallback), state);

              

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
               // Logger.ErrorLog($"Error: {ex.InnerException?.Message.ToString() ?? ex.Message} at line 174");

            }
        }
        public async void ReadCallback(IAsyncResult ar)
        {
            try
            {
                Logger.ErrorLog("*************************************************************************");
                Logger.ErrorLog("entered ReadCallback");
                String content = String.Empty;

                // Retrieve the state object and the handler socket
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                SocketError errorCode;
                // Read data from the client socket. 
                int bytesRead = handler.EndReceive(ar, out errorCode);
                int start;
                int end;
                string tempData;
                int bufSize = handler.ReceiveBufferSize;
                //if(errorCode !=SocketError.Success)
                //{
                //    bytesRead = 0;
                //}

                Logger.ErrorLog("bytesRead " + bytesRead.ToString());
                if (bytesRead > 0)
                {
                    Logger.ErrorLog("Entered Bytes Read Data ");

                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(
                      state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read 
                    // more data.
                    content = state.sb.ToString();





                    Logger.ErrorLog("Entered Bytes Read Data 1 -- " + state.sb.ToString());

                    int sequenceLength = 1;
                    int lengthToLog = Math.Min(sequenceLength, content.Length);

                    Logger.ErrorLog($"Character: '{content[content.Length - 1]}' (ASCII: {(int)content[content.Length - lengthToLog - 1]}, Unicode: \\u{((int)content[content.Length - lengthToLog - 1]).ToString("X4")})");

                    Logger.ErrorLog("checking file u001c-- " + content.IndexOf("<EOF>"));
                    Logger.ErrorLog("checking file EOF-- " + content.EndsWith("\r\u001c\r"));

                    //if (content.IndexOf("<EOF>") > -1)
                    //Correct file
                   // if (content.EndsWith("\r\u001d\r"))
                    if (content.EndsWith("\r\u001c\r"))
                    {

                        Logger.ErrorLog("Entered Bytes Read Data 1 inside if-- " + state.sb.ToString());

                        string[] data = state.sb.ToString().Split('\v');
                        int startindex = 1;
                        string[] splt = data[1].Split('\r');
                        int endindex = 0;
                        //byte[] resBuffer = new byte[106 * (data.Length - 1)];

                        Logger.ErrorLog("Entered Bytes Read Data length -- " + data.Length.ToString());

                        if (data.Length > 0)
                        {
                            for (int i = 1; i < data.Length; i++)
                            {
                                string contents = data[i];
                                //start = contents.IndexOf((char)0x0B);
                                start = contents.IndexOf("MSH");
                                if (start >= 0 && data.Length == 2)
                                {

                                    Logger.ErrorLog("Hl7 Message: " + contents);
                                    // Do what you want with the received message
                                    response = string.Empty;
                                    PMNumber = string.Empty;
                                    response = HandleMessage(contents);
                                    PMNumber = biz.GetPatientMrNumber(contents);
                                    Logger.ErrorLog("PMNumber :" + PMNumber);
                                    Logger.ErrorLog("response :" + response);

                                    //Test VM Logic
                                    if (PMNumber == "Error")
                                    {
                                        int actStatus = 3;

                                        Logger.ErrorLog("Sending Ack");
                                        handler.Send(Encoding.UTF8.GetBytes(response));



                                    }
                                    else
                                    {
                                        Logger.ResponseLog("ResPonse Message: " + response);

                                        string date = DateTime.Now.ToString("yyyyMMddHHmmssFFFFFF");

                                        string path = ReceivePath + "/" + date + ".hl7";
                                        string fName = date;
                                        int count = 0;

                                        while (File.Exists(path))
                                        {
                                            count++;
                                            fName = date + "_" + count;
                                            path = ReceivePath + "/" + date + "_" + count + ".hl7";
                                        }

                                        // Send response
                                        if (response != string.Empty)
                                        {
                                            string[] actArray = response.Split('|');
                                            int actStatus = 1;

                                            if (actArray[21].ToString() == "AE")
                                                actStatus = 2;
                                            else if (actArray[21].ToString() == "AR")
                                                actStatus = 3;
                                            //byte[] resbyte = Encoding.UTF8.GetBytes(response);
                                            //System.Buffer.BlockCopy(resbyte, 0, resBuffer, endindex, resbyte.Length);
                                            //endindex = endindex + resbyte.Length;


                                            biz.PrcFileAckSave(fName + ".hl7", response, actStatus, PMNumber);

                                            await DownloadReceived(Encoding.UTF8.GetBytes(contents), path, response, actStatus, PMNumber);
                                            //biz.PrcFileAckSave(date + ".hl7", response);
                                            handler.Send(Encoding.UTF8.GetBytes(response));
                                        }
                                        else
                                        {
                                            biz.PrcFileAckSave(fName + ".hl7", response, 2, PMNumber);
                                            //string date = DateTime.Now.ToString("yyyyMMddHHmmssFFFFFF");
                                            await DownloadReceived(Encoding.UTF8.GetBytes(contents), path, response, 2, PMNumber);
                                        }
                                    }

                                    //startindex = startindex + end + 2;
                                    //endindex = endindex + 12;
                                }
                                else
                                {
                                    Logger.ErrorLog(contents);
                                }
                            }
                        }
                        state.sb.Clear();
                        if (state.sb.Length == 0)
                        {
                            // Not all data received. Get more.

                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadCallback), state);
                        }
                    }
                    else
                    {
                        Logger.ElseLog("Entered Bytes Read Data 1 inside else-- " + state.sb.ToString());

                        string date = DateTime.Now.ToString("yyyyMMddHHmmssFFFFFF");
                        string path = ReceivePath + "/" + date + ".hl7";
                        NewMailMethod(path + "**************" + state.sb.ToString(), "Incorrect Processing of HL7");


                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

                       

                    }
                    //else
                    //{


                    //    Logger.ElseLog("Entered Bytes Read Data 1 inside else-- " + state.sb.ToString());

                    //    string[] data = state.sb.ToString().Split('\v');
                    //    int startindex = 1;
                    //    string[] splt = data[1].Split('\r');
                    //    int endindex = 0;
                    //    //byte[] resBuffer = new byte[106 * (data.Length - 1)];




                    //    Logger.ElseLog("Entered Bytes Read Data length -- " + data.Length.ToString());

                    //    if (data.Length > 0)
                    //    {
                    //        for (int i = 1; i < data.Length; i++)
                    //        {
                    //            string contents = data[i];
                    //            //start = contents.IndexOf((char)0x0B);
                    //            start = contents.IndexOf("MSH");
                    //            if (start >= 0 && data.Length == 2)
                    //            {

                    //                Logger.ElseLog("Hl7 Message: " + contents);
                    //                // Do what you want with the received message
                    //                response = string.Empty;
                    //                PMNumber = string.Empty;
                    //                response = HandleMessage(contents);
                    //                PMNumber = biz.GetPatientMrNumber(contents);
                    //                Logger.ElseLog("PMNumber :" + PMNumber);
                    //                Logger.ElseLog("response :" + response);

                    //                //Test VM Logic
                    //                if (PMNumber == "Error")
                    //                {
                    //                    int actStatus = 3;

                    //                    Logger.ElseLog("Sending Ack");
                    //                    handler.Send(Encoding.UTF8.GetBytes(response));



                    //                }
                    //                else
                    //                {
                    //                    Logger.ElseLog("ResPonse Message: " + response);

                    //                    string date = DateTime.Now.ToString("yyyyMMddHHmmssFFFFFF");

                    //                    string path = ReceivePath + "/" + date + ".hl7";
                    //                    string fName = date;
                    //                    int count = 0;
                    //                    NewMailMethod(path+"**************"+state.sb.ToString(), "Incorrect Processing of HL7");
                    //                    Logger.ElseLog("ResPonse Message: 1" );

                    //                    while (File.Exists(path))
                    //                    {
                    //                        count++;
                    //                        fName = date + "_" + count;
                    //                        path = ReceivePath + "/" + date + "_" + count + ".hl7";
                    //                    }
                    //                    Logger.ElseLog("ResPonse Message: 2");
                    //                    // Send response
                    //                    if (response != string.Empty)
                    //                    {
                    //                        Logger.ElseLog("ResPonse Message: 3");
                    //                        string[] actArray = response.Split('|');
                    //                        int actStatus = 1;

                    //                        if (actArray[21].ToString() == "AE")
                    //                            actStatus = 2;
                    //                        else if (actArray[21].ToString() == "AR")
                    //                            actStatus = 3;
                    //                        //byte[] resbyte = Encoding.UTF8.GetBytes(response);
                    //                        //System.Buffer.BlockCopy(resbyte, 0, resBuffer, endindex, resbyte.Length);
                    //                        //endindex = endindex + resbyte.Length;
                    //                        Logger.ElseLog("ResPonse Message: 4 , actStatus" + actArray[21].ToString());

                    //                        biz.PrcFileAckSave(fName + ".hl7", response, actStatus, PMNumber);

                    //                        Logger.ElseLog("ResPonse Message: 5");

                    //                        await DownloadReceived(Encoding.UTF8.GetBytes(contents), path, response, actStatus, PMNumber);
                    //                        //biz.PrcFileAckSave(date + ".hl7", response);
                    //                        Logger.ElseLog("ResPonse Message: 6");

                    //                        handler.Send(Encoding.UTF8.GetBytes(response));
                    //                    }
                    //                    else
                    //                    {
                    //                        Logger.ElseLog("ResPonse Message: 7");
                    //                        biz.PrcFileAckSave(fName + ".hl7", response, 2, PMNumber);
                    //                        //string date = DateTime.Now.ToString("yyyyMMddHHmmssFFFFFF");
                    //                        await DownloadReceived(Encoding.UTF8.GetBytes(contents), path, response, 2, PMNumber);
                    //                    }
                    //                }

                    //                //startindex = startindex + end + 2;
                    //                //endindex = endindex + 12;
                    //            }
                    //            else
                    //            {
                    //                Logger.ErrorLog(contents);
                    //            }
                    //        }
                    //    }
                    //    state.sb.Clear();
                    //    if (state.sb.Length == 0)
                    //    {
                    //        // Not all data received. Get more.

                    //        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //        new AsyncCallback(ReadCallback), state);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
               // Logger.ErrorLog($"Error: {ex.InnerException?.Message.ToString() ?? ex.Message} at line 435");

            }
        }
        private static string HandleMessage(string data)
        {
            try
            {
                string responseMessage = String.Empty;
                try
                {
                    //Console.WriteLine("Message received.");
                    BusinessLayer objBiz = new BusinessLayer();
                    Message msg = new Message();
                    msg.DeSerializeMessage(data);
                    // Create a response message
                    responseMessage = CreateResponseMessageString(objBiz.PrcFileAckStatusList(data));
                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex);
                    //Logger.ErrorLog($"Error: {ex.InnerException?.Message.ToString() ?? ex.Message} at line 456");

                }
                return responseMessage;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
               // Logger.ErrorLog($"Error: {ex.InnerException?.Message.ToString() ?? ex.Message} at line 464");

                return string.Empty;
            }
        }
        private static string CreateResponseMessageString(List<string> result)
        {
            try
            {
                Message response = new Message();

                foreach (string item in result)
                    response.AddString(item);

                // Create a Minimum Lower Layer Protocol (MLLP) frame.
                // For this, just wrap the data lik this: <VT> data <FS><CR>
                StringBuilder frame = new StringBuilder();
                frame.Append((char)0x0B);
                frame.Append(response.SerializeMessageString());
                frame.Append((char)0x1C);
                frame.Append((char)0x0D);

                return frame.ToString();
            }
            catch (Exception ex)
            {
                // Exception handling
                Logger.ErrorLog(ex);
               // Logger.ErrorLog($"Error: {ex.InnerException?.Message.ToString() ?? ex.Message} at line 492");

                return String.Empty;
            }
        }
        public async Task DownloadReceived(byte[] buffer, string fileName, string res, int actStatus, string PatientMRNumber)
        {
            try
            {
                //string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
                //string date = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace("AM", "").Replace("PM", "").Replace(":", "");
                //string fName = fileName;
                //string path = ReceivePath + "/" + fileName + ".hl7";
                //int count = 0;

                //while (File.Exists(path))
                //{
                //    count++;
                //    fName = fileName + "_" + count;
                //    path = ReceivePath + "/" + fileName + "_" + count + ".hl7";
                //}

                //biz.PrcFileAckSave(fName + ".hl7", res, actStatus, PatientMRNumber);
                //fileName = date;
                using (System.IO.FileStream fs1 = new FileStream(fileName, FileMode.Create))
                {
                    //BinaryWriter writer = new BinaryWriter(fs1);
                    //writer.Write(buffer);
                    await fs1.WriteAsync(buffer, 0, buffer.Length);
                    // writer.Close();
                    fs1.Close();
                }
                //File.WriteAllBytes(path, buffer);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
             //   Logger.ErrorLog($"Error: {ex.InnerException?.Message.ToString() ?? ex.Message} at line 529");

            }
        }
        protected override void OnStop()
        {
            Logger.ErrorLog("Service is Stopped at " + DateTime.Now);
        }

        public void NewMailMethod(string Data,string Subject)
        {
            DataTable dt = new DataTable();
            try
            {
                Logger.ErrorLog("Incorrect Processing of HL7");
                using (MailMessage mail = new MailMessage())
                {
                    //mail.Attachments.Add(new Attachment(rd.ExportToStream(ExportFormatType.PortableDocFormat), "OrderChange_Report.pdf"));
                    // mail.Attachments.Add(new Attachment(AppDomain.CurrentDomain.BaseDirectory + "\\Reports\\OrderChangeReport.pdf"));
                    // mail.IsBodyHtml = true;
                    //DataAccess dataAccess = new DataAccess();
                    //DataTable dtMail = biz.GetMailCredentials();
                    //string networkUserName = dtMail.Rows[0]["UserName"].ToString();
                    //string networkPassword = dtMail.Rows[0]["Pwd"].ToString();
                    //int networkPort = Convert.ToInt32(dtMail.Rows[0]["Port"].ToString());
                    //string networkHost = dtMail.Rows[0]["Host"].ToString();


                  //  mail.From = new MailAddress("emarsupport@pharmalife.com");

                    string FromEmail = "emarsupport@pharmalife.com";
                    string ToEmail = "rightmarsupport@promantra.us";
                    // mail.To.Add(dtrow["MailTo"].ToString());
                    // if (dtrow["MailCC"].ToString() != "" && dtrow["MailCC"].ToString() != null)
                    // {
                    //     mail.CC.Add(dtrow["MailCC"].ToString());
                    //}
                    // mail.Bcc.Add("rightmarsupport@promantra.us");

                    //mail.Bcc.Add("firdosei@revvpro.com");

                    string networkUserName = "";
                    string networkPassword = "";
                    int networkPort = 587;
                    string networkHost = "smtp1-mke.securence.com";


                    using (MailMessage mm = new MailMessage(FromEmail, ToEmail))
                    {
                        mail.Subject = Subject;
                        mm.Body = Data;
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
                       
                        Logger.ErrorLog("mail Sent successfully");
                    }
                    //Logger.ErrorLog("Order change report mail Sent successfully");
                }
            }
            catch (Exception exe)
            {
                Logger.ErrorLog(exe.InnerException.Message);
                //continue;
            }
        }

    }
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
       public const int BufferSize = 1024 * 1000;
       // public const int BufferSize = 612;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}