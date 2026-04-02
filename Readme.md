![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)
![Angular 21](https://img.shields.io/badge/Angular-21-dd0031?logo=angular)
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

## 🛠️ Setup

Follow these steps to configure your development environment.

### 1. Prerequisites
* **Docker Desktop** (Must be running).
* **SQL Server** (Local or network accessible).
* **Node.js & Angular CLI** (For the frontend).

---

### 2. Infrastructure & CDN (Docker)
The system uses an **Nginx** container to serve images with pre-configured CORS.

> [!IMPORTANT]  
> If you have a local IIS service running on port 80, you must stop it (`iisreset /stop`) before starting the containers to avoid port conflicts.

In the `/setup/` folder, you will find PowerShell scripts to manage the services:

* **To work with the API from your IDE (Visual Studio/VS Code):**
  Run: `.\setup\start-cdn.ps1`  
  *This will only start the image server at http://localhost/cdn/.*

* **To start the backend environment (API + CDN):**
  Run: `.\setup\start-back.ps1`

* **To start the full environment (All containers):**
  Run: `.\setup\start-all.ps1`

---

### 3. Backend Configuration
1. Configure your connection string in the `appsettings.json` file.
2. Define the physical paths for saving images in the same file.
3. Ensure the physical path matches the volume configured in `docker-compose.yml` so the CDN can display them.

---

### 4. Frontend
Inside the frontend folder, run the standard commands:
1. `npm install`
2. `npm start` (or the script configured in your package.json).

---

### 5. Service Access
* **Application (Frontend):** http://localhost:4200
* **Data API:** http://localhost:5000
* **Image CDN:** http://localhost/cdn/

---

**Note on Persistence:** The containers are configured with volumes. Even if you stop or delete the containers, your physical images and configuration data will remain intact.

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