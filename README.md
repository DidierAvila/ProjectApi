# Post Ltda. API - Comprehensive Documentation

## Project Overview

Post Ltda. API is a robust .NET Core REST API built with Clean Architecture principles, implementing a customer and post management system. The API follows CQRS (Command Query Responsibility Segregation) pattern with MediatR, providing a scalable and maintainable solution for content management.

## 🏗️ Architecture

The project implements Clean Architecture with clear separation of concerns:

```
┌─────────────────┐
│   API Layer     │  ← Controllers, HTTP endpoints, Swagger
├─────────────────┤
│ Business Layer  │  ← CQRS Commands/Queries, Handlers, Validators
├─────────────────┤
│  Domain Layer   │  ← Entities, DTOs, Business Rules
├─────────────────┤
│DataAccess Layer │  ← Repository Pattern, Entity Framework
└─────────────────┘
```

## 📚 Documentation Structure

This project includes comprehensive documentation for all layers:

### 🌐 [API Documentation](./API_DOCUMENTATION.md)
Complete REST API reference with:
- All endpoint specifications
- Request/response models
- HTTP status codes
- Usage examples
- Business rules
- Error handling

### 💼 [Business Layer Documentation](./BUSINESS_LAYER_DOCUMENTATION.md)
CQRS implementation details including:
- Commands and Queries
- Handlers and Validators
- AutoMapper configurations
- Pipeline behaviors
- Exception handling

### 🗄️ [Data Access Documentation](./DATA_ACCESS_DOCUMENTATION.md)
Repository pattern and Entity Framework configuration:
- Repository interfaces and implementations
- Database context configuration
- Entity relationships
- Performance optimization
- Migration strategies

## 🚀 Quick Start

### Prerequisites
- .NET 6.0 or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

### 1. Clone and Setup
```bash
git clone <repository-url>
cd ProjectAPI
```

### 2. Database Setup
Choose one of the following options:

**Option A: Restore from backup**
```bash
# Restore JujuTests.bak to your SQL Server instance
```

**Option B: Run SQL script**
```bash
# Execute JujuTests.Script.sql in SQL Server Management Studio
```

### 3. Configure Connection String
Update `API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Development": "Server=localhost;Database=JujuTests;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 4. Run the Application
```bash
cd API
dotnet run
```

### 5. Access Swagger UI
Navigate to: `https://localhost:5001/swagger`

## 🔧 Key Features

### Customer Management
- ✅ Create, read, update, delete customers
- ✅ Unique name validation
- ✅ Cascade delete (removes associated posts)
- ✅ Customer posts retrieval

### Post Management
- ✅ Create, read, update, delete posts
- ✅ Bulk post creation
- ✅ Automatic category assignment based on type
- ✅ Content truncation (97 characters + "...")
- ✅ Customer association validation

### Business Rules
- **Post Categories**: Type 1 → "Farándula", Type 2 → "Política", Type 3 → "Futbol"
- **Content Processing**: Auto-truncation of post body text
- **Data Integrity**: Referential integrity between customers and posts
- **Validation**: Comprehensive input validation with FluentValidation

## 🏛️ Project Structure

```
ProjectAPI/
├── API/                          # Web API layer
│   ├── Controllers/              # REST controllers
│   ├── Extensions/               # API extensions
│   ├── Program.cs               # Application entry point
│   └── Startup.cs               # Service configuration
├── Business/                     # Business logic layer
│   ├── Customers/               # Customer CQRS operations
│   │   ├── Commands/            # Create, Update, Delete
│   │   ├── Queries/             # Read operations
│   │   ├── Handlers/            # Command/Query handlers
│   │   ├── Validators/          # FluentValidation rules
│   │   └── Mappings/            # AutoMapper profiles
│   └── Posts/                   # Post CQRS operations
│       ├── Commands/            # Create, Update, Delete
│       ├── Queries/             # Read operations
│       ├── Handlers/            # Command/Query handlers
│       ├── Validators/          # FluentValidation rules
│       └── Mappings/            # AutoMapper profiles
├── Domain/                       # Domain layer
│   ├── Entities/                # Core business entities
│   ├── Dtos/                    # Data transfer objects
│   └── Validators/              # Domain validators
├── DataAccess/                   # Data access layer
│   ├── DbContexts/              # Entity Framework contexts
│   ├── Repositories/            # Repository implementations
│   └── DataAccessExtension.cs   # DI configuration
├── TestApi/                      # Unit tests
└── Documentation/                # This comprehensive documentation
```

