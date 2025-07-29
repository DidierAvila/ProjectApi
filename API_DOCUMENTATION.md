# Post Ltda. API Documentation

## Overview

This is a comprehensive REST API for managing customers and posts, built with .NET Core using Clean Architecture principles. The API follows CQRS (Command Query Responsibility Segregation) pattern with MediatR for handling commands and queries.

## Base URL

```
https://localhost:5001
```

## Architecture

The project follows Clean Architecture with the following layers:
- **API**: Controllers and HTTP endpoints
- **Business**: Application logic, commands, queries, and handlers
- **Domain**: Entities, DTOs, and validators
- **DataAccess**: Repository pattern and database context

## Authentication

Currently, the API does not implement authentication. All endpoints are publicly accessible.

---

## Customer Management API

### Data Models

#### Customer Entity
```csharp
public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
}
```

#### Customer DTOs

**CustomerDto** - Response model
```csharp
public class CustomerDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
}
```

**CreateCustomerDto** - Request model for creating customers
```csharp
public class CreateCustomerDto
{
    public string Name { get; set; } // Required
}
```

**UpdateCustomerDto** - Request model for updating customers
```csharp
public class UpdateCustomerDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; } // Required
}
```

### Customer Endpoints

#### 1. Get All Customers

**GET** `/Customer`

Retrieves all customers in the system.

**Response:**
- **200 OK**: Array of CustomerDto objects
- **500 Internal Server Error**: Server error

**Example Request:**
```http
GET /Customer
Accept: application/json
```

**Example Response:**
```json
[
    {
        "customerId": 1,
        "name": "John Doe"
    },
    {
        "customerId": 2,
        "name": "Jane Smith"
    }
]
```

#### 2. Get Customer by ID

**GET** `/Customer/{id}`

Retrieves a specific customer by their ID.

**Parameters:**
- `id` (int, path): Customer ID

**Response:**
- **200 OK**: CustomerDto object
- **404 Not Found**: Customer not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
GET /Customer/1
Accept: application/json
```

**Example Response:**
```json
{
    "customerId": 1,
    "name": "John Doe"
}
```

**Error Response (404):**
```json
"No se encontró el cliente con ID 1"
```

#### 3. Create Customer

**POST** `/Customer`

Creates a new customer. Validates that no customer with the same name already exists.

**Request Body:** CreateCustomerDto

**Response:**
- **201 Created**: CustomerDto object with Location header
- **400 Bad Request**: Validation errors or duplicate name
- **500 Internal Server Error**: Server error

**Example Request:**
```http
POST /Customer
Content-Type: application/json

{
    "name": "Alice Johnson"
}
```

**Example Response:**
```json
{
    "customerId": 3,
    "name": "Alice Johnson"
}
```

**Validation Rules:**
- Name is required
- Name must be unique across all customers

#### 4. Update Customer

**PUT** `/Customer/{id}`

Updates an existing customer's information.

**Parameters:**
- `id` (int, path): Customer ID

**Request Body:** UpdateCustomerDto

**Response:**
- **200 OK**: Updated CustomerDto object
- **400 Bad Request**: Validation errors or ID mismatch
- **404 Not Found**: Customer not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
PUT /Customer/1
Content-Type: application/json

{
    "customerId": 1,
    "name": "John Doe Updated"
}
```

**Example Response:**
```json
{
    "customerId": 1,
    "name": "John Doe Updated"
}
```

**Validation Rules:**
- URL ID must match the CustomerId in the request body
- Name is required

#### 5. Delete Customer

**DELETE** `/Customer/{id}`

Deletes a customer and all associated posts.

**Parameters:**
- `id` (int, path): Customer ID

**Response:**
- **204 No Content**: Customer deleted successfully
- **404 Not Found**: Customer not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
DELETE /Customer/1
```

**Business Rules:**
- All posts associated with the customer are automatically deleted before the customer is removed

#### 6. Get Customer Posts

**GET** `/Customer/{id}/posts`

Retrieves all posts for a specific customer.

**Parameters:**
- `id` (int, path): Customer ID

**Response:**
- **200 OK**: Array of PostDto objects
- **404 Not Found**: Customer not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
GET /Customer/1/posts
Accept: application/json
```

**Example Response:**
```json
[
    {
        "postId": 1,
        "title": "My First Post",
        "body": "This is the content of my first post...",
        "type": 1,
        "category": "Farándula",
        "customerId": 1
    }
]
```

---

## Post Management API

### Data Models

#### Post Entity
```csharp
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}
```

#### Post DTOs

**PostDto** - Response model
```csharp
public class PostDto
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
}
```

**CreatePostCommand** - Request model for creating posts
```csharp
public class CreatePostCommand
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
}
```

**UpdatePostCommand** - Request model for updating posts
```csharp
public class UpdatePostCommand
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
}
```

### Post Type Categories

The system automatically assigns categories based on the Type field:

