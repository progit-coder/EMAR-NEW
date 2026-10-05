using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using EncDec;
using System.Net.NetworkInformation;

/// <summary>
/// Inbound Message Transition through Socket Port
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "http://100.8.37.102/hllistenerauth/WebServiceSender.asmx?")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebServiceSender : System.Web.Services.WebService
{
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    clsEncDec encryptObj = new clsEncDec();
    BusinessLayer biz = new BusinessLayer();
    private System.Net.Sockets.Socket sender;
    private System.Net.Sockets.Socket listener;
    private IPEndPoint endPoint;
    byte[] localhost = new byte[4];
    int port;
    byte[] Receivelocalhost = new byte[4];
    int Receiveport;
    string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
    string CompletedPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
    string ErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
    string OutboundPendingPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
    string OutboundCompletedPath = ConfigurationManager.AppSettings.GetValues("OutboundFileCompletedPath")[0].ToString();
    int CompanyID = 1;
    static string response = string.Empty;
    static string fileName = string.Empty;
    static int receiveCompanyId = 0;

    // ManualResetEvent instances signal completion.  
    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private static ManualResetEvent disconnectDone =
        new ManualResetEvent(false);
    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private static ManualResetEvent receiveDone =
        new ManualResetEvent(false);
    public AuthHeader Authentication;
    public WebServiceSender()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    //[WebMethod]
    //public string HelloWorld()
    //{
    //    return "Hello World";
    //}
    //[SoapHeader("Authentication", Required =true)]
    //    [WebMethod]
    //    public string HlMessageCheck()
    //    {
    //        response = @"MSH|^~\&|FrameworkLTC|PLATL|PROMANTRA|FC99|20200615154930||ACK^A08^ACK|21759|P|2.5||||||ASCII|||
    //MSA | AA | 21759 ||||
    //
    //";
    //        response = response.Replace('\r', '}');
    //        string[] msg = response.Split('}');
    //        string[] ackArray = msg[1].Split('|');
    //        if (ackArray.Length > 2)
    //        {
    //            string fileid = ackArray[2].Replace("\r", "").Replace("\u001c", "");
    //        }
    //        return response;
    //    }
    [WebMethod]
    public string SendMessages()
    {
        DataTable dt = new DataTable();
        try
        {
            while (true)
            {
                int count = Directory.GetFiles(OutboundPendingPath).Count();
                if (count > 0)
                {
                    dt = biz.getServerDetailsByCompany(2, 3);
                    string decrptIp = string.Empty;
                    if (dt.Rows.Count > 0)
                    {
                        decrptIp = encryptObj.psDecrypt(dt.Rows[0][0].ToString());
                        string[] receiveIp = decrptIp.Split('.');
                        //string[] receiveIp = "10.0.2.20".Split('.');
                        //port = 5679;
                        port = Convert.ToInt16(dt.Rows[0][1]);
                        var ConnectionCheck = PingHost(decrptIp, port);
                        if (ConnectionCheck)
                        {
                            for (int i = 0; i < receiveIp.Length; i++)
                            {
                                localhost[i] = Convert.ToByte(receiveIp[i]);
                            }
                            IPAddress address = new IPAddress(localhost);
                            IPEndPoint endPoint = new IPEndPoint(address, port);

                            // Create a TCP/IP socket.  
                            //Socket client = new Socket(address.AddressFamily,
                            //    SocketType.Stream, ProtocolType.Tcp);

                            Socket client = new Socket(AddressFamily.InterNetwork,
                            SocketType.Stream, ProtocolType.Tcp);

                            // Connect to the remote endpoint.  
                            client.BeginConnect(endPoint,
                                new AsyncCallback(ConnectCallback), client);
                            connectDone.WaitOne();
                            Thread.Sleep(5000);
                            count = Directory.GetFiles(OutboundPendingPath).Count();
                            // Release the socket.
                            if (count == 0)
                            {
                                client.Shutdown(SocketShutdown.Both);
                                client.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), client);

                                // Wait for the disconnect to complete.
                                disconnectDone.WaitOne();
                                client.Close();
                            }
                        }
                    }
                    else
                    {
                        return "Invalid Configuration";
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.ErrorLog(e);
            SendMessages();
            return e.ToString();
        }
    }
    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            //Console.WriteLine("Socket connected to {0}",
            //    client.RemoteEndPoint.ToString());

            // Signal that the connection has been made. 

            foreach (string file in Directory.EnumerateFiles(OutboundPendingPath, "*.hl7"))
            {
                string fileName = file.Replace(OutboundPendingPath, "");
                byte[] hl7Data = System.IO.File.ReadAllBytes(file);
                //byte[] hl7Data = Authentication.message;
                // Send test data to the remote device.  
                if ((File.Exists(file)))
                {
                    File.Copy(file, OutboundCompletedPath + fileName);
                    File.Delete(file);
                }
                Send(client, hl7Data);
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(client);
                receiveDone.WaitOne();
            }
            connectDone.Set();
        }
        catch (Exception e)
        {
            Logger.ErrorLog(e);
            Console.WriteLine(e.ToString());
        }
    }
    private static void DisconnectCallback(IAsyncResult ar)
    {
        try
        {
            // Complete the disconnect request.
            Socket client = (Socket)ar.AsyncState;
            client.EndDisconnect(ar);
            client.Close();
            // Signal that the disconnect is complete.
            disconnectDone.Set();
        }
        catch (Exception e)
        {
            Logger.ErrorLog(e);
        }
    }
    private static void Send(Socket client, byte[] data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        //byte[] byteData = Encoding.ASCII.GetBytes(data);
        byte[] byteData = data;
        int dataLength = byteData.Length;
        byte[] dataToSend = new byte[dataLength + 3];
        dataToSend[0] = 0x0b; // Add a Vertical Tab (VT) character
        Array.Copy(byteData, 0, dataToSend, 1, dataLength);
        dataToSend[dataLength + 1] = 0x1c; // Add File Separator (FS) charachter
        dataToSend[dataLength + 2] = 0x0d; // Add carriage return (CR) charachter

        // Begin sending the data to the remote device.  
        client.BeginSend(dataToSend, 0, dataToSend.Length, 0,
            new AsyncCallback(SendCallback), client);
    }
    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = client.EndSend(ar);
            //Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            // Signal that all bytes have been sent.  
            sendDone.Set();
        }
        catch (Exception e)
        {
            Logger.ErrorLog(e);
        }
    }
    private static void Receive(Socket client)
    {
        try
        {
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.  
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Logger.ErrorLog(e);
        }
    }
    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket   
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            BusinessLayer objbiz = new BusinessLayer();
            string OutboundPendingPath1 = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
            string OutboundCompletedPath1 = ConfigurationManager.AppSettings.GetValues("OutboundFileCompletedPath")[0].ToString();
            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                    string repMsg = response;
                    repMsg = repMsg.Replace('\r', '}');
                    string[] msg = repMsg.Split('}');
                    string[] ackArray = msg[1].Split('|');
                    if (ackArray.Length > 2)
                    {
                        string fileid = ackArray[2].Replace("\r", "").Replace("\u001c", "");
                        string result = objbiz.OutboundAckUpdate(Convert.ToInt32(fileid), response);
                    }
                }
                receiveDone.Set();
            }
            else
            {
                // All the data has arrived; put it in response.  
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                    string repMsg = response;
                    repMsg = repMsg.Replace('\r', '}');
                    string[] msg = repMsg.Split('}');
                    string[] ackArray = msg[1].Split('|');
                    if (ackArray.Length > 2)
                    {
                        string fileid = ackArray[2].Replace("\r", "").Replace("\u001c", "");
                        string result = objbiz.OutboundAckUpdate(Convert.ToInt64(fileid), response);
                    }
                }
                // Signal that all bytes have been received.  
                receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Logger.ErrorLog(e);
        }
    }
    //[SoapHeader("Authentication", Required = true)]
    [WebMethod]
    public string ReceiveMessage()
    {
        Logger.ErrorLog("Thasbdjksandlsa");
                try
                {
                    DataTable dt = new DataTable();
                    dt = biz.getServerDetailsByCompany(1, 3);
                    string decrptIp = string.Empty;
                    if (dt.Rows.Count > 0)
                    {
                        decrptIp = encryptObj.psDecrypt(dt.Rows[0][0].ToString());
                        string[] receiveIp = decrptIp.Split('.');
                        for (int i = 0; i < receiveIp.Length; i++)
                        {
                            Receivelocalhost[i] = Convert.ToByte(receiveIp[i]);
                        }
                        Receiveport = Convert.ToInt16(dt.Rows[0][1]);
                        IPAddress address = new IPAddress(Receivelocalhost);
                        IPEndPoint endPoint = new IPEndPoint(address, Receiveport);
                        //IPAddress senderaddress = new IPAddress(localhost);
                        //IPEndPoint senderendPoint = new IPEndPoint(address, port);

                        this.endPoint = endPoint;
                        listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        //listener.Disconnect(true);
                        listener.Blocking = false;
                        listener.Bind(endPoint);
                        listener.Listen(1000);

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
                        return "Invalid Configuration";
                    }
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    if (ex.Message == "Only one usage of each socket address (protocol/network address/port) is normally permitted")
                    {
                        return "Service Already Running...";
                    }
                    else
                    {
                        Logger.ErrorLog(ex);
                    }
                    // Exception handling
                }
                finally
                {
                    ReceiveMessage();
                }
                return "Done";
    }

    public async Task DownloadReceived(byte[] buffer, string fileName, string res, int actStatus)
    //public void DownloadReceived(string buffer)
    {
        try
        {
            string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            //string date = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace("AM", "").Replace("PM", "").Replace(":", "");
            string fName = fileName;
            string path = ReceivePath + "/" + fileName + ".hl7";
            int count = 0;

            while (File.Exists(path))
            {
                count++;
                fName = fileName + "_" + count;
                path = ReceivePath + "/" + fileName + "_" + count + ".hl7";
            }
            //fileName = date;
            using (System.IO.FileStream fs1 = new FileStream(path, FileMode.Create))
            {
                //BinaryWriter writer = new BinaryWriter(fs1);
                //writer.Write(buffer);
                await fs1.WriteAsync(buffer, 0, buffer.Length);
                biz.PrcFileAckSave(fName + ".hl7", res, actStatus);
                // writer.Close();
                fs1.Close();
            }
            //File.WriteAllBytes(path, buffer);
        }
        catch (Exception ex)
        {
            Logger.ErrorLog(ex);
        }
    }
    private static string HandleMessage(string data)
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
        }
        return responseMessage;
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
            return String.Empty;
        }
    }
    private static string CreateResponseMessage(string messageControlID, int result)
    {
        try
        {
            Message response = new Message();

            Segment msh = new Segment("MSH");
            msh.Field(2, "^~\\&");
            msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmsszzz"));
            msh.Field(9, "ACK");
            msh.Field(10, Guid.NewGuid().ToString());
            msh.Field(11, "P");
            msh.Field(12, "2.5.1");
            response.Add(msh);

            Segment msa = new Segment("MSA");
            if (result == 0)
                msa.Field(1, "AA");
            else
                msa.Field(1, "AE");
            msa.Field(2, messageControlID);
            response.Add(msa);

            // Create a Minimum Lower Layer Protocol (MLLP) frame.
            // For this, just wrap the data lik this: <VT> data <FS><CR>
            StringBuilder frame = new StringBuilder();
            frame.Append((char)0x0B);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x1C);
            frame.Append((char)0x0D);

            return frame.ToString();
        }
        catch (Exception ex)
        {
            // Exception handling
            Logger.ErrorLog(ex);
            return String.Empty;
        }
    }
    //[WebMethod]
    //public void ImportInboundFiles()
    //{
    //    try
    //    {
    //        int userId = biz.GetUserID();
    //        //while (true)
    //        //{
    //        int result = 0;
    //        foreach (string file in Directory.EnumerateFiles(ReceivePath, "*.hl7"))
    //        {
    //            string fileName = file.Replace(ReceivePath, "");
    //            String contents = File.ReadAllText(file);
    //            byte[] buff = Encoding.UTF8.GetBytes(contents);
    //            //string tempData = Encoding.UTF8.GetString(buff, 1, buff.Length - 12);
    //            Int64 fileID = biz.InsertFileInformation(CompanyID, fileName, buff, userId);
    //            //Int64 fileID = biz.InsertFileInformation(CompanyID, fileName, contents, userId);
    //            if (fileID > 0)
    //            {
    //                result = biz.InsertDataAfterUpload(fileID.ToString(), userId);
    //                if (result > 0)
    //                {
    //                    if ((File.Exists(file)))
    //                    {
    //                        File.Copy(file, CompletedPath + fileName);
    //                        File.Delete(file);
    //                    }
    //                }
    //                else
    //                {
    //                    if ((File.Exists(file)))
    //                    {
    //                        File.Copy(file, ErrorPath + fileName);
    //                        File.Delete(file);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if ((File.Exists(file)))
    //                {
    //                    File.Copy(file, ErrorPath + fileName);
    //                    File.Delete(file);
    //                }
    //            }
    //        }
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(ex.Message);
    //    }
    //}
    // Thread signal.

    public void AcceptCallback(IAsyncResult ar)
    {
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

    public async void ReadCallback(IAsyncResult ar)
    {
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
        //if(errorCode !=SocketError.Success)
        //{
        //    bytesRead = 0;
        //}
        if (bytesRead > 0)
        {
            // There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(
              state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            content = state.sb.ToString();
            //if (content.IndexOf("<EOF>") > -1)
            //if (content.EndsWith("\r\u001c\r"))
            //{
                string[] data = state.sb.ToString().Split('\v');
                int startindex = 1;
                int endindex = 0;
                //byte[] resBuffer = new byte[106 * (data.Length - 1)];
                if (data.Length > 0)
                {
                    for (int i = 1; i < data.Length; i++)
                    {
                        string contents = data[i];
                        //start = contents.IndexOf((char)0x0B);
                        start = contents.IndexOf("MSH");
                        if (start >= 0 && data.Length == 2)
                        {
                            // Search for a File Separator (FS) character to find the end of the frame.
                            end = contents.IndexOf((char)0x1C);
                        if (end > start)
                        {
                            // Remove the MLLP charachters
                            if (bytesRead > 12)
                                tempData = Encoding.UTF8.GetString(state.buffer, startindex, bytesRead - 12);
                            else
                                tempData = string.Empty;
                        }
                        else
                        {
                                tempData = Encoding.UTF8.GetString(state.buffer, startindex, bytesRead);
                        }
                        Logger.ErrorLog("Hl7 Message: "+tempData);
                        // Do what you want with the received message
                        response = HandleMessage(tempData);
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
                                    string date = DateTime.Now.ToString("yyyyMMddHHmmssFFFFFF");
                                    await DownloadReceived(Encoding.UTF8.GetBytes(contents), date, response, actStatus);
                                    //biz.PrcFileAckSave(date + ".hl7", response);
                                    handler.Send(Encoding.UTF8.GetBytes(response));
                                }

                                startindex = startindex + end + 2;
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
    }
    [WebMethod]
    public string RecievingMessageStatus(int Port)
    {
        var check = PortInUse(Port);
        //PingHost("115.115.106.156", 5678);
        if (check)
            return "Service Running";
        else
            return "Service Not Running";
    }
    //[WebMethod]
    //public string RecievingMessageOutboundStatus(int FteConfig_Id)
    //{
    //    DataTable dt = biz.getServerDetailsByFteConfig_Id(FteConfig_Id);
    //    if (dt.Rows.Count > 0)
    //    {
    //        string decrptedIp = encryptObj.psDecrypt(dt.Rows[0][0].ToString());
    //        int portCheck = Convert.ToInt16(dt.Rows[0][1]);
    //        var check = PingHost(decrptedIp, portCheck);
    //        if (check)
    //            return "Service Running";
    //        else
    //            return "Service Not Running";
    //    }
    //    else
    //    {
    //        return "No Configuration";
    //    }
    //}
    public static bool PortInUse(int port)
    {
        bool inUse = false;

        IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();


        foreach (IPEndPoint endPoint in ipEndPoints)
        {
            if (endPoint.Port == port)
            {
                inUse = true;
                break;
            }
        }


        return inUse;
    }
    public static bool PingHost(string hostUri, int portNumber)
    {
        try
        {
            using (var client = new TcpClient(hostUri, portNumber))
                return true;
        }
        catch (SocketException ex)
        {
            //MessageBox.Show("Error pinging host:'" + hostUri + ":" + portNumber.ToString() + "'");
            return false;
        }
    }

    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024 * 1000;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}