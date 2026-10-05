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
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;

namespace HL7OutBoundService
{
    public partial class Service1 : ServiceBase
    {
        private System.Timers.Timer timer1 = null;
        string OutboundPendingPath = ConfigurationManager.AppSettings.GetValues("OutboundFilePendingPath")[0].ToString();
        string OutboundCompletedPath = ConfigurationManager.AppSettings.GetValues("OutboundFileCompletedPath")[0].ToString();
        int Processcount = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("ProcessCount")[0]);
        int ProcessSleep = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("ProcessSleep")[0]);
        BusinessLayer biz = new BusinessLayer();
        clsEncDec encryptObj = new clsEncDec();
        int port;
        byte[] localhost = new byte[4];
        static string response = string.Empty;

        private static ManualResetEvent connectDone =
        new ManualResetEvent(false);
        private static ManualResetEvent disconnectDone =
        new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        public Service1()
        {
            InitializeComponent();
        }
        //public void Start()
        //{
        //    string decrptIp1 = encryptObj.psDecrypt("nIYSV7dy7IkThyo3TyCEgg==");
        //    string decrptIp2 = encryptObj.psDecrypt("nIYSV7dy7IkCjihZRwkhVg==");
        //    //timer1_Tick();
        //}
        protected override void OnStart(string[] args)
        {
            timer1 = new System.Timers.Timer();
            int Processtimer = Convert.ToInt16(ConfigurationManager.AppSettings.GetValues("Processtime")[0]);
            timer1.Interval = Processtimer;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            timer1.Enabled = true;
            Library.ErrorLog("Import Service Started.....");
        }
        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                //while (true)
                //{
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
                        //return "Invalid Configuration";
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                Library.ExceptionLog(ex);
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

                foreach (string file in Directory.EnumerateFiles(OutboundPendingPath, "*.hl7").Take(Processcount))
                {
                    string fileName = file.Replace(OutboundPendingPath, "");
                    byte[] hl7Data = System.IO.File.ReadAllBytes(file);
                    //byte[] hl7Data = Authentication.message;
                    // Send test data to the remote device.  
                    //if ((File.Exists(file)))
                    //{
                    //    File.Copy(file, OutboundCompletedPath + fileName);
                    //    File.Delete(file);
                    //}
                    Send(client, hl7Data);
                    sendDone.WaitOne();

                    // Receive the response from the remote device.  
                    Receive(client);
                    receiveDone.WaitOne();

                    // Write the response to the console.  
                    //Console.WriteLine("Response received : {0}", response);
                    //int result = biz.OutboundAckUpdate(fileName, response);
                    //if ((File.Exists(file)))
                    //{
                    //    File.Copy(file, OutboundCompletedPath + fileName);
                    //    File.Delete(file);
                    //}
                    Thread.Sleep(ProcessSleep);
                }
                connectDone.Set();
            }
            catch (Exception e)
            {
                Library.ExceptionLog(e);
                //Console.WriteLine(e.ToString());
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
                Library.ExceptionLog(e);
                //Console.WriteLine(e.ToString());
            }
        }
        private static void Send(Socket client, byte[] data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            //byte[] byteData = Encoding.ASCII.GetBytes(data);
            byte[] byteData = data;
            int dataLength = byteData.Length;
            byte[] dataToSend = new byte[dataLength + 3];
            dataToSend[0] = 0x0B; // Add a Vertical Tab (VT) character
            Array.Copy(byteData, 0, dataToSend, 1, dataLength);
            dataToSend[dataLength + 1] = 0x1C; // Add File Separator (FS) charachter
            dataToSend[dataLength + 2] = 0x0D; // Add carriage return (CR) charachter
            string fileData = System.Text.Encoding.UTF8.GetString(dataToSend);
            Library.Filelog(fileData);
            // Begin sending the data to the remote device.  
            client.BeginSend(dataToSend, 0, dataToSend.Length, 0,
                new AsyncCallback(SendCallback), client);
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
                Library.ExceptionLog(e);
                //Console.WriteLine(e.ToString());
            }
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
                Library.ExceptionLog(e);
                //Console.WriteLine(e.ToString());
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
                        Library.ResponseFilelog(response);
                        string repMsg = response;
                        repMsg = repMsg.Replace('\r', '}');
                        string[] msg = repMsg.Split('}');
                        string[] ackArray = msg[1].Split('|');
                        if (ackArray.Length > 2)
                        {
                            string fileid = ackArray[2].Replace("\r", "").Replace("\u001c", "");
                            if (fileid != "")
                            {
                                //string result = objbiz.OutboundAckUpdate(Convert.ToInt32(fileid), response);
                                string result = objbiz.OutboundAckUpdate(fileid, response);
                                if (result != "")
                                {
                                    string file = OutboundPendingPath1 + @"\" + result;
                                    if ((File.Exists(file)))
                                    {
                                        File.Copy(file, OutboundCompletedPath1 + result, true);
                                        File.Delete(file);
                                    }
                                }
                            }
                            else
                            {
                                Library.ErrorLog(response);
                            }
                        }
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
                        Library.ResponseFilelog(response);
                        string repMsg = response;
                        repMsg = repMsg.Replace('\r', '}');
                        string[] msg = repMsg.Split('}');
                        string[] ackArray = msg[1].Split('|');
                        if (ackArray.Length > 2)
                        {
                            string fileid = ackArray[2].Replace("\r", "").Replace("\u001c", "");
                            if (fileid != "")
                            {
                                string result = objbiz.OutboundAckUpdate(fileid, response);
                                if (result != "")
                                {
                                    string file = OutboundPendingPath1 + @"\" + result;
                                    if ((File.Exists(file)))
                                    {
                                        File.Copy(file, OutboundCompletedPath1 + result);
                                        File.Delete(file);
                                    }
                                }
                            }
                            else
                            {
                                Library.ErrorLog(response);
                            }
                        }
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Library.ExceptionLog(e);
                //Console.WriteLine(e.ToString());
            }
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
        protected override void OnStop()
        {
            this.timer1.Stop();
            this.timer1 = null;
            Library.ErrorLog("Service Stopped....." + DateTime.Now);
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
