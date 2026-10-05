using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Hl7ListenerInboundService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //        System.Net.ServicePointManager.SecurityProtocol |=
            //SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);

            //#if DEBUG
            //Service1 myService = new Service1();
            //myService.Start();
            //myService.Stop();
            //#endif
        }
    }
}
