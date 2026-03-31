![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)
![Angular 19](https://img.shields.io/badge/Angular-19-dd0031?logo=angular)
![License](https://img.shields.io/badge/License-MIT-green)
# App Stock
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

## ⚙️ Setup
1. Configure the connection string in the `appsettings.json` file.
2. Configure the physical paths for saving the images, also in the `appsettings.json` file.
3. I use IIS to serve the image files, but you can use any available service; it just needs to point to the `static` path you defined in the previous step.
4. In the frontend folder, run:   ```bash
   npm install
   ng serve
   ```
5. The application will be available at http://localhost:4200

## 📝 Features
- **Automatic Auditing:** Via the EF Core interceptor that manages `CreatedAt` and `UpdatedAt`.
- **Duplicate Validation:** Integrated logic in the save flow based on the triad (Box/Item/Brand).
- **Reactive UI:** Extensive use of **Angular Signals** for state management and computed data for summarizing.
- **Dynamic Customization:** Colors, icons, and visibility (Scopes) controlled directly from the database for Categories and Brands.

## 🛠️ Migrations
The following details the commands needed to manage data persistence (if you need to revert to the initial state, remember to back up the contents of these existing migration files before restarting).

```bash
dotnet ef migrations add InitialCreate --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialData --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialBoxesSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialItemsSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialStorageSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef migrations add AddInitialCategoriesSPs --project Stock.Infrastructure --startup-project Stock.Api
dotnet ef database update --project Stock.Infrastructure --startup-project Stock.Api
```

### Restore to initial migration
To revert the database to its initial state:
```bash
dotnet ef database update 0 --project Stock.Infrastructure --startup-project Stock.Api
```