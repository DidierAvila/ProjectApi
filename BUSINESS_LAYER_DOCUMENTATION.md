# Business Layer Documentation

## Overview

The Business layer implements the CQRS (Command Query Responsibility Segregation) pattern using MediatR. This layer contains all application logic, validation rules, and data transformation logic.

## Architecture Components

### Commands
Commands represent write operations (Create, Update, Delete) and follow the Command pattern.

### Queries
Queries represent read operations and are optimized for data retrieval.

### Handlers
Handlers process commands and queries, containing the actual business logic.

### Validators
Validators use FluentValidation to ensure data integrity and business rule compliance.

---

## Customer Management

### Commands

#### CreateCustomerCommand
Creates a new customer with validation.

```csharp
public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public string Name { get; set; }
}
```

**Handler: CreateCustomerCommandHandler**
- Validates customer name uniqueness
- Creates new customer entity
- Returns CustomerDto

**Validator: CreateCustomerCommandValidator**
- Name is required
- Name must be unique in the system

#### UpdateCustomerCommand
Updates an existing customer.

```csharp
public class UpdateCustomerCommand : IRequest<CustomerDto>
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
}
```

**Handler: UpdateCustomerCommandHandler**
- Validates customer exists
- Updates customer information
- Returns updated CustomerDto

**Validator: UpdateCustomerCommandValidator**
- CustomerId must be valid
- Name is required
- Name must be unique (excluding current customer)

#### DeleteCustomerCommand
Deletes a customer and all associated posts.

```csharp
public class DeleteCustomerCommand : IRequest
{
    public int CustomerId { get; set; }
}
```

**Handler: DeleteCustomerCommandHandler**
- Validates customer exists
- Deletes all associated posts first
- Deletes the customer
- No return value (void)

### Queries

#### GetAllCustomersQuery
Retrieves all customers.

```csharp
public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
    // No parameters needed
}
```

**Handler: GetAllCustomersQueryHandler**
- Retrieves all customers from repository
- Maps to CustomerDto collection
- Returns IEnumerable<CustomerDto>

#### GetCustomerByIdQuery
Retrieves a specific customer by ID.

```csharp
public class GetCustomerByIdQuery : IRequest<CustomerDto>
{
    public int CustomerId { get; set; }
}
```

**Handler: GetCustomerByIdQueryHandler**
- Validates customer exists
- Maps to CustomerDto
- Returns CustomerDto or null

#### GetCustomerPostsQuery
Retrieves all posts for a specific customer.

```csharp
public class GetCustomerPostsQuery : IRequest<IEnumerable<PostDto>>
{
    public int CustomerId { get; set; }
}
```

**Handler: GetCustomerPostsQueryHandler**
- Validates customer exists
- Retrieves customer's posts
- Maps to PostDto collection
- Returns IEnumerable<PostDto>

---

## Post Management

### Commands

#### CreatePostCommand
Creates a new post with business rule validation.

```csharp
public class CreatePostCommand : IRequest<PostDto>
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
}
```

**Handler: CreatePostCommandHandler**

**Business Rules Applied:**
1. **Customer Validation**: Verifies the associated customer exists
2. **Body Truncation**: If body exceeds 97 characters, truncates and adds "..."
3. **Category Assignment**:
   - Type 1 → "Farándula"
   - Type 2 → "Política"
   - Type 3 → "Futbol"
   - Other → Uses provided category

**Processing Flow:**
```csharp
// 1. Validate customer exists
var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
if (customer == null)
    throw new NotFoundException($"Customer with ID {command.CustomerId} not found");

// 2. Process body length
var processedBody = command.Body;
if (processedBody.Length > 97)
{
    processedBody = processedBody.Substring(0, 97) + "...";
}

// 3. Assign category based on type
var category = command.Type switch
{
    1 => "Farándula",
    2 => "Política",
    3 => "Futbol",
    _ => command.Category
};

// 4. Create and save post
var post = new Post
{
    Title = command.Title,
    Body = processedBody,
    Type = command.Type,
    Category = category,
    CustomerId = command.CustomerId
};

await _postRepository.AddAsync(post);
return _mapper.Map<PostDto>(post);
```

#### CreateMultiplePostsCommand
Creates multiple posts in a single transaction.

```csharp
public class CreateMultiplePostsCommand : IRequest<IEnumerable<PostDto>>
{
    public List<CreatePostRequest> Posts { get; set; }
}

public class CreatePostRequest
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
}
```

**Handler: CreateMultiplePostsCommandHandler**
- Validates all customers exist
- Applies same business rules as single post creation
- Creates all posts in a single transaction
- Returns collection of created PostDto objects

#### UpdatePostCommand
Updates an existing post.

```csharp
public class UpdatePostCommand : IRequest<PostDto>
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
}
```

**Handler: UpdatePostCommandHandler**
- Validates post exists
- Validates customer exists
- Applies same business rules as creation
- Updates post entity
- Returns updated PostDto

#### DeletePostCommand
Deletes a specific post.

```csharp
public class DeletePostCommand : IRequest
{
    public int PostId { get; set; }
}
```

**Handler: DeletePostCommandHandler**
- Validates post exists
- Deletes the post
- No return value

### Queries

#### GetAllPostsQuery
Retrieves all posts in the system.

```csharp
public class GetAllPostsQuery : IRequest<IEnumerable<PostDto>>
{
    // No parameters needed
}
```

**Handler: GetAllPostsQueryHandler**
- Retrieves all posts from repository
- Maps to PostDto collection
- Returns IEnumerable<PostDto>

#### GetPostByIdQuery
Retrieves a specific post by ID.

