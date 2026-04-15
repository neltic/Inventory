# Inventory App
Technical guide for the Inventory Control Application

![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)
![Angular 21](https://img.shields.io/badge/Angular-21-dd0031?logo=angular)
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-006dc1?logo=microsoft%20sql%20server&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-%23ff4438.svg?logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)

## 🎯 Overview
Inventory App is a comprehensive stock control system designed under **Clean Architecture** principles. It doesn't just manage physical inventory; it ensures data integrity and availability through a coordinated micro-services architecture for temporary file cleanup and automated cloud backups.


## 🧭 Content
- [🏗️ Domain Structure](#️-domain-structure)
- [🚀 Technologies & Tools](#-technologies--tools)
- [⚙️ Setup & Configuration](#️-setup--configuration)
- [🐳 Container Infrastructure](#-container-infrastructure)
- [🛡️ Resilience Strategy (DRP)](#️-resilience-strategy-drp)
- [🛠️ Development & Migrations](#️-development--migrations)
- [📝 Key Features](#-key-features)


## 🏗️ Domain Structure
The system uses a logical hierarchy for stock control:
- **Boxes:** Containers with support for infinite nesting (Box in Box).
- **Items:** Global product definitions from the catalog.
- **Storage:** The actual stock record, linking the triad: **Box + Item + Brand**.


## 🚀 Technologies & Tools
- **Backend:** .NET 9.0 (C# 13), EF Core, SqlClient.
- **Frontend:** Angular 21, Signals, Angular Material.
- **Cache:** Redis for frequent query optimization.
- **Storage:** Hybrid system (Local + Google Drive API).
- **Infrastructure:** Docker & Nginx as CDN.


## ⚙️ Setup & Configuration

### 1. Prerequisites
* Docker Desktop must be running.
* .NET 9 SDK & Node.js / Angular CLI.
* A Google Cloud account (for Google Drive API credentials).

### 2. Environment Configuration (.env)
Configure the `.env` file in the root directory. This file acts as the bridge between your Host and the containers:

```env
STATIC_STORAGE_PATH=/host_mnt/c/your/path/Storage/front/static
DB_CONNECTION="Server=host.docker.internal,1433;Database=StockDb;User Id=YourUser;Password=YourPassword;TrustServerCertificate=True;"
DB_BACKUP_CONNECTION=Server=YourLocalServer;Database=StockDb;User Id=YourUser;Password=YourPassword;TrustServerCertificate=True;
DB_BACKUP_PATH=C:\your\path\Storage\front\static\temp
GOOGLE_DRIVE_FOLDER_ID=YourGoogleDriveFolderId
GOOGLE_CLIENT_ID=YourGoogleDriveClientId
GOOGLE_CLIENT_SECRET=YourGoogleDriveSecret
GOOGLE_REFRESH_TOKEN=YourGoogleDriveRefreshToken
WORKER_USER_ID=Storage OAuth
WORKER_APP_NAME=Storage-Data-Backup
```
This ensures your private configuration and connection strings are decoupled from the code.


## 🐳 Container Infrastructure
The system orchestrates 6 specialized services:

| Container | Function | Access |
| :--- | :--- | :--- |
| **stock-back** | Main API (ASP.NET Core) | `http://localhost:5000` |
| **stock-front** | Web Application (Angular) | `http://localhost:4200` |
| **stock-worker** | Background Service (Backups & Cloud Sync) | Logs via Docker |
| **stock-cdn** | Image Server (Nginx) | `http://localhost/cdn/` |
| **stock-cleaner** | `/temp` folder cleanup (Python) | Automated |
| **stock-cache** | Fast persistence engine (Redis) | Port 6379 |

Important: If you have a local IIS service running on port 80, you must stop it (`iisreset /stop`) before starting the containers to avoid port conflicts.


## 🛡️ Resilience Strategy (DRP)
This application implements an automated **Disaster Recovery Plan (DRP)**:

### 1. Cloud Sync (Files)
The `stock-worker` utilizes a **FileSystemWatcher** to detect new images or synchronization files. Once a file is created, it is automatically uploaded to Google Drive, preserving the original folder structure.

### 2. Database Backup (Scheduled)
Every **12 hours**, the Worker instructs SQL Server to generate a Full Backup (`.bak`).
- **Local Resilience:** The file is saved directly to the Host's physical volume.
- **Cloud Resilience:** The Worker detects the new backup and uploads it immediately to the cloud.
- **Self-Healing:** On startup, the Worker verifies the last existing backup to decide whether to execute a new one, avoiding unnecessary duplication.


## 🛠️ Development & Migrations

### Execution Scripts
The `/setup/` folder contains PowerShell scripts to streamline the environment for example:
- `.\setup\start-all.ps1`: Starts the entire ecosystem.
- `.\setup\start-cdn.ps1`: Only starts the image server (useful for local API debugging).

### Migrations
The following details the commands needed to manage data persistence (If you want to return to the initial state and add something different from the beginning).

```bash
dotnet ef migrations add InitialCreate --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialData --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialBoxesSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialItemsSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialStorageSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialCategoriesSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddFeatBoxTransferSPs --project Stock.Infrastructure --startup-project Stock.Api
```

### Restore to initial migration
To revert the database to its initial state:
```bash
dotnet ef database update 0 --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef database remove --project Stock.Infrastructure --startup-project Stock.Api
```

### Database Management
```bash
# Add new migration
dotnet ef migrations add <Name> --project Stock.Infrastructure --startup-project Stock.Api
# Update database
dotnet ef database update --project Stock.Infrastructure --startup-project Stock.Api
```


## 📝 Key Features
- **Hybrid Storage:** Files are served locally for speed (CDN) but backed up in the cloud for security.
- **Audit Interceptors:** Automatic management of `CreatedAt` and `UpdatedAt` in EF Core.
- **Reactive State:** Powered by **Angular Signals** for a fluid UI experience without unnecessary refreshes.
- **Soft Delete Sync:** Support for cloud file removal via temporary markers.