using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        ServiceHost Host = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Host = new ServiceHost(typeof(ImageViewerWCFService.ImageViewerWCFService));
            Host.Open();
        }

        protected override void OnStop()
        {
            Host.Close();
        }
    }
}
