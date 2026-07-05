File-Monitoring-Windows-Service
A robust Windows Service built in C# (.NET Framework) designed to monitor a specific source directory for newly created files. Once detected, the service automatically processes the files, applies unique renaming logic, moves them to a destination folder, and maintains logs of all activities and errors.

Features
Real-time File Monitoring: Utilizes system event handlers to immediately catch file creation events.

Automated Processing: Automatically renames processed files to a unique GUID to prevent naming collisions in the destination directory.

Clean Cleanup: Safely transfers files to the destination folder and ensures they are deleted from the source path.

Full-State Event Logging: Tracks the complete lifecycle of service operations, including successful transfers, structural events, and exceptions directly into the Windows Event Log.

Dynamic Configuration: Easily adjust the source folder path, destination path, and other parameters through the App.config file without needing to recompile the service.

Getting Started
Prerequisites
Windows OS

.NET Framework (compatible with the version used in your project)

Administrator privileges (required to install and start Windows Services)

Configuration
Before running or installing the service, open the App.config file to configure your target directories:

XML
<appSettings>
  <add key="SourceFolder" value="C:\Your\Source\Path" />
  <add key="DestinationFolder" value="C:\Your\Destination\Path" />
</appSettings>
Installation & Deployment
Since this is a Windows Service, it cannot be run simply by double-clicking the executable. You must register it using the Visual Studio Developer Command Prompt.

1. Install the Service
Open the Developer Command Prompt for Visual Studio as Administrator and run the following command:

Bash
InstallUtil.exe "C:\Path\To\Your\FileMonitoringWindowsService.exe"
2. Start the Service
You can start the service either via the Windows Services Manager (services.msc) or directly through the command line:

Bash
net start FileMonitoringWindowsService
3. Uninstall the Service
To remove the service from your machine, run:

Bash
InstallUtil.exe /u "C:\Path\To\Your\FileMonitoringWindowsService.exe"
How it Works
The service reads the configured folders from App.config on startup and initializes a file system watcher.

When a file enters the source folder, the service captures the event, locks the file safely for processing, and generates a new Guid.

It moves the file to the destination folder under its new name (GUID.extension).

All lifecycle actions, including state transitions and error exceptions, are neatly logged for system administration auditing.