| Type | Category |
|------|----------|
| 1    | Farándula |
| 2    | Política |
| 3    | Futbol |
| Other | User-defined |

### Post Endpoints

#### 1. Get All Posts

**GET** `/Post`

Retrieves all posts in the system.

**Response:**
- **200 OK**: Array of PostDto objects
- **500 Internal Server Error**: Server error

**Example Request:**
```http
GET /Post
Accept: application/json
```

**Example Response:**
```json
[
    {
        "postId": 1,
        "title": "Celebrity News",
        "body": "Latest celebrity gossip and entertainment news...",
        "type": 1,
        "category": "Farándula",
        "customerId": 1
    },
    {
        "postId": 2,
        "title": "Election Update",
        "body": "Current political developments and election coverage...",
        "type": 2,
        "category": "Política",
        "customerId": 2
    }
]
```

#### 2. Get Post by ID

**GET** `/Post/{id}`

Retrieves a specific post by its ID.

**Parameters:**
- `id` (int, path): Post ID

**Response:**
- **200 OK**: PostDto object
- **404 Not Found**: Post not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
GET /Post/1
Accept: application/json
```

**Example Response:**
```json
{
    "postId": 1,
    "title": "Celebrity News",
    "body": "Latest celebrity gossip and entertainment news...",
    "type": 1,
    "category": "Farándula",
    "customerId": 1
}
```

#### 3. Create Post

**POST** `/Post`

Creates a new post with automatic validation and processing.

**Request Body:** CreatePostCommand

**Response:**
- **201 Created**: PostDto object with Location header
- **400 Bad Request**: Validation errors
- **500 Internal Server Error**: Server error

**Example Request:**
```http
POST /Post
Content-Type: application/json

{
    "title": "Football Match Results",
    "body": "Today's football match was incredible with amazing goals and fantastic plays from both teams. The crowd was enthusiastic throughout the entire game.",
    "type": 3,
    "category": "Sports",
    "customerId": 1
}
```

**Example Response:**
```json
{
    "postId": 3,
    "title": "Football Match Results",
    "body": "Today's football match was incredible with amazing goals and fantastic plays from both teams...",
    "type": 3,
    "category": "Futbol",
    "customerId": 1
}
```

**Business Rules:**
1. **Customer Validation**: The associated customer must exist
2. **Body Length**: If body text exceeds 97 characters, it's truncated and "..." is appended
3. **Category Assignment**:
   - Type 1 → "Farándula"
   - Type 2 → "Política"  
   - Type 3 → "Futbol"
   - Other types → Use provided category

**Validation Rules:**
- Title is required
- Body is required
- CustomerId must reference an existing customer

#### 4. Create Multiple Posts

**POST** `/Post/multiple`

Creates multiple posts in a single request.

**Request Body:** CreateMultiplePostsCommand

```csharp
public class CreateMultiplePostsCommand
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

**Response:**
- **201 Created**: Array of created PostDto objects
- **400 Bad Request**: Validation errors
- **500 Internal Server Error**: Server error

**Example Request:**
```http
POST /Post/multiple
Content-Type: application/json

{
    "posts": [
        {
            "title": "Entertainment News",
            "body": "Latest updates from the entertainment industry",
            "type": 1,
            "category": "Entertainment",
            "customerId": 1
        },
        {
            "title": "Political Analysis",
            "body": "Deep dive into current political situation",
            "type": 2,
            "category": "News",
            "customerId": 1
        }
    ]
}
```

**Example Response:**
```json
[
    {
        "postId": 4,
        "title": "Entertainment News",
        "body": "Latest updates from the entertainment industry",
        "type": 1,
        "category": "Farándula",
        "customerId": 1
    },
    {
        "postId": 5,
        "title": "Political Analysis",
        "body": "Deep dive into current political situation",
        "type": 2,
        "category": "Política",
        "customerId": 1
    }
]
```

#### 5. Update Post

**PUT** `/Post/{id}`

Updates an existing post.

**Parameters:**
- `id` (int, path): Post ID

**Request Body:** UpdatePostCommand

**Response:**
- **200 OK**: Updated PostDto object
- **400 Bad Request**: Validation errors or ID mismatch
- **404 Not Found**: Post not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
PUT /Post/1
Content-Type: application/json

{
    "postId": 1,
    "title": "Updated Celebrity News",
    "body": "Updated content about celebrity news and entertainment",
    "type": 1,
    "category": "Entertainment",
    "customerId": 1
}
```

**Example Response:**
```json
{
    "postId": 1,
    "title": "Updated Celebrity News",
    "body": "Updated content about celebrity news and entertainment",
    "type": 1,
    "category": "Farándula",
    "customerId": 1
}
```

**Validation Rules:**
- URL ID must match the PostId in the request body
- Same business rules as Create Post apply

#### 6. Delete Post

**DELETE** `/Post/{id}`

Deletes a specific post.

**Parameters:**
- `id` (int, path): Post ID

**Response:**
- **204 No Content**: Post deleted successfully
- **404 Not Found**: Post not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
DELETE /Post/1
```

