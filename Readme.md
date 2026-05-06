# Inventory App
Technical guide for the Inventory Control Application

![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)
![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-006dc1?logo=microsoft%20sql%20server&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-%23ff4438.svg?logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)
![Keycloak](https://img.shields.io/badge/Auth-Keycloak-008aaa?logo=keycloak&logoColor=white)
![Angular 21](https://img.shields.io/badge/Angular-21-dd0031?logo=angular)
![Languages](https://img.shields.io/badge/Languages-en%20%7C%20es--MX-orange)
![License](https://img.shields.io/badge/License-MIT-green)

## 🎯 Overview
Inventory App is a comprehensive stock control system designed under **Clean Architecture** principles. It doesn't just manage physical inventory; it ensures data integrity and availability through a coordinated micro-services architecture for temporary file cleanup and automated cloud backups.


## 🧭 Content
- [🏗️ Domain Structure](#️-domain-structure)
- [🚀 Technologies & Tools](#-technologies--tools)
- [⚙️ Setup & Configuration](#️-setup--configuration)
- [🐳 Container Infrastructure](#-container-infrastructure)
- [🛡️ Resilience Strategy (DRP)](#️-resilience-strategy-drp)
- [🛠️ Start & Migrations](#️-start--migrations)
- [🌍 Globalization](#-globalization)
- [🔐 Security](#-security)
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
DB_USERNAME=YourUser
DB_PASSWORD=YourPassword123!
DB_CONNECTION=Server=stock-db,1433;Database=StockDb;User Id=YourUser;Password=YourPassword;TrustServerCertificate=True;
DB_BACKUP_CONNECTION=Server=localhost;Database=StockDb;User Id=YourUser;Password=YourPassword;TrustServerCertificate=True;
DB_BACKUP_PATH=/var/opt/mssql/temp
STATIC_STORAGE_PATH=/host_mnt/c/your/path/Storage/front/static
GOOGLE_DRIVE_FOLDER_ID=YourGoogleDriveFolderId
GOOGLE_CLIENT_ID=YourGoogleDriveClientId
GOOGLE_CLIENT_SECRET=YourGoogleDriveSecret
GOOGLE_REFRESH_TOKEN=YourGoogleDriveRefreshToken
KEYCLOAK_ADMIN_PASSWORD=admin
WORKER_USER_ID=Storage OAuth
WORKER_APP_NAME=Storage-Data-Backup
WORKER_BACKUP_RUN_AT_STARTUP=true
WORKER_BACKUP_RUN_AFTER_MINUTES=60
```
This ensures your private configuration and connection strings are decoupled from the code.

### 3. Database Configuration
The system is designed for **Zero-Configuration Deployment**. When you start the containers for the first time, the SQL Server instance automatically initializes the required databases using the internal setup scripts.

#### **Automatic Initialization**
The infrastructure guarantees the creation of the following tables:
*   **StockDb** 
*   **KeycloakDb:** Created with `READ_COMMITTED_SNAPSHOT ON` to support Keycloak's high-concurrency requirements.

#### **Manual Inspection (Optional)**
If you need to verify the state of the databases or perform manual maintenance, you can connect to the instance using any SQL client (like SSMS or Azure Data Studio) with the credentials defined in your `.env` file:

*   **Server:** `localhost,1433`
*   **Authentication:** SQL Server Authentication
*   **Database:** `StockDb` / `KeycloakDb`

### 4. Users (Keycloak)
When the container is created, it reads the file `infra\auth\realm-export.json`, which contains the configuration for creating the realm, roles, and users needed to get started.

## 🐳 Container Infrastructure
The system orchestrates 6 specialized services:

| Container | Function | Access |
| :--- | :--- | :--- |
| **stock-db** | SQL Server 2022 (Linux) w/ UTF8 Collation | Port 1433 |
| **stock-back** | Main API (ASP.NET Core) | `http://localhost:5000` |
| **stock-front** | Web Application (Angular) | `http://localhost:4200` |
| **stock-worker** | Background Service (Backups & Cloud Sync) | Logs via Docker |
| **stock-cdn** | Image Server (Nginx) | `http://localhost/cdn/` |
| **stock-cleaner** | `/temp` folder cleanup (Python) | Automated |
| **stock-cache** | Fast persistence engine (Redis) | Port 6379 |

> [!TIP]
> **Port Conflicts:** If you have a local IIS service running on port 80, you must stop it (`iisreset /stop`) before starting the containers to avoid conflicts with the **stock-cdn**.


## 🛡️ Resilience Strategy (DRP)
This application implements an automated **Disaster Recovery Plan (DRP)**:

### 1. Cloud Sync (Files)
The `stock-worker` utilizes a **FileSystemWatcher** to detect new images or synchronization files. Once a file is created, it is automatically uploaded to Google Drive, preserving the original folder structure.

### 2. Database Backup (Scheduled)
Every **12 hours**, the Worker instructs SQL Server to generate a Full Backup (`.bak`).
- **Local Resilience:** The file is saved directly to the Host's physical volume.
- **Cloud Resilience:** The Worker detects the new backup and uploads it immediately to the cloud.
- **Self-Healing:** On startup, the Worker verifies the last existing backup to decide whether to execute a new one, avoiding unnecessary duplication.


## 🛠️ Start & Migrations

### Execution Scripts
The `/setup/` folder contains PowerShell scripts to streamline the environment for example:
- `.\setup\start-all.ps1`: Starts the entire ecosystem.
- `.\setup\start-cdn.ps1`: Only starts the image server (useful for local API debugging).
- `.\setup\start-quick-commands.ps1`: injects custom functions into your PowerShell Profile to simplify the Git workflow.


### Database Schema Evolution
The project follows a Code-First approach. The following migrations establish the core structure, stored procedures (SPs), and seed data:

```powershell
dotnet ef migrations add InitialCreate --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialData --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialProcedures --project Stock.Infrastructure --startup-project Stock.Api
```

### Database Management
```powershell
# Add new migration
dotnet ef migrations add <Name> --project Stock.Infrastructure --startup-project Stock.Api
# Update database
dotnet ef database update --project Stock.Infrastructure --startup-project Stock.Api
# Revert the database to its initial state
dotnet ef database update 0 --project Stock.Infrastructure --startup-project Stock.Api
```

## 🌍 Globalization

To maintain translation integrity in the frontend, we use a Strong Typing system. This prevents typos when calling translation labels from HTML or components.

### Key Synchronization (Database to TypeScript)

When new labels are added to the database (Label table), the Angular type contract must be updated.

#### 1. Union Type Generation (SQL Server)
Run the following script in your database environment to get the updated type string:

```sql
SELECT 
    'export type GlobalizationKey = ' + 
    STRING_AGG(CAST('''' + Context + '.' + LabelKey + '''' AS VARCHAR(MAX)), ' | ') + ';' 
    AS TypeScriptType
FROM Label;
```

#### 2. Update in the Angular Project
Once you have the script result (the TypeScriptType column):

1. Open the type definition file: `src/app/core/types/globalization-keys.ts`
2. Replace the existing content with the new result.
3. The Angular compiler will automatically detect the new keys and validate their use in the TranslateDirective.

Note: By using Clean Architecture, we ensure the presentation layer does not rely on `magic strings`, but on a defined contract that guarantees each requested label actually exists in the database.

### Template Implementation Example

Thanks to this synchronization, usage in components is completely type-safe:

```html
<span translate="Storage.SELECT_BOX"></span>

<span [translate]="'Storage.WRONG_KEY'"></span>
```

## 🔐 Security

### Identity and Access Management (IAM)
This project implements a layered security architecture, delegating identity management to Keycloak. 
This approach provides:
- **Single Sign-On (SSO)**: Centralized authentication.
- **Role-Based Access Control (RBAC)**: Permissions managed via specific roles.
- **JWT Tokens**: Secure and standardized communication between the Frontend and the Backend API.

### Users and Roles
The system defines the following profiles to ensure users only access the data and actions they are authorized to perform:
| Role | Descripción | Capabilities |
| :--- | :--- | :--- |
| **viewer** | General inquiry user | Read-only access. Cannot perform any action. |
| **editor** | Catalog manager | Authorized to edit brands and categories. Access to administrative forms and catalogs. |
| **operator** | Inventory manager | Authorized to move stock and handle inventory actions. |
| **admin** | Full system access | Can manage both inventory, catalogs, and system configurations. |

> [!TIP]
> To facilitate testing, the system includes pre-configured users for each role. You can find the credentials in the `infra/auth/realm-export.json` file or use the `admin` account created at startup.

## 📝 Key Features
- **Zero-Config Infrastructure:** Automated environment setup via Docker, including auto-initialization of databases with specialized configurations (UTF8 & Snapshot Isolation).
- **Hybrid Storage & Cloud Sync:** Files are served locally via Nginx (CDN) for maximum speed but are automatically synchronized to Google Drive for disaster recovery.
- **Automated Resilience (DRP):** Integrated background worker that performs scheduled Full Backups and immediate cloud uploads.
- **Soft Delete Sync:** Support for cloud file removal via temporary markers.
- **Reactive UI:** Built with **Angular Signals**, providing a fluid and high-performance user experience with granular state management.
- **Type-Safe Globalization:** Automated synchronization between SQL Server labels and TypeScript Union Types to eliminate magic strings.
- **Lazy-Loaded Architecture:** Highly optimized bundle sizes with specific chunking for Material components and feature modules.
- **Clean Architecture:** Strict separation of concerns (Domain, Application, Infrastructure, and Presentation) ensuring a maintainable and scalable codebase.
- **Robust Security (IAM):** Granular Role-Based Access Control (RBAC) managed by Keycloak, protecting every API endpoint and UI component.
- **Smart Audit System:** EF Core interceptors for automated management of `CreatedAt` and `UpdatedAt` timestamps across all entities.