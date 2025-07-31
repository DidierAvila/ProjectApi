using Domain.Dtos;
using MediatR;

namespace Business.Posts.Commands
{
    public class CreateMultiplePostsCommand : IRequest<IEnumerable<PostDto>>
    {
        public List<CreatePostRequest> Posts { get; set; } = new List<CreatePostRequest>();
    }

    public class CreatePostRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
} 