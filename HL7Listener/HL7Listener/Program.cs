using System;
using System.Net;
using System.Threading;

namespace HL7Listener
{
    class Program
    {
        private static readonly byte[] Localhost = { 10, 20, 6, 111};
        private const int Port = 5678;

        static void Main(string[] args)
        {
            System.Net.IPAddress address = new IPAddress(Localhost);
            System.Net.IPEndPoint endPoint = new IPEndPoint(address, Port);
            string date = DateTime.Now.ToString().Replace("/","").Replace(" ","").Replace("AM","").Replace("PM","").Replace(":","");
            try
            {
                // Create a thread for listening to a port.
                Subscriber subscriber = new Subscriber(endPoint);
                System.Threading.Thread listnerThread = new Thread(new ThreadStart(subscriber.Listen));
                listnerThread.Start();
                // Craete another thread for sending HL7 messages
                // Send Message so that the listening port catches it.
                //Publisher publisher = new Publisher(Localhost, Port);
                //Thread senderThread = new Thread(new ThreadStart(publisher.Send));
                //senderThread.Start();
            }
            catch (Exception e)
            {
                // Exception handling
                Console.WriteLine("An unexpected exception occured: {0}", e.Message);
            }
        }
    }
}