```csharp
public class GetPostByIdQuery : IRequest<PostDto>
{
    public int PostId { get; set; }
}
```

**Handler: GetPostByIdQueryHandler**
- Validates post exists
- Maps to PostDto
- Returns PostDto or null

#### GetPostsByCustomerIdQuery
Retrieves all posts for a specific customer.

```csharp
public class GetPostsByCustomerIdQuery : IRequest<IEnumerable<PostDto>>
{
    public int CustomerId { get; set; }
}
```

**Handler: GetPostsByCustomerIdQueryHandler**
- Retrieves posts filtered by customer ID
- Maps to PostDto collection
- Returns IEnumerable<PostDto>

---

## Validation Rules

### Customer Validation

#### CreateCustomerCommandValidator
```csharp
public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MustAsync(async (name, cancellation) => 
            {
                var existingCustomer = await customerRepository.GetByNameAsync(name);
                return existingCustomer == null;
            })
            .WithMessage("A customer with this name already exists");
    }
}
```

#### UpdateCustomerCommandValidator
```csharp
public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MustAsync(async (command, name, cancellation) => 
            {
                var existingCustomer = await customerRepository.GetByNameAsync(name);
                return existingCustomer == null || existingCustomer.CustomerId == command.CustomerId;
            })
            .WithMessage("A customer with this name already exists");
    }
}
```

### Post Validation

#### CreatePostCommandValidator
```csharp
public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator(ICustomerRepository customerRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Post title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Body)
            .NotEmpty()
            .WithMessage("Post body is required");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0")
            .MustAsync(async (customerId, cancellation) => 
            {
                var customer = await customerRepository.GetByIdAsync(customerId);
                return customer != null;
            })
            .WithMessage("The specified customer does not exist");

        RuleFor(x => x.Type)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Post type must be at least 1");
    }
}
```

#### UpdatePostCommandValidator
```csharp
public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator(IPostRepository postRepository, ICustomerRepository customerRepository)
    {
        RuleFor(x => x.PostId)
            .GreaterThan(0)
            .WithMessage("Post ID must be greater than 0")
            .MustAsync(async (postId, cancellation) => 
            {
                var post = await postRepository.GetByIdAsync(postId);
                return post != null;
            })
            .WithMessage("The specified post does not exist");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Post title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Body)
            .NotEmpty()
            .WithMessage("Post body is required");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0")
            .MustAsync(async (customerId, cancellation) => 
            {
                var customer = await customerRepository.GetByIdAsync(customerId);
                return customer != null;
            })
            .WithMessage("The specified customer does not exist");
    }
}
```

---

## AutoMapper Profiles

### CustomerMappingProfile
```csharp
public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<UpdateCustomerCommand, Customer>();
        CreateMap<Customer, CustomerWithPostsDto>()
            .ForMember(dest => dest.Posts, opt => opt.MapFrom(src => src.Posts));
    }
}
```

### PostMappingProfile
```csharp
public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Post, PostDto>();
        CreateMap<CreatePostCommand, Post>();
        CreateMap<UpdatePostCommand, Post>();
        CreateMap<CreatePostRequest, Post>();
        CreateMap<Post, PostWithCustomerDto>()
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer));
    }
}
```

---

## Exception Handling

### Custom Exceptions

#### NotFoundException
```csharp
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    
    public NotFoundException(string name, object key) 
        : base($"Entity \"{name}\" ({key}) was not found.") { }
}
```

#### ValidationException
```csharp
public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
```

#### BusinessRuleException
```csharp
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
```

---

## Dependency Injection Configuration

### Business Layer Registration
```csharp
public static class BusinessExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(typeof(CreateCustomerCommand));
        
        // Register AutoMapper
        services.AddAutoMapper(typeof(CustomerMappingProfile), typeof(PostMappingProfile));
        
        // Register FluentValidation
        services.AddValidatorsFromAssembly(typeof(CreateCustomerCommandValidator).Assembly);
        
        // Register pipeline behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        return services;
    }
}
```

### Pipeline Behaviors

#### ValidationBehavior
Automatically validates commands using FluentValidation:

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }

        return await next();
    }
}
```

#### LoggingBehavior
Logs all requests and responses:

```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        
        _logger.LogInformation("Handling {RequestName}: {@Request}", requestName, request);
        
        var response = await next();
        
        _logger.LogInformation("Handled {RequestName}: {@Response}", requestName, response);
        
        return response;
    }
}
```

---

## Testing Considerations

### Unit Testing
- Test each handler independently
- Mock repository dependencies
- Verify business rules are applied correctly
- Test validation logic

### Integration Testing
- Test complete command/query flow
- Verify database interactions
- Test transaction boundaries
- Validate AutoMapper configurations

### Example Unit Test
```csharp
[Test]
public async Task CreatePostCommandHandler_Should_Truncate_Body_When_Exceeds_97_Characters()
{
    // Arrange
    var command = new CreatePostCommand
    {
        Title = "Test Post",
        Body = new string('a', 120), // 120 characters
        Type = 1,
        Category = "Test",
        CustomerId = 1
    };

    _customerRepository.Setup(x => x.GetByIdAsync(1))
        .ReturnsAsync(new Customer { CustomerId = 1, Name = "Test Customer" });

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.That(result.Body, Is.EqualTo(new string('a', 97) + "..."));
    Assert.That(result.Category, Is.EqualTo("Farándula")); // Type 1 maps to Farándula
}
```

This business layer provides a robust, well-tested foundation for the application's core logic while maintaining separation of concerns and following SOLID principles.