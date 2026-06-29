# Deploy

This project is ready to publish as an ASP.NET Core MVC application with SQL Server.

## Required production settings

Configure these values on the hosting platform as environment variables or secrets:

| Setting | Purpose |
|---|---|
| `ASPNETCORE_ENVIRONMENT=Production` | Enables production error handling and HSTS. |
| `ConnectionStrings__InventoryManagementDbContext` | SQL Server connection string for the deployed database. |
| `Authentication__Google__ClientId` | Optional Google login client id. |
| `Authentication__Google__ClientSecret` | Optional Google login client secret. |
| `OpenAI__ApiKey` | Optional key for product AI advice. |
| `Database__ApplyMigrations=true` | Optional startup migration switch. Use only when the deployment process should migrate the database automatically. |

## Publish locally

From the `InventoryManagement` folder:

```powershell
dotnet publish InventoryManagement/InventoryManagement.csproj -c Release -o ./publish
```

Run the published app:

```powershell
$env:ASPNETCORE_ENVIRONMENT="Production"
$env:ConnectionStrings__InventoryManagementDbContext="Server=YOUR_SERVER;Database=InventoryManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
dotnet ./publish/InventoryManagement.dll
```

## Docker

Build the container:

```powershell
docker build -t inventory-management .
```

Run the container:

```powershell
docker run --rm -p 8080:8080 `
  -e ConnectionStrings__InventoryManagementDbContext="Server=host.docker.internal;Database=InventoryManagementDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True" `
  inventory-management
```

Open `http://localhost:8080/health` to verify that the app is running.

## Deployment checklist

- Build in `Release` configuration.
- Set the production SQL Server connection string as a secret, not in source control.
- Apply EF Core migrations before traffic is routed to the app, or explicitly enable `Database__ApplyMigrations=true`.
- Keep uploaded files in persistent storage if product attachments are used in production.
- Verify `/health`, login, dashboard, CRUD screens, API endpoints, and file logging after deployment.
