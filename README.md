# InfinityElectronics API

Backend API for the InfinityElectronics e-commerce platform, built with ASP.NET Core and Entity Framework Core.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

## Getting Started

### 1. Clone the repository

```bash
git clone <repo-url>
cd InfinityElectronics
```

### 2. Configure API key

Create `InfinityElectronics.Api/appsettings.Development.json` and add your API key:

```json
{
  "ApiKey": "your-api-key-here"
}
```

### 3. Apply database migrations

```bash
cd InfinityElectronics.Api
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The API will start on `https://localhost:7061`. On startup, product and category data is automatically synchronized from the ERP system.

### 5. Explore the API

Open `https://localhost:7061/swagger` in your browser to access the Swagger UI. Use the **Authorize** button to enter your API key before making requests.
