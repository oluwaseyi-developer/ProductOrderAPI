# 📦 Product Order API

## 🚀 Overview
A production-ready **ASP.NET Core Web API** for product catalog management and order processing with **authentication**, **authorization**, and **comprehensive testing**.  

This solution follows **Clean Architecture**, **CQRS**, and **Domain-Driven Design (DDD)** principles to ensure scalability, maintainability, and testability.

---

## 🛠 Tech Stack
- **.NET 9** with **ASP.NET Core**
- **Entity Framework Core 8** (SQL Server)
- **MediatR** for CQRS
- **AutoMapper** for object mapping
- **FluentValidation** for request validation
- **JWT Bearer Authentication**
- **xUnit + Moq** for testing
- **Swagger/OpenAPI** for documentation

---

## 📋 Assumptions
- **User Roles**:  
  - **Admin** → manages products  
  - **Customer** → places orders
- **Stock Management**: Prevent overselling using EF Core concurrency tokens.
- **Authentication**: JWT tokens (1-hour expiration).
- **Error Handling**: Consistent JSON error responses with HTTP status codes.
- **API Versioning**: Ready for future updates.
- **Logging**: Structured logging for production monitoring.

---

## 🏗 Architecture
```
ProductOrderApi/
├── API              → Presentation Layer
├── Application      → Use Cases, CQRS Handlers
├── Domain           → Entities, Interfaces, Events
└── Infrastructure   → Persistence, External Services
```

---

## ⚙️ Setup Instructions

### ✅ Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB or full instance)
- [Git](https://git-scm.com/)

### 1. Clone the Repository
```bash
git clone <your-github-repo-url>
cd ProductOrderApi
```

### 2. Configure Database
Update **appsettings.json** with your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProductOrderApi;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Migrations
```bash
dotnet ef database update   --project src/ProductOrderApi.Infrastructure   --startup-project src/ProductOrderApi.API
```

### 4. Run the Application
```bash
dotnet run --project src/ProductOrderApi.API
```
API available at:
- https://localhost:7000  
- http://localhost:5000  

### 5. Access Swagger Docs
Navigate to:  
👉 `https://localhost:7000/swagger`

---

## 👤 Default Users
Two seeded users are available for testing:

**Admin User**
- Email: `admin@example.com`
- Password: `Admin123!`
- Roles: `Admin`

**Customer User**
- Email: `user@example.com`
- Password: `User123!`
- Roles: `Customer`

---

## 🧪 Running Tests
```bash
# Run all tests
dotnet test

# Run specific test projects
dotnet test tests/ProductOrderApi.Domain.UnitTests
dotnet test tests/ProductOrderApi.Application.UnitTests
```

---

## 📁 Project Structure
```
src/
├── ProductOrderApi.API/            # Controllers, Middleware
├── ProductOrderApi.Application/    # Commands, Queries, Services
├── ProductOrderApi.Domain/         # Entities, Interfaces, Events
└── ProductOrderApi.Infrastructure/ # Persistence, Repositories

tests/
├── ProductOrderApi.Domain.UnitTests/
└── ProductOrderApi.Application.UnitTests/
```

---

## 🔐 Authentication Endpoints
- `POST /api/auth/register` → Register new user  
- `POST /api/auth/login` → Login & receive JWT  

---

## 🛍 API Endpoints

### Products (Admin only)
- `GET /api/products` → Get all products *(public)*
- `GET /api/products/{id}` → Get product by ID *(public)*
- `POST /api/products` → Create product *(Admin)*
- `PUT /api/products/{id}` → Update product *(Admin)*
- `DELETE /api/products/{id}` → Delete product *(Admin)*

### Orders (Authenticated users)
- `GET /api/orders` → Get current user’s orders
- `GET /api/orders/{id}` → Get order details
- `POST /api/orders` → Place new order

---

## 🎯 Key Features
- ✅ Clean Architecture with separation of concerns  
- ✅ CQRS Pattern using MediatR  
- ✅ Repository + Unit of Work pattern  
- ✅ JWT Authentication with role-based authorization  
- ✅ Concurrency Control (EF Core `[Timestamp]`)  
- ✅ Domain Events (e.g., email notifications)  
- ✅ Global Exception Handling  
- ✅ Request Validation (FluentValidation)  
- ✅ API Versioning support  
- ✅ Comprehensive Unit Testing  

---

## 🚨 Important Notes
- **Concurrency**: Stock updates are protected with concurrency tokens.  
- **Transactions**: Orders processed in atomic transactions.  
- **Security**: Passwords hashed with BCrypt.  
- **Error Handling**: Unified error format across endpoints.  
- **Logging**: Structured logging for observability.  

---

## 📈 Production Considerations
- Add **Redis caching** for hot data
- Implement **rate limiting**
- Configure **health checks**
- Apply strict **CORS policies**
- Use **environment-specific config**
- Add **distributed tracing** for microservices
- Set up **CI/CD pipeline**

---

## 📝 License
This project was created as part of a **technical assessment** for a job application.  
