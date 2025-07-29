# Data Access Layer Documentation

## Overview

The Data Access layer implements the Repository pattern with Entity Framework Core, providing a clean abstraction over database operations. This layer handles all data persistence concerns and maintains separation between business logic and data storage.

## Architecture Components

### Repository Pattern
- **IRepositoryBase<T>**: Generic interface for common CRUD operations
- **RepositoryBase<T>**: Base implementation with Entity Framework operations
- **Specific Repositories**: Customer and Post repositories extending base functionality

### Entity Framework Configuration
- **JujuTestContext**: Main database context with entity configurations
- **Entity Configurations**: Fluent API configurations for database mapping
- **Migration Support**: Code-first approach with automatic migrations

---

## Repository Interfaces

### IRepositoryBase<TEntity>
Generic base interface providing common operations for all entities.

```csharp
public interface IRepositoryBase<TEntity> where TEntity : class
{
    // Retrieval Operations
    Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken);
    Task<TEntity?> GetByID(int id, CancellationToken cancellationToken);
    Task<TEntity?> Find(Expression<Func<TEntity, bool>> expr, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>?> Finds(Expression<Func<TEntity, bool>> expr, CancellationToken cancellationToken);
    
    // Modification Operations
    Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken);
    Task Update(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity?> Delete(int id, CancellationToken cancellationToken);
}
```

#### Method Descriptions

**GetAll(CancellationToken cancellationToken)**
- Retrieves all entities of type T
- Uses `AsNoTracking()` for read-only operations
- Returns: `IEnumerable<TEntity>`

**GetByID(int id, CancellationToken cancellationToken)**
- Retrieves a single entity by primary key
- Uses Entity Framework's `FindAsync` for optimal performance
- Returns: `TEntity?` (null if not found)

**Find(Expression<Func<TEntity, bool>> expr, CancellationToken cancellationToken)**
- Finds the first entity matching the given expression
- Uses `AsNoTracking()` for read-only operations
- Returns: `TEntity?` (null if not found)

**Finds(Expression<Func<TEntity, bool>> expr, CancellationToken cancellationToken)**
- Finds all entities matching the given expression
- Uses `AsNoTracking()` for read-only operations
- Returns: `IEnumerable<TEntity>?`

**Create(TEntity entity, CancellationToken cancellationToken)**
- Adds a new entity to the database
- Automatically saves changes
- Returns: Created `TEntity` with generated ID

**Update(TEntity entity, CancellationToken cancellationToken)**
- Updates an existing entity
- Sets entity state to Modified
- Automatically saves changes
- Returns: `void`

**Delete(int id, CancellationToken cancellationToken)**
- Deletes an entity by ID
- Checks if entity exists before deletion
- Automatically saves changes
- Returns: Deleted `TEntity?` (null if not found)

### ICustomerRepository
Specific repository interface for Customer entities.

```csharp
public interface ICustomerRepository : IRepositoryBase<Customer>
{
    // Inherits all base repository methods
    // No additional methods currently defined
}
```

**Usage Examples:**
```csharp
// Get all customers
var customers = await _customerRepository.GetAll(cancellationToken);

// Get customer by ID
var customer = await _customerRepository.GetByID(1, cancellationToken);

// Find customer by name
var customer = await _customerRepository.Find(c => c.Name == "John Doe", cancellationToken);

// Create new customer
var newCustomer = new Customer { Name = "Jane Smith" };
var createdCustomer = await _customerRepository.Create(newCustomer, cancellationToken);

// Update customer
customer.Name = "Updated Name";
await _customerRepository.Update(customer, cancellationToken);

// Delete customer
var deletedCustomer = await _customerRepository.Delete(1, cancellationToken);
```

### IPostRepository
Specific repository interface for Post entities.

```csharp
public interface IPostRepository : IRepositoryBase<Post>
{
    // Inherits all base repository methods
    // No additional methods currently defined
}
```

**Usage Examples:**
```csharp
// Get all posts
var posts = await _postRepository.GetAll(cancellationToken);

// Get posts by customer
var customerPosts = await _postRepository.Finds(p => p.CustomerId == 1, cancellationToken);

// Find posts by category
var categoryPosts = await _postRepository.Finds(p => p.Category == "Farándula", cancellationToken);

// Create new post
var newPost = new Post 
{ 
    Title = "New Post", 
    Body = "Post content", 
    Type = 1, 
    Category = "Farándula", 
    CustomerId = 1 
};
var createdPost = await _postRepository.Create(newPost, cancellationToken);
```

---

## Repository Implementation

