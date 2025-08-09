using Domain.Dtos;
using MediatR;

namespace Business.Posts.Commands
{
    public class CancelPostCommand : IRequest<PostDto>
    {
        public int PostId { get; set; }
    }
}