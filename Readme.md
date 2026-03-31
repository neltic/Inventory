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
El sistema gestiona un inventario jerárquico basado en:
- **Boxes:** Contenedores que soportan anidación (cajas dentro de cajas).
- **Items:** Definiciones de productos del catálogo.
- **Storage:** Registro de stock real vinculando Box + Item + Brand.

## ⚙️ Setup
1. Configurar la cadena de conexión en el archivo `appsettings.json`.
2. Configurar las rutas físicas para guardar las imágenes también en el archivo `appsettings.json`.
3. Para despachar los archivos de imágenes uso un `IIS`, pero puedes usar cualquiera que tengas a la mano, solo debe apuntar a la ruta `static` que definas en el punto anterior.
4. En la carpeta del frontend, ejecutar:
   ```bash
   npm install
   ng serve
   ```
5. La aplicación estará disponible en http://localhost:4200

## 📝 Features
- **Auditoría automática:** Mediante el interceptor de EF Core que gestiona `CreatedAt` y `UpdatedAt`.
- **Validación de duplicados:** Lógica integrada en el flujo de guardado basada en la triada (Box/Item/Brand).
- **UI Reactiva:** Uso intensivo de **Angular Signals** para la gestión de estados y `computed` para el resumen de datos.
- **Personalización Dinámica:** Colores, iconos y visibilidad (Scopes) controlados directamente desde la base de datos para Categorías y Marcas.


## 🛠️ Migrations
A continuación se detallan los comandos necesarios para gestionar la persistencia de datos (en caso de tener que pasar al estado inicial, no olvides respaldar el contenido de estos archivos de migración que ya existen antes de hacer el reinicio).

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
Para revertir la base de datos al estado inicial:
```bash
dotnet ef database update 0 --project Stock.Infrastructure --startup-project Stock.Api
```