### RepositoryBase<TEntity>
Base implementation providing common functionality for all repositories.

```csharp
public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    internal readonly JujuTestContext _context;
    
    public RepositoryBase(JujuTestContext context) => _context = context;
    
    internal DbSet<TEntity> EntitySet => _context.Set<TEntity>();
    
    // Implementation details...
}
```

#### Key Implementation Features

**Dependency Injection**
- Receives `JujuTestContext` through constructor injection
- Provides access to Entity Framework DbContext

**Entity Set Access**
- `EntitySet` property provides typed access to DbSet<TEntity>
- Simplifies entity operations

**Asynchronous Operations**
- All operations are async for better performance
- Supports cancellation tokens for operation cancellation

**Change Tracking Optimization**
- Read operations use `AsNoTracking()` for better performance
- Modification operations use full change tracking

#### Performance Considerations

**Read Operations**
```csharp
public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken)
{
    return await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
}
```
- Uses `AsNoTracking()` to disable change tracking
- Improves performance for read-only scenarios
- Reduces memory usage for large result sets

**Write Operations**
```csharp
public async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken)
{
    var result = await EntitySet.AddAsync(entity, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    return result.Entity;
}
```
- Automatically saves changes after each operation
- Returns the created entity with generated values
- Handles database-generated properties (like IDs)

---

## Database Context Configuration

### JujuTestContext
Main Entity Framework DbContext with comprehensive entity configurations.

```csharp
public partial class JujuTestContext : DbContext
{
    public JujuTestContext(DbContextOptions<JujuTestContext> options) : base(options) { }
    
    // DbSets
    public virtual DbSet<Customer> Customer { get; set; }
    public virtual DbSet<Post> Post { get; set; }
    public virtual DbSet<Logs> Logs { get; set; }
    
    // Configuration methods...
}
```

### Entity Configurations

#### Customer Configuration
```csharp
modelBuilder.Entity<Customer>(entity =>
{
    entity.ToTable("Customer");
    entity.HasKey(e => e.CustomerId);
    entity.Property(e => e.CustomerId).ValueGeneratedOnAdd();
    entity.Property(e => e.Name).HasMaxLength(500).IsRequired();
    
    // One-to-many relationship with Posts
    entity.HasMany(e => e.Posts)
          .WithOne(e => e.Customer)
          .HasForeignKey(e => e.CustomerId)
          .OnDelete(DeleteBehavior.Cascade);
});
```

**Configuration Details:**
- **Table Name**: Maps to "Customer" table
- **Primary Key**: CustomerId with auto-generation
- **Name Property**: Required, max length 500 characters
- **Cascade Delete**: Deleting a customer automatically deletes all posts

#### Post Configuration
```csharp
modelBuilder.Entity<Post>(entity =>
{
    entity.ToTable("Post");
    entity.HasKey(e => e.PostId);
    entity.Property(e => e.PostId).ValueGeneratedOnAdd();
    entity.Property(e => e.Title).HasMaxLength(500).IsRequired();
    entity.Property(e => e.Body).HasMaxLength(500).IsRequired();
    entity.Property(e => e.Category).HasMaxLength(500).IsRequired();
    entity.Property(e => e.Type).IsRequired();
    entity.Property(e => e.CustomerId).IsRequired();
});
```

**Configuration Details:**
- **Table Name**: Maps to "Post" table
- **Primary Key**: PostId with auto-generation
- **String Properties**: All have max length 500 characters
- **Required Fields**: All properties are required
- **Foreign Key**: CustomerId references Customer table

#### Logs Configuration
```csharp
modelBuilder.Entity<Logs>(entity =>
{
    entity.ToTable("Logs");
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Id).ValueGeneratedOnAdd();
    entity.Property(e => e.Message).HasMaxLength(4000);
    entity.Property(e => e.MessageTemplate).HasMaxLength(4000);
    entity.Property(e => e.Level).HasMaxLength(50);
    entity.Property(e => e.Exception).HasMaxLength(4000);
    entity.Property(e => e.Properties).HasMaxLength(4000);
    entity.Property(e => e.TimeStamp).HasColumnType("datetime");
});
```

**Configuration Details:**
- **Table Name**: Maps to "Logs" table for Serilog integration
- **Large Text Fields**: Message, Exception, Properties (4000 chars)
- **DateTime Mapping**: TimeStamp uses SQL Server datetime type

---

## Dependency Injection Configuration

### DataAccessExtension
Extension method for registering data access services.

```csharp
public static class DataAccessExtension
{
    public static IServiceCollection AddDataAccessExtension(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        return services;
    }
}
```

