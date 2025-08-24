# Patient Management System

A mini **Patient Management System** built using **.NET 8 Web API**, **SQL Server**, and **Sitecore (documented Information Architecture)**.  
The solution demonstrates clean API design, business rules, repository/service pattern, JWT authentication, unit testing, and SQL optimization.

---

## 1. API Project

**Path:** `/PatientManagement.API`

- Built with **.NET 8 Web API** 

Run locally:
```bash
cd PatientManagement.API
dotnet run
```

Swagger available at: `https://localhost:5001/swagger`

---

## 2. Test Project

**Path:** `/PatientManagement.Tests`

- Built with **xUnit + Moq + FluentAssertions**  

Run tests:
```bash
dotnet test
```

Coverage can be checked using **coverlet**

---

## 3. Database Scripts

**Path:** `/Documentation/DB Scripts`

- **DB Schema Initial.sql** 
- **ReportingQueries.sql** 
- **SQL_Optimization_AddIndex.sql** → Adds optimized index
- **SQL_Optimization_ExecutionPlan.docx** → Execution plan analysis write-up (before vs after index)

---

## 4. Documentation

**Path:** `/Documentation`

- **Sitecore-Patient-Management.docx** 
- **SQL-Optimization-ExecutionPlan.docx**

---

## Quick Start

1. Run the SQL scripts in `/Documentation/DB Scripts` to create schema, procedures, and indexes.  
2. Update DB connection string and JWT key in appsettings.json.  
3. Run the API project: `dotnet run`  
4. Test with Swagger or Postman.  
5. Run unit tests: `dotnet test`  
6. Review docs in `/Documentation` for Sitecore and SQL design details.