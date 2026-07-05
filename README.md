# File-Monitoring-Windows-Service

---

A robust **Windows Service** built in C# (.NET Framework) designed to monitor a specific source directory for newly created files. Once detected, the service automatically processes the files, applies unique renaming logic, moves them to a destination folder, and maintains logs of all activities and errors.

<br>

## 🛠️ Features

---

* **Real-time File Monitoring:** Utilizes system event handlers to immediately catch file creation events.

* **Automated Processing:** Automatically renames processed files to a unique **GUID** to prevent naming collisions in the destination directory.

* **Clean Cleanup:** Safely transfers files to the destination folder and ensures they are deleted from the source path.

* **Full-State Event Logging:** Tracks the complete lifecycle of service operations, including successful transfers, structural events, and exceptions directly into the Windows Event Log.

* **Dynamic Configuration:** Easily adjust the source folder path, destination path, and other parameters through the `App.config` file without needing to recompile the service.

<br>

## 🚀 Getting Started

---

### Prerequisites

* Windows OS
* .NET Framework (compatible with the version used in your project)
* Administrator privileges (required to install and start Windows Services)

<br>

## ⚙️ Configuration

---

Before running or installing the service, open the `App.config` file to configure your target directories:

```xml
<appSettings>
  <add key="SourceFolder" value="C:\Your\Source\Path" />
  <add key="DestinationFolder" value="C:\Your\Destination\Path" />
</appSettings>