### Complete Configuration Example
```csharp
// In Startup.cs ConfigureServices method
public void ConfigureServices(IServiceCollection services)
{
    // Database Context
    services.AddDbContext<JujuTestContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("Development")));
    
    // Repository Registration
    services.AddDataAccessExtension();
    
    // Other services...
}
```

---

## Database Relationships

### Entity Relationship Diagram
```
Customer (1) -----> (Many) Post
├── CustomerId (PK)         ├── PostId (PK)
├── Name                    ├── Title
└── Posts (Navigation)      ├── Body
                           ├── Type
                           ├── Category
                           ├── CustomerId (FK)
                           └── Customer (Navigation)
```

### Cascade Delete Behavior
- **Customer Deletion**: Automatically deletes all associated posts
- **Post Deletion**: Does not affect the associated customer
- **Data Integrity**: Foreign key constraints ensure referential integrity

---

## Connection String Configuration

### appsettings.json
```json
{
    "ConnectionStrings": {
        "Development": "Server=localhost;Database=JujuTests;Trusted_Connection=true;TrustServerCertificate=true;",
        "Production": "Server=prod-server;Database=JujuTests;User Id=username;Password=password;TrustServerCertificate=true;"
    }
}
```

### Environment-Specific Configuration
- **Development**: Uses Windows Authentication
- **Production**: Uses SQL Server Authentication
- **Trust Server Certificate**: Enabled for SSL connections

---

## Error Handling and Logging

### Database Exception Handling
```csharp
public async Task<TEntity> Create(TEntity entity, CancellationToken cancellationToken)
{
    try
    {
        var result = await EntitySet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity;
    }
    catch (DbUpdateException ex)
    {
        // Handle database constraint violations
        throw new DatabaseException("Failed to create entity", ex);
    }
    catch (SqlException ex)
    {
        // Handle SQL Server specific errors
        throw new DatabaseException("Database operation failed", ex);
    }
}
```

### Common Database Exceptions
- **DbUpdateException**: Constraint violations, validation errors
- **SqlException**: Connection issues, timeout errors
- **InvalidOperationException**: Context disposal, threading issues

---

## Performance Optimization

### Query Optimization
```csharp
// Efficient: Uses AsNoTracking for read-only operations
public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken)
{
    return await _context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
}

// Efficient: Direct FindAsync for primary key lookups
public async Task<TEntity?> GetByID(int id, CancellationToken cancellationToken)
{
    return await EntitySet.FindAsync(id, cancellationToken);
}
```

### Connection Pooling
- Entity Framework Core automatically manages connection pooling
- DbContext is registered as Scoped service for proper lifecycle management
- Connection strings should specify appropriate timeout values

### Bulk Operations
For bulk operations, consider using:
```csharp
// Bulk insert example
public async Task<IEnumerable<TEntity>> CreateMultiple(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
{
    await EntitySet.AddRangeAsync(entities, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);
    return entities;
}
```

---

## Testing Considerations

### Repository Testing
```csharp
[Test]
public async Task Create_Should_Add_Entity_And_Return_With_Id()
{
    // Arrange
    var options = new DbContextOptionsBuilder<JujuTestContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;
    
    using var context = new JujuTestContext(options);
    var repository = new CustomerRepository(context);
    var customer = new Customer { Name = "Test Customer" };
    
    // Act
    var result = await repository.Create(customer, CancellationToken.None);
    
    // Assert
    Assert.That(result.CustomerId, Is.GreaterThan(0));
    Assert.That(result.Name, Is.EqualTo("Test Customer"));
}
```

### Integration Testing
- Use SQL Server LocalDB or Docker containers for integration tests
- Test actual database constraints and relationships
- Verify transaction behavior and rollback scenarios

---

## Migration and Database Updates

### Code First Migrations
```bash
# Add new migration
dotnet ef migrations add MigrationName --project DataAccess

# Update database
dotnet ef database update --project DataAccess

# Generate SQL script
dotnet ef migrations script --project DataAccess
```

### Database Initialization
```csharp
public static class DatabaseInitializer
{
    public static async Task InitializeAsync(JujuTestContext context)
    {
        await context.Database.EnsureCreatedAsync();
        
        if (!context.Customer.Any())
        {
            // Seed initial data
            var customers = new List<Customer>
            {
                new Customer { Name = "Default Customer" }
            };
            
            context.Customer.AddRange(customers);
            await context.SaveChangesAsync();
        }
    }
}
```

This data access layer provides a solid foundation for database operations with proper separation of concerns, performance optimization, and maintainable code structure.