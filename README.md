# UserWebApi

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (ensure the required version is installed)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or an alternative database supported by the project)
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/)

## Setup and Run the API

### 1. Configure the Database Connection String

#### Option 1: Use SQL Server

1. Open `appsettings.json` or `appsettings.Development.json`.
2. Locate the `ConnectionStrings` section.
3. Update the connection string to match your database configuration. Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
}
```

#### Option 2: Use In-Memory Database

To use an in-memory database instead of SQL Server, update `appsettings.json`:

```json
"UseInMemoryDatabase": true
```

This will bypass the SQL Server connection and use an in-memory store instead.

### 2. Apply Database Migrations - if Option 1: Use SQL Server is selected

Run the following command in the project root directory to apply the latest migrations:

```sh
dotnet ef database update
```

*Note: This step is not required when using the in-memory database.*

If you encounter issues, ensure that Entity Framework Core is installed:

```sh
dotnet tool install --global dotnet-ef
```

### 3. Run the API

Execute the following command:

```sh
dotnet run
```

Alternatively, if using Visual Studio:

1. Open `UserWebApi.sln` in Visual Studio.
2. Select `UserWebApi` as the startup project.
3. Press `F5` to run the application.

### 4. Access the API

Once the API is running, it should be available at:

- Swagger UI: [http://localhost:5000/swagger](http://localhost:5000/swagger) (or the configured port)
- API Endpoints: `http://localhost:5000/api/{controller}/{action}`


