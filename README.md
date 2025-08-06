# ProductCatalog Sample (.NET 8, PostgreSQL, CQRS)

This repository contains a minimal **Product Catalog API** illustrating:

* Clean separation of **Domain model** (`ProductCatalog.Domain`) and **API layer** (`ProductCatalog.Api`)
* **LTS Entity Framework Core 8** with **Npgsql** provider for PostgreSQL
* CQRS with dispatcher pattern
* API‑key header authentication
* Soft‑delete auditing
* Seed data (customers, tickets) aligned with TM Forum information model

## Getting started

```bash
# 1. Restore & build from root directory
dotnet restore
dotnet build

# 2. Apply migrations (create one first if needed)
dotnet ef migrations add InitialCreate -p ProductCatalog.Api -s ProductCatalog.Api
dotnet ef database update -p ProductCatalog.Api -s ProductCatalog.Api

# 3. Run the API
dotnet run --project ProductCatalog.Api
```

Open <https://localhost:5000/swagger/index.html>  
Click **Authorize**, type `ApiKey: mUOL5/M40,{{P#zOp8+.E5TkBLy:gH`, and explore the endpoints.

