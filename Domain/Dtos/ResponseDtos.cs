using Domain.Enums;

namespace Domain.Dtos
{
    public class CustomerWithPostsDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public EntityStatus Status { get; set; }
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }

    public class PostWithCustomerDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
        public EntityStatus Status { get; set; }
        public CustomerDto Customer { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
} 