using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ImageViewerWindowsServiceHost
{
    public partial class ImageViewerWindowsService : ServiceBase
    {
        ServiceHost _host = null;

        public ImageViewerWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _host = new ServiceHost(typeof(ImageViewerWCFService.ImageViewerWCFService));
            _host.Open();
        }

        protected override void OnStop()
        {
            _host.Close();
        }
    }
}
