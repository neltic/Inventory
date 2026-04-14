![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)
![Angular 21](https://img.shields.io/badge/Angular-21-dd0031?logo=angular)
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-006dc1?logo=microsoft%20sql%20server&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-%23ff4438.svg?logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-green)
# Inventory App
Technical guide for the Inventory Control Application

## 📋 Content
- [Technologies](#technologies)
- [Domain Structure](#domain-structure)
- [Setup](#setup)
- [Features](#features)
- [Migrations](#migrations)

## 🚀 Technologies
- **Backend:** .NET 9.0 (C# 13), Clean Architecture, Entity Framework Core.
- **Frontend:** Angular 21, Signals, Angular Material.

## 🏗️ Domain Structure
The system manages a hierarchical inventory based on:
- **Boxes:** Containers that support nesting (boxes within boxes).
- **Items:** Product definitions from the catalog.
- **Storage:** A record of actual stock levels, linking Box + Item + Brand.

## Setup

Follow these steps to configure your development environment.

### 1. Prerequisites
* Docker Desktop (Must be running).
* SQL Server (Local or network accessible).
* .NET 8 SDK & Node.js / Angular CLI.

---

### 2. Environment Configuration (.env)
The project uses a .env file to manage local paths and credentials. Create a file named .env in the root directory:

```
STATIC_STORAGE_PATH=/host_mnt/c/your/path/Storage/front/static
DB_CONNECTION="Server=host.docker.internal,1433;Database=StockDb;User Id=YourUser;Password=YourPassword;TrustServerCertificate=True;"
GOOGLE_DRIVE_FOLDER_ID=YourGoogleDriveFolderId
GOOGLE_CLIENT_ID=YourGoogleDriveClientId
GOOGLE_CLIENT_SECRET=YourGoogleDriveSecret
GOOGLE_REFRESH_TOKEN=YourGoogleDriveRefreshToken
WORKER_USER_ID=Storage OAuth
WORKER_APP_NAME=Storage-Data-Backup
```

This ensures your private configuration and connection strings are decoupled from the code.

---

### 3. Infrastructure & CDN (Docker)
The system orchestrates three specialized containers:
* stock-cdn: Nginx image server at http://localhost/cdn/
* stock-cleaner: Python service that automatically deletes files in /temp older than 24 hours.
* stock-back: Containerized API environment.

Important: If you have a local IIS service running on port 80, you must stop it (`iisreset /stop`) before starting the containers to avoid port conflicts.

In the `/setup/` folder, you will find PowerShell scripts to manage the services:
* **To work with the API from your IDE (Visual Studio/VS Code):**
  Run: `.\setup\start-cdn.ps1`  
  *This will only start the image server at http://localhost/cdn/.*

* **To start the backend environment (API + CDN):**
  Run: `.\setup\start-back.ps1`

* **To start the full environment (All containers):**
  Run: `.\setup\start-all.ps1`

---

### 4. Data Persistence & Migrations
The project uses a Hybrid Migration Strategy:
* Schema: Managed by standard EF Core Migrations.
* Persistent Data: SQL scripts (Brands, Categories, etc.) are stored in `Persistence` folder.
* Logic: These scripts are linked via Partial Classes to the migrations, ensuring your data-seeding logic survives database resets or migration cleanups.

---

### 5. Service Access
* Frontend Application: http://localhost:4200
* Data API: http://localhost:5000
* Image CDN: http://localhost/cdn/
* Cleanup Service: Monitors the /temp folder automatically.

---

### 6. Development Workflow
1. Frontend: Run `npm install` for the first time and then `npm start`.
2. Backend: Open `Stock.slnx` and select the `AppStock-Dev` profile in Visual Studio.
3. Verification: Use `docker logs -f stock-cleaner` to verify the automated cleanup service.

Note on Persistence: The system uses Docker Volumes to map your physical Storage folder to the containers. Even if you stop or delete the containers, your physical images and configuration data will remain intact.

## 📝 Features
- **Automatic Auditing:** Via the EF Core interceptor that manages `CreatedAt` and `UpdatedAt`.
- **Duplicate Validation:** Integrated logic in the save flow based on the triad (Box/Item/Brand).
- **Reactive UI:** Extensive use of **Angular Signals** for state management and computed data.
- **Dynamic Customization:** Colors, icons, and visibility (Scopes) controlled directly from the database for Categories and Brands.

## 🛠️ Migrations
The following details the commands needed to manage data persistence (If you want to return to the initial state and add something different from the beginning).

```bash
dotnet ef migrations add InitialCreate --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialData --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialBoxesSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialItemsSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialStorageSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialCategoriesSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddFeatBoxTransferSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef database update --project Stock.Infrastructure --startup-project Stock.Api
```

### Restore to initial migration
To revert the database to its initial state:
```bash
dotnet ef database update 0 --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef database remove --project Stock.Infrastructure --startup-project Stock.Api
```