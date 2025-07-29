using Domain.Dtos;
using MediatR;

namespace Business.Posts.Commands
{
    public class CreatePostCommand : IRequest<PostDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
} 