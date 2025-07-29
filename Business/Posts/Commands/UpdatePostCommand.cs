using Domain.Dtos;
using MediatR;

namespace Business.Posts.Commands
{
    public class UpdatePostCommand : IRequest<PostDto>
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Category { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
} 