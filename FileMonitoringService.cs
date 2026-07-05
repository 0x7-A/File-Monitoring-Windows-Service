using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace FileMonitoringWindowsService
{
    public partial class FileMonitoringService : ServiceBase
    {

        private string _SourceDirectory;
        private string _DestinationDirectory;
        private string _LogDirectory;
        private string _LogFileName;
        private string _LogFilePath;

        private FileSystemWatcher _watcher;

        public FileMonitoringService()
        {
            InitializeComponent();

            CanPauseAndContinue = true; //The service supports pausing and resuming operations.
            CanShutdown = true; // The service is notified when the system shuts down.



            _SourceDirectory = ConfigurationManager.AppSettings["SourceFolder"].ToString();
            _DestinationDirectory = ConfigurationManager.AppSettings["DestinationFolder"].ToString();
            _LogDirectory = ConfigurationManager.AppSettings["LogFolder"];
            _LogFileName = ConfigurationManager.AppSettings["LogFileName"];


            if (string.IsNullOrWhiteSpace(_SourceDirectory))
            {
                _SourceDirectory = @"C:\FileMonitoring\Source";
                LogServiceEvent(@"Default Sourced Folder Used.  C:\FileMonitoring\Source.");

            }
            if (string.IsNullOrWhiteSpace(_DestinationDirectory))
            {
                _DestinationDirectory = @"C:\FileMonitoring\Destination";
                LogServiceEvent(@"Default Destination Folder Used.  C:\FileMonitoring\Destination.");
            }
            if (string.IsNullOrWhiteSpace(_LogDirectory))
            {
               _LogDirectory = @"C:\FileMonitoring\Logs"; 
                LogServiceEvent(@"Default Log Folder Used.  C:\FileMonitoring\Logs");

            }
            if (string.IsNullOrWhiteSpace(_LogFileName))
            {
                _LogFileName = @"ServiceStateLog.txt";
                LogServiceEvent(@"Default Log Folder Used.  C:\FileMonitoring\Logs");
            }


            _LogFilePath = Path.Combine(_LogDirectory, _LogFileName);

            CheckDirectoryExistsAndCreate();

        }

        protected override void OnStart(string[] args)
        { 
            _watcher = new FileSystemWatcher(_SourceDirectory);
           
            _watcher.Created += OnSourceFolderCreatedFile;
            _watcher.IncludeSubdirectories = false;
            _watcher.Filter = "*.*";

            _watcher.EnableRaisingEvents = true;


            LogServiceEvent("Service Started");
        }

        protected override void OnStop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Created -= OnSourceFolderCreatedFile; 
                _watcher.Dispose();
            }
            LogServiceEvent("Service Stopped");
        }


        protected override void OnPause()
        {
            _watcher.EnableRaisingEvents = false;
            LogServiceEvent("Service Paused");
        }

        // OnContinue Event
        protected override void OnContinue()
        {
            _watcher.EnableRaisingEvents = true;
            LogServiceEvent("Service Resumed");
        }

        // OnShutdown Event
        protected override void OnShutdown()
        {
            LogServiceEvent("Service Shutdown due to system shutdown");
        }


        private void OnSourceFolderCreatedFile(object source, FileSystemEventArgs e)
        {
            try
            {
                LogServiceEvent($"File detected:  {e.FullPath} ");

                string extension = System.IO.Path.GetExtension(e.FullPath);
                string NewFilePath = System.IO.Path.Combine(_DestinationDirectory, Guid.NewGuid().ToString() + extension);

                // 1- delete from source
                // 2- adding it to dest
                // 3- renameing it 
                System.IO.File.Move(e.FullPath, NewFilePath);


                LogServiceEvent($"File Moved: {NewFilePath} ");
            }
            catch (Exception error)
            {
                LogServiceEvent(error.Message);
            }

        }

       
        private void CheckDirectoryExistsAndCreate()
        {
            try
            {
                if (!Directory.Exists(_SourceDirectory))
                { 
                   Directory.CreateDirectory(_SourceDirectory);
                }
                if (!Directory.Exists(_DestinationDirectory))
                {
                    Directory.CreateDirectory(_DestinationDirectory);
                }
                if (!Directory.Exists(_LogDirectory))
                {
                    Directory.CreateDirectory(_LogDirectory);
                }


            }
            catch (Exception error)
            {
                LogServiceEvent(error.Message);
            }

        }


        private void LogServiceEvent(string message)
        {
            string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n";
          
            if (Environment.UserInteractive)
            {
                Console.WriteLine(logMessage);
                return;
            }

            File.AppendAllText(_LogFilePath, logMessage);

        }



        public void StartInConsole()
        {
            OnStart(null);
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine();

            OnStop();

        }



    }

}
