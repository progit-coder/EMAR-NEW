using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DrFirstApiService
{
    public partial class DrFirstApiService : ServiceBase
    {
        private System.Timers.Timer timer;

        public DrFirstApiService()
        {
            //Business business = new Business();
            //business.GetResidentsList();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Business business = new Business();
            business.GetResidentsList();
            double interval = Convert.ToDouble(ConfigurationManager.AppSettings["Interval"]);
            this.timer = new System.Timers.Timer(interval * 60000);  // 60000 milliseconds = 60 seconds

            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();
            
        }
        protected override void OnStop()
        {
            this.timer.Stop();
            this.timer = null;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Business business = new Business();
            business.GetResidentsList();
        }

    }
}