#### 7. Get Posts by Customer ID

**GET** `/Post/customer/{customerId}`

Retrieves all posts for a specific customer.

**Parameters:**
- `customerId` (int, path): Customer ID

**Response:**
- **200 OK**: Array of PostDto objects
- **404 Not Found**: Customer not found
- **500 Internal Server Error**: Server error

**Example Request:**
```http
GET /Post/customer/1
Accept: application/json
```

**Example Response:**
```json
[
    {
        "postId": 1,
        "title": "My First Post",
        "body": "This is my first post content...",
        "type": 1,
        "category": "Farándula",
        "customerId": 1
    },
    {
        "postId": 2,
        "title": "Another Post",
        "body": "More content from the same customer...",
        "type": 2,
        "category": "Política",
        "customerId": 1
    }
]
```

---

## Response Models

### Standard Response Models

#### ApiResponse<T>
Generic wrapper for API responses:
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }
}
```

#### PaginatedResponse<T>
For paginated results:
```csharp
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
```

#### CustomerWithPostsDto
Customer with their posts:
```csharp
public class CustomerWithPostsDto
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public List<PostDto> Posts { get; set; }
}
```

#### PostWithCustomerDto
Post with customer information:
```csharp
public class PostWithCustomerDto
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Type { get; set; }
    public string Category { get; set; }
    public int CustomerId { get; set; }
    public CustomerDto Customer { get; set; }
}
```

---

## Error Handling

The API uses standard HTTP status codes and provides descriptive error messages.

### Common HTTP Status Codes

| Status Code | Description |
|-------------|-------------|
| 200 OK | Request successful |
| 201 Created | Resource created successfully |
| 204 No Content | Request successful, no content to return |
| 400 Bad Request | Invalid request data or validation errors |
| 404 Not Found | Resource not found |
| 500 Internal Server Error | Server error |

### Error Response Format

```json
{
    "success": false,
    "message": "Error description",
    "data": null,
    "errors": [
        "Specific error message 1",
        "Specific error message 2"
    ]
}
```

### Validation Errors

Validation errors return detailed information about which fields failed validation:

```json
{
    "Name": [
        "The Name field is required."
    ],
    "CustomerId": [
        "The CustomerId field must be greater than 0."
    ]
}
```

---

## Business Rules Summary

### Customer Management
1. Customer names must be unique
2. When deleting a customer, all associated posts are automatically deleted
3. Customer name is required for all operations

### Post Management
1. Posts must be associated with an existing customer
2. Post body text is automatically truncated to 97 characters if longer, with "..." appended
3. Categories are automatically assigned based on Type:
   - Type 1 → "Farándula"
   - Type 2 → "Política"
   - Type 3 → "Futbol"
   - Other → User-defined category
4. Title and Body are required fields
5. Multiple posts can be created in a single operation

---

## Development Setup

### Prerequisites
- .NET 6.0 or later
- SQL Server
- Visual Studio 2022 or VS Code

### Database Setup
1. Restore the database using `JujuTests.bak` file
2. Or execute the SQL script `JujuTests.Script.sql`
3. Update the connection string in `appsettings.json`

### Running the API
1. Clone the repository
2. Open the solution in Visual Studio
3. Set the API project as startup project
4. Run the application (F5)
5. Navigate to `https://localhost:5001/swagger` for API documentation

### Configuration
Update `appsettings.json` with your database connection:
```json
{
    "ConnectionStrings": {
        "Development": "Server=localhost;Database=JujuTests;Trusted_Connection=true;"
    }
}
```

---

## Testing

The API includes comprehensive validation and error handling. Use the Swagger UI at `/swagger` to test all endpoints interactively.

### Sample Test Scenarios

1. **Create Customer**: Test with valid and invalid names
2. **Create Post**: Test with different types and body lengths
3. **Bulk Post Creation**: Test creating multiple posts at once
4. **Customer Deletion**: Verify posts are deleted with customer
5. **Validation**: Test required fields and data constraints

---

## Logging

The API uses Serilog for structured logging:
- Console output for development
- File logging to `logs/app-{date}.log`
- Configurable log levels
- Request/response logging middleware

---

## Architecture Components

### Controllers
- `CustomerController`: Handles customer-related HTTP requests
- `PostController`: Handles post-related HTTP requests

### Business Layer (CQRS)
- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations
- **Handlers**: Process commands and queries
- **Validators**: FluentValidation for input validation

### Data Access
- Entity Framework Core with SQL Server
- Repository pattern implementation
- Database context: `JujuTestContext`

### Domain
- **Entities**: Core business objects (Customer, Post)
- **DTOs**: Data transfer objects for API contracts
- **Validators**: Domain-specific validation rules

This API provides a robust foundation for managing customers and posts with comprehensive validation, error handling, and business rule enforcement.