using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace HL7EmarInboundSample
{
    public partial class Service1 : ServiceBase
    {
        private Thread _thread;
        private List<xConnection> _sockets;
        private System.Net.Sockets.Socket _serverSocket;
        int _bufferSize = 8000;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.ErrorLog("Service is started at " + DateTime.Now);
            _thread = new Thread(Start);
            _thread.Start();
        }
        public void Start()
        {
            System.Net.IPHostEntry localhost = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            System.Net.IPEndPoint serverEndPoint;
            try
            {
                serverEndPoint = new System.Net.IPEndPoint(localhost.AddressList[1], 5678);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException("Port number entered would seem to be invalid, should be between 1024 and 65000", e);
            }
            try
            {
                _serverSocket = new System.Net.Sockets.Socket(serverEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                throw new ApplicationException("Could not create socket, check to make sure not duplicating port", e);
            }
            try
            {
                _serverSocket.Bind(serverEndPoint);
                _serverSocket.Listen(2000);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error occured while binding socket, check inner exception", e);
            }
            try
            {
                //warning, only call this once, this is a bug in .net 2.0 that breaks if 
                // you're running multiple asynch accepts, this bug may be fixed, but
                // it was a major pain in the ass previously, so make sure there is only one
                //BeginAccept running
                while(true)
                _serverSocket.BeginAccept(new AsyncCallback(acceptCallback), _serverSocket);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error occured starting listeners, check inner exception", e);
            }
            //return true;
        }
        private void acceptCallback(IAsyncResult result)
        {
            xConnection conn = new xConnection();
            try
            {
                //Finish accepting the connection
                System.Net.Sockets.Socket s = (System.Net.Sockets.Socket)result.AsyncState;
                conn = new xConnection();
                conn.socket = s.EndAccept(result);
                conn.buffer = new byte[_bufferSize];
                lock (_sockets)
                {
                    _sockets.Add(conn);
                }
                //Queue recieving of data from the connection
                conn.socket.BeginReceive(conn.buffer, 0, conn.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), conn);
                //Queue the accept of the next incomming connection
                _serverSocket.BeginAccept(new AsyncCallback(acceptCallback), _serverSocket);
            }
            catch (SocketException e)
            {
                if (conn.socket != null)
                {
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                }
                //Queue the next accept, think this should be here, stop attacks based on killing the waiting listeners
                _serverSocket.BeginAccept(new AsyncCallback(acceptCallback), _serverSocket);
            }
            catch (Exception e)
            {
                if (conn.socket != null)
                {
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                }
                //Queue the next accept, think this should be here, stop attacks based on killing the waiting listeners
                _serverSocket.BeginAccept(new AsyncCallback(acceptCallback), _serverSocket);
            }
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            //get our connection from the callback
            xConnection conn = (xConnection)result.AsyncState;
            //catch any errors, we'd better not have any
            try
            {
                //Grab our buffer and count the number of bytes receives
                int bytesRead = conn.socket.EndReceive(result);
                //make sure we've read something, if we haven't it supposadly means that the client disconnected
                if (bytesRead > 0)
                {
                    //put whatever you want to do when you receive data here

                    //Queue the next receive
                    conn.socket.BeginReceive(conn.buffer, 0, conn.buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), conn);
                }
                else
                {
                    //Callback run but no data, close the connection
                    //supposadly means a disconnect
                    //and we still have to close the socket, even though we throw the event later
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                }
            }
            catch (SocketException e)
            {
                //Something went terribly wrong
                //which shouldn't have happened
                if (conn.socket != null)
                {
                    conn.socket.Close();
                    lock (_sockets)
                    {
                        _sockets.Remove(conn);
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }
        public class xConnection 
        {
            public byte[] buffer;
            public System.Net.Sockets.Socket socket;
        }
    }
}
