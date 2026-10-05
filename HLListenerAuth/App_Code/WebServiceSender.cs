using System;
using System.Collections.Generic;
using System.Configuration;
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
//using EncDec;

/// <summary>
/// Summary description for WebServiceSender
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "http://100.8.37.102/hllistenerauth/WebServiceSender.asmx?")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebServiceSender : System.Web.Services.WebService
{
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    BusinessLayer biz = new BusinessLayer();
    private System.Net.Sockets.Socket sender;
    private System.Net.Sockets.Socket listener;
    private IPEndPoint endPoint;
    byte[] localhost = { 173, 160, 76, 129 };
    //byte[] localhost = { 115, 115, 106, 156 };
    int port = 1024;
    //int port = 5678;
    byte[] Receivelocalhost = { 10, 0, 2, 20 };
    int Receiveport = 5678;
    string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
    string CompletedPath = ConfigurationManager.AppSettings.GetValues("InboundFileCompletedPath")[0].ToString();
    string ErrorPath = ConfigurationManager.AppSettings.GetValues("InboundFileErrorPath")[0].ToString();
    string OutboundPendingPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
    string OutboundCompletedPath = ConfigurationManager.AppSettings.GetValues("OutboundFileCompletedPath")[0].ToString();
    int CompanyID = 1;
    static string response = string.Empty;
    static string fileName = string.Empty;

    // ManualResetEvent instances signal completion.  
    private static ManualResetEvent connectDone =
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
    [SoapHeader("Authentication", Required =true)]
    [WebMethod]
    public string SendMessages()
    {
        //if (Authentication.UserName != "" && Authentication.Password != "")
        {
            //if (biz.ValidateUser(Authentication.UserName, Authentication.Password) > 0)
            {
                IPAddress address = new IPAddress(localhost);
                IPEndPoint endPoint = new IPEndPoint(address, port);
                //try
                //{
                //    foreach (string file in Directory.EnumerateFiles(OutboundPendingPath, "*.hl7"))
                //    {
                //        string fileName = file.Replace(OutboundPendingPath, "");
                //        sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //        sender.Connect(endPoint);
                //        //byte[] hl7Data = System.IO.File.ReadAllBytes(@"C:\Users\Jagadeesh\Desktop\Files\Admit.txt");
                //        byte[] hl7Data = System.IO.File.ReadAllBytes(file);
                //        int dataLength = hl7Data.Length;
                //        byte[] dataToSend = new byte[dataLength + 3];
                //        dataToSend[0] = 0x0b; // Add a Vertical Tab (VT) character
                //        Array.Copy(hl7Data, 0, dataToSend, 1, dataLength);
                //        dataToSend[dataLength + 1] = 0x1c; // Add File Separator (FS) charachter
                //        dataToSend[dataLength + 2] = 0x0d; // Add carriage return (CR) charachter
                //        sender.SendBufferSize = 4096;
                //        try
                //        {
                //            sender.Send(dataToSend);
                //            //if ((File.Exists(file)))
                //            //{
                //            //    File.Copy(file, OutboundCompletedPath + fileName);
                //            //    File.Delete(file);
                //            //}
                //            //Console.WriteLine("HL7 message sent.");
                //        }
                //        catch (System.Net.Sockets.SocketException ex)
                //        {
                //            // Exception handling
                //        }
                //        System.Threading.Thread.Sleep(1000);
                //    }
                //}
                //catch (System.Net.Sockets.SocketException ex)
                //{
                //    // Exception handling
                //}
                //finally
                //{
                //    sender.Close();
                //}
                try
                {
                    // Create a TCP/IP socket.  
                    Socket client = new Socket(address.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.  
                    client.BeginConnect(endPoint,
                        new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();
                    foreach (string file in Directory.EnumerateFiles(OutboundPendingPath, "*.hl7"))
                    {
                        string fileName = file.Replace(OutboundPendingPath, "");
                        byte[] hl7Data = System.IO.File.ReadAllBytes(file);
                        // Send test data to the remote device.  
                        Send(client, hl7Data);
                        sendDone.WaitOne();

                        // Receive the response from the remote device.  
                        Receive(client);
                        receiveDone.WaitOne();

                        // Write the response to the console.  
                        Console.WriteLine("Response received : {0}", response);
                        int result = biz.OutboundAckUpdate(fileName, response);
                        if ((File.Exists(file)))
                        {
                            File.Copy(file, OutboundCompletedPath + fileName);
                            File.Delete(file);
                        }
                        Thread.Sleep(500);
                    }
                    // Release the socket.  
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.ToString();
                    //Console.WriteLine(e.ToString());
                }
            }
            //else
            //{
            //    return "Invalid Credentials";
            //}
        }
        //else
        //{
        //    return "Please enter UserName and Password";
        //}
    }
    private static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.  
            connectDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
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
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            // Signal that all bytes have been sent.  
            sendDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
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
            Console.WriteLine(e.ToString());
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

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                //DownloadReceived(state.buffer);
                // Get the rest of the data.  
                //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //    new AsyncCallback(ReceiveCallback), state);
                receiveDone.Set();
            }
            else
            {
                // All the data has arrived; put it in response.  
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }
                // Signal that all bytes have been received.  
                receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    //[SoapHeader("Authentication", Required = true)]
    [WebMethod]
    public string ReceiveMessage(string userId, byte[] passwordDigest, string nonce, DateTime ts, byte[] message)
    {
        //if (Authentication.UserName != "" && Authentication.Password != "")
        {
            //if (biz.ValidateUser(Authentication.UserName, Authentication.Password) > 0)
            {
                //clsEncDec encObj = new clsEncDec();
                //string usr = encObj.psDecrypt(userName);
                //string pwd = encObj.psDecrypt(password);
                try
                {
                    IPAddress address = new IPAddress(Receivelocalhost);
                    IPEndPoint endPoint = new IPEndPoint(address, Receiveport);
                    IPAddress senderaddress = new IPAddress(localhost);
                    IPEndPoint senderendPoint = new IPEndPoint(address, port);

                    this.endPoint = endPoint;
                    listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //listener.Disconnect(true);
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
                catch (System.Net.Sockets.SocketException ex)
                {
                    if (ex.Message == "Only one usage of each socket address (protocol/network address/port) is normally permitted")
                    {
                        return "Service Already Running...";
                    }
                    else
                    {
                        return ex.Message;
                    }
                    // Exception handling
                }
                return "Done";
            }
            //else
            //{
            //    return "Invalid Credentials";
            //}
        }
        //else
        //{
        //    return "Please enter UserName and Password";
        //}
    }

    public static void DownloadReceived(byte[] buffer)
    {
        try
        {
            string ReceivePath = ConfigurationManager.AppSettings.GetValues("InboundFilePendingPath")[0].ToString();
            string date = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace("AM", "").Replace("PM", "").Replace(":", "");
            string path = ReceivePath + "/" + date + ".hl7";
            fileName = date;
            System.IO.FileStream fs1 = null;
            fs1 = new FileStream(path, FileMode.Create);
            fs1.Write(buffer, 0, buffer.Length);
            fs1.Close();
            //File.WriteAllBytes(path, buffer);
        }
        catch (Exception ex)
        {
            //Throw your Error
        }
    }
    private static string HandleMessage(string data)
    {
        string responseMessage = String.Empty;
        try
        {
            //Console.WriteLine("Message received.");

            Message msg = new Message();
            msg.DeSerializeMessage(data);

            // You can do what you want with the message here as per your appliation requirements.
            // For eg: read patient ID, patient last name, age etc.

            // Create a response message
            //
            responseMessage = CreateResponseMessage(msg.MessageControlId());
            //responseMessage = "\vMSH|^~\\&|RECEIVER|SMARTHL7|||||ACK|20190115104815|P|2.5||||||ASCII|||\nMSA|AA|488248||||\u001c\r";
            //responseMessage = "\vMSH|^~\\&|RECEIVER|SMARTHL7|||||ACK|20190115104815|P|2.5.1||||||ASCII|||\u001c\r";

        }
        catch (Exception ex)
        {
            // Exception handling            
        }
        return responseMessage;
    }
    private static string CreateResponseMessage(string messageControlID)
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
            msh.Field(12, "2.5.1||");
            response.Add(msh);

            Segment msa = new Segment("MSA");
            msa.Field(1, "AR");
            msa.Field(2, messageControlID + "|||");
            response.Add(msa);


            // Create a Minimum Lower Layer Protocol (MLLP) frame.
            // For this, just wrap the data lik this: <VT> data <FS><CR>
            StringBuilder frame = new StringBuilder();
            frame.Append((char)0xB);
            frame.Append(response.SerializeMessage());
            frame.Append((char)0x1C);
            frame.Append((char)0xD);

            return frame.ToString();
        }
        catch (Exception ex)
        {
            // Exception handling

            return String.Empty;
        }
    }
    [WebMethod]
    public void ImportInboundFiles()
    {
        try
        {
            int userId = biz.GetUserID();
            //while (true)
            //{
            int result = 0;
            foreach (string file in Directory.EnumerateFiles(ReceivePath, "*.hl7"))
            {
                string fileName = file.Replace(ReceivePath, "");
                String contents = File.ReadAllText(file);
                byte[] buff = Encoding.UTF8.GetBytes(contents);
                //string tempData = Encoding.UTF8.GetString(buff, 1, buff.Length - 12);
                Int64 fileID = biz.InsertFileInformation(CompanyID, fileName, buff, userId);
                //Int64 fileID = biz.InsertFileInformation(CompanyID, fileName, contents, userId);
                if (fileID > 0)
                {
                    result = biz.InsertDataAfterUpload(fileID.ToString(), userId);
                    if (result > 0)
                    {
                        if ((File.Exists(file)))
                        {
                            File.Copy(file, CompletedPath + fileName);
                            File.Delete(file);
                        }
                    }
                    else
                    {
                        if ((File.Exists(file)))
                        {
                            File.Copy(file, ErrorPath + fileName);
                            File.Delete(file);
                        }
                    }
                }
                else
                {
                    if ((File.Exists(file)))
                    {
                        File.Copy(file, ErrorPath + fileName);
                        File.Delete(file);
                    }
                }
            }
            //}
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
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

    public void ReadCallback(IAsyncResult ar)
    {
        String content = String.Empty;

        // Retrieve the state object and the handler socket
        // from the asynchronous state object.
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        // Read data from the client socket. 
        int bytesRead = handler.EndReceive(ar);
        int start;
        int end;
        string tempData;

        if (bytesRead > 0)
        {
            //DownloadReceived(state.buffer);
            // There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(
              state.buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read 
            // more data.
            content = state.sb.ToString();
            //if (content.IndexOf("<EOF>") > -1)
            if (content.EndsWith("\r\u001c\r"))
            {
                string[] data = state.sb.ToString().Split('\v');
                int startindex = 1;
                int endindex = 0;
                byte[] resBuffer = new byte[106 * (data.Length - 1)];
                if (data.Length > 0)
                {
                    for (int i = 1; i < data.Length; i++)
                    {
                        string contents = data[i];
                        //start = contents.IndexOf((char)0x0B);
                        DownloadReceived(Encoding.UTF8.GetBytes(contents));
                        System.Threading.Thread.Sleep(1000);
                        start = contents.IndexOf("MSH");
                        if (start >= 0 && data.Length == 2)
                        {
                            // Search for a File Separator (FS) character to find the end of the frame.
                            end = contents.IndexOf((char)0x1C);
                            if (end > start)
                            {
                                // Remove the MLLP charachters
                                tempData = Encoding.UTF8.GetString(state.buffer, startindex, bytesRead - 12);
                                // Do what you want with the received message
                                response = HandleMessage(tempData);
                                // Send response
                                if (response != string.Empty)
                                {
                                    //byte[] resbyte = Encoding.UTF8.GetBytes(response);
                                    //System.Buffer.BlockCopy(resbyte, 0, resBuffer, endindex, resbyte.Length);
                                    //endindex = endindex + resbyte.Length;
                                    biz.PrcFileAckSave(fileName + ".hl7", response);
                                    handler.Send(Encoding.UTF8.GetBytes(response));
                                }

                                startindex = startindex + end + 2;
                                //endindex = endindex + 12;
                            }
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
                // Not all data received. Get more.
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
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
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}