## 🛠️ Technology Stack

### Core Framework
- **.NET 6.0**: Modern, cross-platform framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for database operations

### Architecture Patterns
- **Clean Architecture**: Separation of concerns
- **CQRS**: Command Query Responsibility Segregation
- **Repository Pattern**: Data access abstraction
- **MediatR**: In-process messaging

### Libraries and Tools
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **Serilog**: Structured logging
- **Swagger/OpenAPI**: API documentation
- **SQL Server**: Database engine

## 📊 API Endpoints Overview

### Customer Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Customer` | Get all customers |
| GET | `/Customer/{id}` | Get customer by ID |
| POST | `/Customer` | Create new customer |
| PUT | `/Customer/{id}` | Update customer |
| DELETE | `/Customer/{id}` | Delete customer |
| GET | `/Customer/{id}/posts` | Get customer's posts |

### Post Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/Post` | Get all posts |
| GET | `/Post/{id}` | Get post by ID |
| POST | `/Post` | Create new post |
| POST | `/Post/multiple` | Create multiple posts |
| PUT | `/Post/{id}` | Update post |
| DELETE | `/Post/{id}` | Delete post |
| GET | `/Post/customer/{customerId}` | Get posts by customer |

## 🔒 Security Considerations

### Current State
- No authentication/authorization implemented
- All endpoints are publicly accessible
- Input validation through FluentValidation
- SQL injection protection via Entity Framework

### Recommendations for Production
- Implement JWT authentication
- Add role-based authorization
- Enable HTTPS only
- Add rate limiting
- Implement API versioning

## 📈 Performance Features

### Database Optimization
- **AsNoTracking**: Read-only queries for better performance
- **Connection Pooling**: Automatic EF Core connection management
- **Async/Await**: Non-blocking operations throughout

### Caching Strategies
- Consider implementing Redis for distributed caching
- Add response caching for read-heavy operations
- Implement query result caching

## 🧪 Testing

### Current Test Structure
```
TestApi/
├── UnitTests/           # Unit tests for business logic
├── IntegrationTests/    # API endpoint tests
└── RepositoryTests/     # Data access tests
```

### Running Tests
```bash
cd TestApi
dotnet test
```

### Test Coverage Areas
- Business logic validation
- Repository operations
- API endpoint functionality
- Error handling scenarios

## 🚀 Deployment

### Development Environment
```bash
# Run with development settings
dotnet run --environment Development
```

### Production Deployment
1. **Build the application**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Configure production settings**
   - Update connection strings
   - Set appropriate log levels
   - Configure HTTPS certificates

3. **Deploy to hosting platform**
   - IIS, Azure App Service, Docker, etc.

## 📝 Logging

### Serilog Configuration
- **Console Logging**: Development environment
- **File Logging**: `logs/app-{date}.log`
- **Structured Logging**: JSON format for analysis
- **Log Levels**: Information, Warning, Error

### Log Examples
```csharp
Log.Information("Creating customer with name {CustomerName}", customerName);
Log.Warning("Customer {CustomerId} not found", customerId);
Log.Error(ex, "Failed to create post for customer {CustomerId}", customerId);
```

## 🔄 CI/CD Recommendations

### Build Pipeline
```yaml
# Example GitHub Actions workflow
name: Build and Test
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v2
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 6.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

## 📖 Additional Resources

### Learning Materials
- [Clean Architecture by Robert Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern Documentation](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)

### Related Projects
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [AutoMapper Documentation](https://automapper.org/)
- [FluentValidation Documentation](https://fluentvalidation.net/)

## 🤝 Contributing

### Development Guidelines
1. Follow Clean Architecture principles
2. Write comprehensive unit tests
3. Use meaningful commit messages
4. Update documentation for new features
5. Follow C# coding conventions

### Code Review Checklist
- [ ] Business rules implemented correctly
- [ ] Proper error handling
- [ ] Unit tests added/updated
- [ ] Documentation updated
- [ ] Performance considerations addressed

## 📞 Support

### Issue Reporting
- Use GitHub Issues for bug reports
- Provide detailed reproduction steps
- Include relevant log entries
- Specify environment details

### Contact Information
- **Development Team**: andres.rodriguez@juju.com.co
- **Innovation Team**: cristian.moreno@juju.com.co

---

## 📄 License

This project is proprietary software developed for Post Ltda. All rights reserved.

---

*This documentation is maintained alongside the codebase. For the most up-to-date information, please refer to the linked documentation files and inline code comments.*