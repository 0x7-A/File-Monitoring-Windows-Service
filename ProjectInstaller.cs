using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FileMonitoringWindowsService
{



    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;
        public ProjectInstaller()
        {
            InitializeComponent();

            processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };

            serviceInstaller = new ServiceInstaller
            {
                // Set the name of the service
                ServiceName = "FileMonitoringService",
                DisplayName = "File Monitoring Windows Service project",
                Description = "A Windows Service that monitor files  that come to a source folder, renaeming, deleteing and moving , contain all service states and events.",
                StartType = ServiceStartMode.Automatic,
                ServicesDependedOn = new string[] { "RpcSs", "EventLog", "LanmanWorkstation" }
                // File watcher depend on these services
            };

            // Add both installers to the Installers collection
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

        }
    }
}
