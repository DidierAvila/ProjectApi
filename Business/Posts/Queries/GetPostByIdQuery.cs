using Domain.Dtos;
using MediatR;

namespace Business.Posts.Queries
{
    public class GetPostByIdQuery : IRequest<PostDto?>
    {
        public int PostId { get; set; }
    }
} 